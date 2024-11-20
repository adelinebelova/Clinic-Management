using System;
using Library.Clinic.Models;
using Library.Clinic.DTO;

namespace Api.Clinic.Database;

public static class FakeDatabase
{
    public static int LastKey{
        get{
            if(Patients.Any()){
                return Patients.Select(x => x.Id).Max();
            }
            return 0;
        }
    }

    private static List<PatientDTO> patients = new List<PatientDTO>{
        new PatientDTO{Id = 1, Name = "John Doe"},
        new PatientDTO{Id = 2, Name = "Jane Doe"}
    };
    public static List<PatientDTO> Patients{
        get{
            return patients;
        }
    
    }

    public static PatientDTO? AddOrUpdatePatient(PatientDTO? patient){
        if(patient == null) return null; 

        bool isAdd = false;
        if(patient.Id <= 0 ){
            patient.Id = LastKey + 1;
            isAdd = true;
        }
        if(isAdd){
            Patients.Add(patient);
        }

        return patient;
    }
}
