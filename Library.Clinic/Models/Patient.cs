using System;

namespace Library.Clinic.Models{
    public class Patient{
        public int Id {get; set;}
        public string Name {get; set;}
        public DateOnly Birthday {get; set;}
        public string Address {get; set;}
        public string Race{get; set;}
        public string Gender {get; set;}

        public List<string> Diagnoses {get; set;}
        public List<string> Prescriptions {get; set;}

        public Patient(){
            Name = string.Empty;  
            Address = string.Empty;
            Race = string.Empty;
            Gender = string.Empty;
            Diagnoses = new List<string>(); 
            Prescriptions = new List<string>();
        }
        public override string ToString()
        {
            return $"[{Id}] {Name} {Birthday} {Address} {Race} {Gender}";
        }
 
    }
}