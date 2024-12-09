using System;
using Library.Clinic.DTO;

namespace Library.Clinic.Models{
    public class Physician{
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? LicenseNumber {get; set;}
        public DateTime GradDate {get; set;}
        public string? Specializations {get; set;}

        public Physician(){
            Name = string.Empty;
            GradDate = DateTime.MinValue;
            Specializations = string.Empty;
        }

        public Physician(PhysicianDTO p){
            Id = p.Id;
            Name = p.Name;
            LicenseNumber = p.LicenseNumber;
            GradDate = p.GradDate;
            Specializations = p.Specializations;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} {LicenseNumber} {GradDate} {Specializations}";
        }
    }
}