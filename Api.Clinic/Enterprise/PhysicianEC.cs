using System;
using Api.Clinic.Database;
using Library.Clinic.Models;
using Library.Clinic.DTO;

namespace Api.Clinic.Enterprise;

public class PhysicianEC
{
    public PhysicianEC(){}

    public IEnumerable<PhysicianDTO> Physicians{
        get{
            return new MySQLContext().GetPhysicians().Select(p => new PhysicianDTO(p));
        }
    }

    public IEnumerable<PhysicianDTO>? Search(string query)
    {
        // return FakeDatabase.Physicians
        //     .Where(p => p.Name.ToUpper()
        //         .Contains(query?.ToUpper() ?? string.Empty))
        //     .Select(p => new PhysicianDTO(p));

        return new MySQLContext().SearchPhysicians(query).Select(p => new PhysicianDTO(p));; 
    }

    public PhysicianDTO? GetById(int id){
        var physician = FakeDatabase.Physicians.FirstOrDefault(p => p.Id == id);
        if(physician != null){
            return new PhysicianDTO(physician);
        }
        return null;
     }
    // public PhysicianDTO? Delete(int id){
    //     var physicianToDelete = FakeDatabase.Physicians.FirstOrDefault(p => p.Id == id);
    //     if(physicianToDelete != null){
    //         FakeDatabase.Physicians.Remove(physicianToDelete);
    //         return new PhysicianDTO(physicianToDelete);
    //     }
    //     return null;
    // }

    // public Physician? AddOrUpdate(PhysicianDTO? physician){
    //     if(physician == null) return null;

    //     return FakeDatabase.AddOrUpdatePhysician(new Physician(physician));
    // }

    public Physician Add(Physician phy)
    {
        return new MySQLContext().AddPhysician(phy);
    }

    public Physician Update(Physician phy)
    {
        new MySQLContext().UpdatePhysician(phy);

        return phy;
    }

    public void Delete(int id)
    {
        new MySQLContext().DeletePhysician(id);
        return;
    }

}
