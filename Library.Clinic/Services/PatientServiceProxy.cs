using System;
using Library.Clinic.Models;

namespace Library.Clinic.Services{
    public class PatientServiceProxy
    {
        private static object _lock = new object();
        public static PatientServiceProxy Current
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                    {
                        instance = new PatientServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static PatientServiceProxy? instance;
        private PatientServiceProxy()
        {
            instance = null;


            Patients = new List<Patient>{ new Patient{Id = 0, Name = "bob"}};
        }
        public int LastKey
        {
            get
            {
                if(Patients.Any())
                {
                    return Patients.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        private List<Patient> patients; 
        public List<Patient> Patients { 
            get {
                return patients;
            }

            private set
            {
                if (patients != value)
                {
                    patients = value;
                }
            }
        }

        public void AddOrUpdatePatient(Patient patient)
        {
            bool isAdd = false;
            if (patient.Id <= 0)
            {
                patient.Id = LastKey + 1;
                isAdd = true;
            }

            if(isAdd)
            {
                Patients.Add(patient);
            }

        }

        public void DeletePatient(int id) {
            var patientToRemove = Patients.FirstOrDefault(p => p.Id == id);

            if (patientToRemove != null)
            {
                Patients.Remove(patientToRemove);
            }
        }

        public void AddPrescription(int id, string prescription){
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if(patient != null){
                patient.Prescriptions.Add(prescription);
            }
        }

        public void AddDiagnoses(int id, string diagnoses){
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if(patient != null){
                patient.Diagnoses.Add(diagnoses);
            }
        }
    }
}