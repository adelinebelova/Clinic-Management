using System;
using Api.Clinic.Database;
using Library.Clinic.Models;
using Library.Clinic.DTO;

namespace Api.Clinic.Enterprise;

public class PatientEC
{
    public PatientEC(){}

    public IEnumerable<PatientDTO> Patients{
        get{
            return FakeDatabase.Patients;
        }
    }

    public PatientDTO? GetById(int id){
        return FakeDatabase.Patients.FirstOrDefault(p => p.Id == id);
    }
    public PatientDTO? Delete(int id){
        var patientToDelete = FakeDatabase.Patients.FirstOrDefault(p => p.Id == id);
        if(patientToDelete != null){
            FakeDatabase.Patients.Remove(patientToDelete);
        }
        return patientToDelete;
    }

    public PatientDTO? AddOrUpdate(PatientDTO? patient){
        return FakeDatabase.AddOrUpdatePatient(patient);
    }

}
