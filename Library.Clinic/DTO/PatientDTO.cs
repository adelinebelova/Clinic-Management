using System;

namespace Library.Clinic.DTO;

public class PatientDTO
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public DateTime Birthday {get; set;}
    public string? Address {get; set;}
    public string? Race {get; set;}
    public string? Gender {get; set;}

    //insurance information. eventually add coverage type, effectives dates, copay/deductible. 
    public string? InsuranceProvider {get; set;}
    public string? PolicyNumber {get; set;}

    //based on what took place in appointment
    public List<string>? Treatments {get; set;}

    public List<string>? Diagnoses {get; set;}
    public List<string>? Prescriptions {get; set;}

    public override string ToString()
    {
        return $"[{Id}] {Name} {Birthday} {Address} {Race} {Gender}";
    }
}
