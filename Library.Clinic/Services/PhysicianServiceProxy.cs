using System;
using Library.Clinic.DTO;
using Library.Clinic.Models;
using Library.Clinic.Util;
using Newtonsoft.Json;

namespace Library.Clinic.Services{
    public class PhysicianServiceProxy
    {
        private static object _lock = new object();
        public static PhysicianServiceProxy Current
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                    {
                        instance = new PhysicianServiceProxy();
                    }
                }
                return instance;
            }
        }

        private static PhysicianServiceProxy? instance;
        private PhysicianServiceProxy()
        {
            instance = null;

            var physicianData = new WebRequestHandler().Get("/Physician").Result;

            Physicians = JsonConvert.DeserializeObject<List<PhysicianDTO>>(physicianData) ?? new List<PhysicianDTO>();      
        }

        public int LastKey{
            get{
                if(Physicians.Any()){
                    return Physicians.Select(x => x.Id).Max();
                }
                else{
                    return 0;
                }
            }
        }

        private List<PhysicianDTO> physicians;

        public List<PhysicianDTO> Physicians { 
            get {
                return physicians;
            }

            private set
            {
                if (physicians != value)
                {
                    physicians = value;
                }
            }
        }

        public async Task<List<PhysicianDTO>> Search(string query) {

            var physiciansPayload = await new WebRequestHandler()
                .Post("/Physician/Search", new Query(query));

            Physicians = JsonConvert.DeserializeObject<List<PhysicianDTO>>(physiciansPayload)
                ?? new List<PhysicianDTO>();

            return Physicians;
        }
       public async Task<PhysicianDTO?> AddOrUpdatePhysician(PhysicianDTO physician)
        {
            var payload = await new WebRequestHandler().Post("/Physician", physician);
            var newPhysician = JsonConvert.DeserializeObject<PhysicianDTO>(payload);
            if(newPhysician != null && newPhysician.Id > 0 && physician.Id == 0)
            {
                //new patient to be added to the list
                Physicians.Add(newPhysician);
            } else if(newPhysician != null && physician != null && physician.Id > 0 && physician.Id == newPhysician.Id)
            {
                //edit, exchange the object in the list
                var currentPhysician = Physicians.FirstOrDefault(p => p.Id == newPhysician.Id);
                var index = Physicians.Count;
                if (currentPhysician != null)
                {
                    index = Physicians.IndexOf(currentPhysician);
                    Physicians.RemoveAt(index);
                }
                Physicians.Insert(index, newPhysician);
            }

            return newPhysician;
        }

        public async void DeletePhysician(int id) {
            var physicianToRemove = Physicians.FirstOrDefault(p => p.Id == id);

            if (physicianToRemove != null)
            {
                //removes it from the server and the client. Optimization thing
                Physicians.Remove(physicianToRemove);
                await new WebRequestHandler().Delete($"/Physician/{id}");
            }
        }
    }
}
