using System;

namespace Library.Clinic.Models{
    public class Appointment
    {
        public Appointment() { }
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public int PhysicianId { get; set;}

        public Physician? Physician { get; set; }
    }
}
