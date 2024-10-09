using System;
using System.Runtime.CompilerServices;
using Library.Clinic.Models;

namespace App.Clinic.ViewModels;

public class PatientViewModel
{
    private Patient? model { get; set;}
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

    public DateOnly Birthday{
        get => model?.Birthday ?? DateOnly.MinValue; 

        set{
            if(model != null){
                model.Birthday = value;
            }
        }
    }

    public string Address{
        get => model?.Address ?? string.Empty;
        set{
            if(model != null){
                model.Address = value;
            }
        }
    }

    public string Race{
        get => model?.Race ?? string.Empty;
        set{
            if(model != null){
                model.Race = value;
            }
        }
    }

    public string Gender{
        get => model?.Gender ?? string.Empty;
        set{
            if(model != null){
                model.Gender = value;
            }
        }
    }

    public PatientViewModel(){
        model = new Patient();
    }

    //conversion constructor
    public PatientViewModel(Patient? _model){
        model = _model;
    }
}
