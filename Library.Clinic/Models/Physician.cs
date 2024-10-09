using System;

namespace Library.Clinic.Models{
    public class Physician{
        public int Id {get; set;}
        public string Name {get; set;}
        public int LicenseNumber {get; set;}
        public DateOnly GradDate {get; set;}
        public string Specializations {get; set;}

        public Physician(){
            Name = string.Empty;
            GradDate = DateOnly.MinValue;
            Specializations = string.Empty;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} {LicenseNumber} {GradDate} {Specializations}";
        }
    }
}