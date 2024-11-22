using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Library.Clinic.DTO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;

namespace App.Clinic.ViewModels
{
    public class AppointmentViewModel: INotifyPropertyChanged
    {
        //To update the insurance and price once treatments and patiens are selected.
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Appointment? Model { get; set; }

        public ObservableCollection<PatientDTO> Patients { 
            get
            {
                return new ObservableCollection<PatientDTO>(PatientServiceProxy.Current.Patients);
            }
        }

        public ObservableCollection<PhysicianDTO> Physicians {
            get{
                return new ObservableCollection<PhysicianDTO>(PhysicianServiceProxy.Current.Physicians);
            }
        }

        //This will allow us to display the treatment options and select checkboxes without modifying Treatments
        //universally. 
        public ObservableCollection<Treatment> TreatmentOptions{
            get{
                return new ObservableCollection<Treatment>(TreatmentServiceProxy.Current.Treatments);
            }
        }


        public void AddorRemoveTreatments(Treatment treatmentOption) { 
            if(Model != null && treatmentOption != null){
                if (treatmentOption.isSelected && !Model.Treatments.Contains(treatmentOption)){
                    Model.Treatments.Add(treatmentOption);
                }
                else if(!treatmentOption.isSelected && Model.Treatments.Contains(treatmentOption)){
                    Model.Treatments.Remove(treatmentOption);
                } 

                NotifyPropertyChanged(nameof(TotalWithoutInsurance));
                NotifyPropertyChanged(nameof(TotalWithInsurance));
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

        public PatientDTO? SelectedPatient { 
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
                    UpdateInsuranceProviderOnScreen();
                    NotifyPropertyChanged(nameof(IsPatientPhysicianSelected));
                }

            }
         }

        public PhysicianDTO? SelectedPhysician {
            get{
                return Model?.Physician;
            }
            set{
                var selectedPhysician = value;
                if(Model != null){
                    Model.Physician = selectedPhysician;
                    Model.PhysicianId = selectedPhysician?.Id ?? 0;
                    NotifyPropertyChanged(nameof(IsPatientPhysicianSelected));
                }
            }
        }

        public List<Treatment> SelectedTreatments{
            get => Model.Treatments;
        }

        public double TotalWithoutInsurance{
            get{
                if(Model != null && Model.Treatments != null){
                    var total = 0.00;
                    foreach(var treatment in Model.Treatments){
                        total += treatment.Price;
                    }
                    return total;
                }
                else{
                    return 0.00;
                }
            }
        }

        public double PatientCopay{
            get{
                if(Model != null && Model.Patient != null){
                    return PatientServiceProxy.Current.GetCopay(Model.Patient);
                }
                else{
                    return 0.00;
                }
            }
        }

        public double TotalWithInsurance{
            get{
                if(Model != null && Model.Patient != null && TotalWithoutInsurance > 0){
                    if(Model.Patient.InsuranceProvider != "N/A"){
                        //set total to copay based on insurance used
                        var newTotal = PatientCopay;

                        //if the copay is greater than the initial total cost, set it to the normal total
                        if(newTotal > TotalWithoutInsurance){
                            newTotal = TotalWithoutInsurance;
                        }

                        return newTotal;
                    }
                    //return the full price if patient doesn't have insurance
                    else{
                        return TotalWithoutInsurance;
                    }
                }
                return 0.00;
            }
        }

        public bool IsPhysicianAvailable{
            get{
                return AppointmentServiceProxy.Current.CheckPhysicianAvailability(Model);
            }       
        }
        //used to show the rest of the screen once patient and physician are selected
        public bool IsPatientPhysicianSelected{
            get
            {
                return SelectedPatient != null && SelectedPhysician != null;
            }
        }


        //get the insurance from patient selected in dropdown
        public string InsuranceProvider{
            get => SelectedPatient?.InsuranceProvider ?? "N/A"; 
            set{
                if(SelectedPatient != null){
                    SelectedPatient.InsuranceProvider = value;
                    NotifyPropertyChanged(nameof(InsuranceProvider));
                }
            }
        }

        private void UpdateInsuranceProviderOnScreen(){
            InsuranceProvider = SelectedPatient?.InsuranceProvider ?? "N/A";
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
                return Model?.Start?.Date ?? DateTime.Today;
            }

            set
            {
                if (Model != null)
                {
                    Model.Start = value;
                    RefreshTime();
                }
            }
        }

        public TimeSpan Start { get; set; }
        //each appointment will be one hour long.

        // this prevents the time from being printed. Used in the management.xml 
        public string DisplayAppointmentStartDate => StartDate.ToString("MM/dd/yyyy");

        //print just the time
        public string DisplayAppointmentStartTime => Model.Start?.ToString("hh:mm tt") ?? "";

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
                Model.Start = StartDate;
                Model.Start = Model.Start.Value.AddHours(Start.Hours);
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
                //create appointment if the physician is not already booked
                if(IsPhysicianAvailable){
                    AppointmentServiceProxy.Current.AddOrUpdate(Model);
                }           
            }
            
        }

    }
}
