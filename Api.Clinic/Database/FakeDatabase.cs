using System;
using Library.Clinic.Models;

namespace Api.Clinic.Database;

public static class FakeDatabase
{
    //generic LastKey for physician, appointment, and patient
    public static int LastKey<T>(IEnumerable<T> collection) where T : class
    {
        if (collection.Any())
        {
            return collection
                .Select(item => (int)item.GetType().GetProperty("Id").GetValue(item))
                .Max();
        }
        return 0;
    }


    private static List<Patient> patients = new List<Patient>{
        new Patient{Id = 1, Name = "John Doe"},
        new Patient{Id = 2, Name = "Jane Doe"}
    };
    public static List<Patient> Patients{
        get{
            return patients;
        }
    
    }

    public static Patient? AddOrUpdatePatient(Patient? patient){
        if(patient == null) return null; 

        bool isAdd = false;
        if(patient.Id <= 0 ){
            var lastKey = LastKey(Patients);
            patient.Id = lastKey + 1;
            isAdd = true;
        }
        if(isAdd){
            Patients.Add(patient);
        }

        return patient;
    }

    /*****  PHYSICIANS  ****/
    private static List<Physician> physicians = new List<Physician>{
        new Physician{Id = 1, Name = "John Doe"},
        new Physician{Id = 2, Name = "Jane Doe"}
    };
    public static List<Physician> Physicians{
        get{
            return physicians;
        }
    
    }

    public static Physician? AddOrUpdatePhysician(Physician? physician){
        if(physician == null) return null; 

        bool isAdd = false;
        if(physician.Id <= 0 ){
            var lastKey = LastKey(Physicians);
            physician.Id = lastKey + 1;
            isAdd = true;
        }
        if(isAdd){
            Physicians.Add(physician);
        }

        return physician;
    }

}
