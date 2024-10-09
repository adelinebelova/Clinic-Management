using System;
using System.Runtime.CompilerServices;
using Library.Clinic.Models;

namespace App.Clinic.ViewModels;

public class AppointmentViewModel
{
    private Appointment? model { get; set;}
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

    public DateTime Schedule{
        get => model?.Schedule ?? DateTime.MinValue;

        set{
            if(model != null){
                model.Schedule = value;
            }
        }
    }

    public Patient Patient{
        get => model?.Patient ?? new Patient();

        set{
            if(model != null){
                model.Patient = value;
            }
        }
    }

    public Physician Physician{
        get => model?.Physician ?? new Physician();

        set{
            if(model != null){
                model.Physician = value;
            }
        }
    }
    
    public AppointmentViewModel()
    {
        model = new Appointment();
    }

    //conversion constructor
    public AppointmentViewModel(Appointment? _model){
        model = _model;
    }
}
