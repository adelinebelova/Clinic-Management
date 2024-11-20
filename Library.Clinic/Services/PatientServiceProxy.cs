using System;
using Library.Clinic.Models;
using Library.Clinic.Util;
using Newtonsoft.Json;

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

            var patientsData = new WebRequestHandler().Get("/Patient").Result;

            Patients = JsonConvert.DeserializeObject<List<Patient>>(patientsData) ?? new List<Patient>();      
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

        //just dealing with copays for now. Eventually want to add deductibles
        public double GetCopay(Patient p){
            var copay = 0.00;
            if(p.InsuranceProvider != "N/A"){
                switch(p.InsuranceProvider){
                    case "UnitedHealthcare":
                        copay = 30.00;
                        break;
                    case "Blue Cross Blue Shield":
                        copay = 50.00;
                        break;
                    case "Cigna":
                        copay = 15.00;
                        break;
                    case "Humana":
                        copay = 25.00;
                        break;
                    case "Aetna":
                        copay = 40.00;
                        break;
                    case "Tricare":
                        copay = 45.00;
                        break;
                    default:
                        copay = 0.00;
                        break;
                }
            }
            //no insurance
            else{
                copay = 0.00;
            }
            return copay;
        }

    }
}