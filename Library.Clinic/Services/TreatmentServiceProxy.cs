using System;
using Library.Clinic.Models;

namespace Library.Clinic.Services;

public class TreatmentServiceProxy
{
    private static object _lock = new object();
    public static TreatmentServiceProxy Current{
        get{
            lock(_lock){
                if(instance == null){
                    instance = new TreatmentServiceProxy();
                }
            }
            return instance;
        }
    }

    private static TreatmentServiceProxy? instance;

    private TreatmentServiceProxy(){
        instance = null;
        Treatments = new List<Treatment>(); 
    }

    private List<Treatment> treatments;

    public List<Treatment> Treatments{
        get{
            return treatments; 
        }
        private set{
            if(treatments != value){
                treatments = value;
            }
        }
    }

    public int LastKey{
        get{
            if(Treatments.Any()){
                return Treatments.Select(x => x.Id).Max();
            }
            else{
                return 0;
            }
        }
    }

    public void AddOrUpdate(Treatment treatment)
    {
        bool isAdd = false;
        if (treatment.Id <= 0)
        {
            treatment.Id = LastKey + 1;
            isAdd = true;
        }

        if(isAdd)
        {
            Treatments.Add(treatment);
        }

    }

    public void DeleteTreatment(int id){
        //grabs the first patient that has that ID, or the default if not found
        var treatmentToRemove = Treatments.FirstOrDefault(p => p.Id == id);
        if(treatmentToRemove != null){
            Treatments.Remove(treatmentToRemove);
        }
    }

}
