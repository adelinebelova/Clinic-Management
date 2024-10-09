using System;
using System.Runtime.CompilerServices;
using Library.Clinic.Models;

namespace App.Clinic.ViewModels;

public class PhysicianViewModel
{
    private Physician? model { get; set;}
    public int Id {
        get{
            if(model == null){
                return -1;
            }

            return model.Id;
        }
        set{
            if(model != null && model.Id != value){
                model.Id = value;
            }
        }
    }

    public string Name{
        //you can use lambda expressions to return the getter here
        get => model?.Name ?? string.Empty;
            
        set{
            if(model != null){
                model.Name = value;
            }
        }
    }

    public int LicenseNumber{
        //you can use lambda expressions to return the getter here
        get => model?.LicenseNumber ?? 0;
            
        set{
            if(model != null){
                model.LicenseNumber = value;
            }
        }
    }

    public DateOnly GradDate{
        get => model?.GradDate ?? DateOnly.MinValue; 

        set{
            if(model != null){
                model.GradDate = value;
            }
        }
    }

    public string Specializations{
        //you can use lambda expressions to return the getter here
        get => model?.Specializations ?? string.Empty;
            
        set{
            if(model != null){
                model.Specializations = value;
            }
        }
    }

    public PhysicianViewModel(){
        model = new Physician();
    }

    //conversion constructor
    public PhysicianViewModel(Physician? _model){
        model = _model;
    }
}
