using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App.Clinic.ViewModels
{
    public class AppointmentViewModel
    {
        public Appointment? Model { get; set; }

        public ObservableCollection<Patient> Patients { 
            get
            {
                return new ObservableCollection<Patient>(PatientServiceProxy.Current.Patients);
            }
        }

        public ObservableCollection<Physician> Physicians {
            get{
                return new ObservableCollection<Physician>(PhysicianServiceProxy.Current.Physicians);
            }
        }

        public int Id {
            get{
                if(Model == null){
                    return -1;
                }

                return Model.Id;
            }
            set{
                if(Model != null && Model.Id != value){
                    Model.Id = value;
                }
            }
        }

        public string PatientName
        {
            get
            {
                if(Model != null && Model.PatientId > 0)
                {
                    if(Model.Patient == null)
                    {
                        Model.Patient = PatientServiceProxy
                            .Current
                            .Patients
                            .FirstOrDefault(p => p.Id == Model.PatientId);
                    }
                }

                return Model?.Patient?.Name ?? string.Empty;
            }
        }

        public string PhysicianName
        {
            get
            {
                if(Model != null && Model.PhysicianId > 0)
                {
                    if(Model.Physician == null)
                    {
                        Model.Physician = PhysicianServiceProxy
                            .Current
                            .Physicians
                            .FirstOrDefault(p => p.Id == Model.PhysicianId);
                    }
                }

                return Model?.Physician?.Name ?? string.Empty;
            }
        }

        public Patient? SelectedPatient { 
            get
            {
                return Model?.Patient;
            }

            set
            {
                var selectedPatient = value;
                if(Model != null)
                {
                    Model.Patient = selectedPatient;
                    Model.PatientId = selectedPatient?.Id ?? 0;
                }

            }
         }

        public Physician? SelectedPhysician {
            get{
                return Model?.Physician;
            }
            set{
                var selectedPhysician = value;
                if(Model != null){
                    Model.Physician = selectedPhysician;
                    Model.PhysicianId = selectedPhysician?.Id ?? 0;
                }
            }
        }

        public DateTime MinStartDate
        {
            get
            {
                return DateTime.Today;
            }
        }

        public DateTime StartDate { 
            get
            {
                return Model?.StartTime?.Date ?? DateTime.Today;
            }

            set
            {
                if (Model != null)
                {
                    Model.StartTime = value;
                    RefreshTime();
                }
            }
        }

        public TimeSpan StartTime { get; set; }
        
        //don't let the user choose this; it will be updated for the type of appointment selected.
        public TimeSpan EndTime { get; set; }

        // this prevents the time from being printed. Used in the management.xml 
        public string DisplayAppointmentStartDate => StartDate.ToString("MM/dd/yyyy");

        //print just the time
        public string DisplayAppointmentStartTime => Model.StartTime?.ToString("hh:mm tt") ?? "";

        //Can't use TimePicker with mccatalyst, so I'm hardcoding times into a list here
        public List<TimeSpan> AvailableStartTimes { get; set; } = new List<TimeSpan>
        {
            new TimeSpan(8, 0, 0),
            new TimeSpan(9, 0, 0),
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            new TimeSpan(12, 0, 0),
            new TimeSpan(13, 0, 0),
            new TimeSpan(14, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(16, 0, 0),
            new TimeSpan(17, 0, 0)
        };

        public void RefreshTime()
        {
            if (Model != null)
            {
                if (Model.StartTime != null)
                {
                    Model.StartTime = StartDate;
                    Model.StartTime = Model.StartTime.Value.AddHours(StartTime.Hours);
                }
            }
        }

        public ICommand? DeleteCommand {get; set;}
        public ICommand? EditCommand { get; set; }

        //Allows us to execute Edit and Delete with the Id associated with the row, instead 
        //of having to click on the row before hitting the button.
        public void SetupCommands(){
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as AppointmentViewModel));
        }

        private void DoDelete(){
            if(Id > 0){
                AppointmentServiceProxy.Current.CancelAppointment(Id);
                Shell.Current.GoToAsync("//Appointments");
            }
        }

        private void DoEdit(AppointmentViewModel? avm){
            if (avm == null){
                return;
            }
            var selectedAppointmentId = avm?.Id ?? 0;
            Shell.Current.GoToAsync($"//AppointmentDetails?appointmentId={selectedAppointmentId}");
        }    

        public AppointmentViewModel() {
            Model = new Appointment();
            SetupCommands();
        }
        
        public AppointmentViewModel(Appointment a)
        {
            Model = a;
            SetupCommands();
        }

        public void AddOrUpdate()
        {
            if(Model != null)
            {
                AppointmentServiceProxy.Current.AddOrUpdate(Model);
            }
            
        }
    }
}
