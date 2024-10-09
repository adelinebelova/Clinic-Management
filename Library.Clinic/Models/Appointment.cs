using System;

namespace Library.Clinic.Models{
    public class Appointment
    {
        public int Id {get; set;}
        public DateTime Schedule {get; set;}
        public Patient Patient {get; set;}
        public Physician Physician {get; set;}
        public Appointment(){
            Schedule = DateTime.MinValue;
            Patient = new Patient();
            Physician = new Physician();
        }

        public override string ToString()
        {
            return $"[{Id}] {Schedule} {Patient.Name} {Physician.Name}";
        }
    }
}
