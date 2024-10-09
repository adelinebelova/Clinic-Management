using System;
using Library.Clinic.Models;

namespace Library.Clinic.Services{
    public class AppointmentServiceProxy
    {
        private static object _lock = new object();
        public static AppointmentServiceProxy Current
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                    {
                        instance = new AppointmentServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static AppointmentServiceProxy? instance;
        private AppointmentServiceProxy()
        {
            instance = null;

            Appointments = new List<Appointment>();
        }

        public int LastKey{
            get{
                if(Appointments.Any()){
                    return Appointments.Select(x => x.Id).Max();
                }
                else{
                    return 0;
                }
            }
        }

        private List<Appointment> appointments;

        public List<Appointment> Appointments{
            get{
                return appointments;
            }
            private set{
                if(appointments != value){
                    appointments = value;
                }
            }
        }
        public void AddorUpdateAppointment(Appointment appointment){
            bool isAdd = false;
            if(appointment.Id <= 0){
                //set the next Id. Calculate the highest Id given and increment on that
                appointment.Id = LastKey + 1;
                isAdd = true;
            }

            //check if appointment date or time is outside of business hours
            if(appointment.Schedule.TimeOfDay < new TimeSpan(8, 0, 0) || appointment.Schedule.TimeOfDay > new TimeSpan(17, 0, 0) || appointment.Schedule.DayOfWeek == DayOfWeek.Saturday || appointment.Schedule.DayOfWeek == DayOfWeek.Sunday) {
                return;
            }

            //conditions for appointment
            foreach (var app in Appointments){
                TimeSpan timeDifference = appointment.Schedule - app.Schedule;

                //Physician is already booked for this time.
                if(appointment.Schedule == app.Schedule && appointment.Physician == app.Physician){
                    return;
                }
                //Patient is already booked for this time.
                else if(appointment.Schedule == app.Schedule && appointment.Patient == app.Patient){
                    return;
                }
                //This patient already has an appointment with this physician.
                else if(appointment.Physician == app.Physician && appointment.Patient == app.Patient){
                    return;
                }
                //This patient already has an appointment within an hour of this one.
                else if(Math.Abs(timeDifference.TotalMinutes) <= 60 && app.Patient == appointment.Patient){
                    return;
                }
                //This physician already has an appointment within an hour of this one.
                else if(Math.Abs(timeDifference.TotalMinutes) <= 60 && app.Physician == appointment.Physician){
                    return;
                }
            }     

            if(isAdd){
                Appointments.Add(appointment);
            }
        }

        public void CancelAppointment(int id){
            //grabs the first appointment that has that ID
            var appointmentToCancel = Appointments.FirstOrDefault(p => p.Id == id);
            if(appointmentToCancel != null){
                Appointments.Remove(appointmentToCancel);
            }
        }
    }
}

