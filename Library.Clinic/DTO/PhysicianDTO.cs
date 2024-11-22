using System;
using Library.Clinic.Models;

namespace Library.Clinic.DTO;

public class PhysicianDTO
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public int? LicenseNumber {get; set;}
    public DateTime GradDate {get; set;}
    public string? Specializations {get; set;}

    public PhysicianDTO(){}

    public PhysicianDTO(Physician p){
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
