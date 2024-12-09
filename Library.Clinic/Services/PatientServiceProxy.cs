using System;
using Library.Clinic.DTO;
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

            Patients = JsonConvert.DeserializeObject<List<PatientDTO>>(patientsData) ?? new List<PatientDTO>();      
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

        private List<PatientDTO> patients; 
        public List<PatientDTO> Patients { 
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

        public async Task<List<PatientDTO>> Search(string query) {

            var patientsPayload = await new WebRequestHandler()
                .Post("/Patient/Search", new Query(query));

            Patients = JsonConvert.DeserializeObject<List<PatientDTO>>(patientsPayload)
                ?? new List<PatientDTO>();

            return Patients;
        }

        public async Task<PatientDTO?> AddOrUpdatePatient(PatientDTO patient)
        {
            var payload = await new WebRequestHandler().Post("/patient", patient);
            var newPatient = JsonConvert.DeserializeObject<PatientDTO>(payload);
            if(newPatient != null && newPatient.Id > 0 && patient.Id == 0)
            {
                //new patient to be added to the list
                Patients.Add(newPatient);
            } else if(newPatient != null && patient != null && patient.Id > 0 && patient.Id == newPatient.Id)
            {
                //edit, exchange the object in the list
                var currentPatient = Patients.FirstOrDefault(p => p.Id == newPatient.Id);
                var index = Patients.Count;
                if (currentPatient != null)
                {
                    index = Patients.IndexOf(currentPatient);
                    Patients.RemoveAt(index);
                }
                Patients.Insert(index, newPatient);
            }

            return newPatient;
        }

        public async void DeletePatient(int id) {
            var patientToRemove = Patients.FirstOrDefault(p => p.Id == id);

            if (patientToRemove != null)
            {
                //removes it from the server and the client. Optimization thing
                Patients.Remove(patientToRemove);
                await new WebRequestHandler().Delete($"/Patient/{id}");
            }
        }

        //just dealing with copays for now. Eventually want to add deductibles
        public double GetCopay(PatientDTO p){
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