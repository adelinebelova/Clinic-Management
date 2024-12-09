using System;
using Library.Clinic.DTO;

namespace Library.Clinic.Models{
    public class Patient{
        public int Id {get; set;}
        public string Name {get; set;}
        public DateTime Birthday {get; set;}
        public string Address {get; set;}
        public string Race{get; set;}
        public string Gender {get; set;}

        //insurance information. eventually add coverage type, effectives dates, copay/deductible. 
        public string InsuranceProvider {get; set;}
        public string PolicyNumber {get; set;}

        //based on what took place in appointment
        public List<string> Treatments {get; set;}

        public Patient(){
            Name = string.Empty;  
            Address = string.Empty;
            Race = string.Empty;
            Gender = string.Empty;
        }

        public Patient(PatientDTO p){
            Id = p.Id;
            Name = p.Name;
            Birthday = p.Birthday;
            Address = p.Address;
            Race = p.Race;
            Gender = p.Gender;
            InsuranceProvider = p.InsuranceProvider;
            PolicyNumber = p.PolicyNumber;
            Treatments = p.Treatments;
        }
        public override string ToString()
        {
            return $"[{Id}] {Name} {Birthday} {Address} {Race} {Gender}";
        }
 
    }
}