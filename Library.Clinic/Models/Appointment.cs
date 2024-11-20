using System;
using Library.Clinic.DTO;

namespace Library.Clinic.Models{
    public class Appointment
    {
        public Appointment() { Treatments = new List<Treatment>(); }
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        //each appointment will be 1 hour long. 
        public int PatientId { get; set; }
        public PatientDTO? Patient { get; set; }

        public int PhysicianId { get; set;}

        public Physician? Physician { get; set; }

        public List<Treatment>? Treatments {get; set;}

        public int TreatmentId {get; set;}
    }
}
