using System;
using Library.Clinic.Models;

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

            Physicians = new List<Physician>();
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

        private List<Physician> physicians;

        public List<Physician> Physicians { 
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
        public void AddOrUpdatePhysician(Physician physician)
        {
            bool isAdd = false;
            if (physician.Id <= 0)
            {
                physician.Id = LastKey + 1;
                isAdd = true;
            }

            if(isAdd)
            {
                Physicians.Add(physician);
            }

        }

        public void DeletePhysician(int id){
            //grabs the first physician that has that ID, or the default if not found
            var physicianToRemove = Physicians.FirstOrDefault(p => p.Id == id);
            if(physicianToRemove != null){
                Physicians.Remove(physicianToRemove);
            }
        }
    }
}
