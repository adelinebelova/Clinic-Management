using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;

namespace App.Clinic.ViewModels;

public class TreatmentViewModel
{
    public Treatment? Model {get; set;}

    public int Id {
        get{
            if(Model == null){
                return -1;
            }

            return Model.Id;
        }
        set{
            if(Model != null && Model.Id != value){
                Model.Id = value;
            }
        }
    }
    public string Name{
        //you can use lambda expressions to return the getter here
        get => Model?.Name ?? string.Empty;
            
        set{
            if(Model != null){
                Model.Name = value;
            }
        }
    }

    public double Price{
        get => Model?.Price ?? 0.00;

        set{
            if(Model != null){
                Model.Price = value;
            }
        }
    }

    public ICommand? DeleteCommand {get; set;}
    public ICommand? EditCommand { get; set; }

    public void SetupCommands(){
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as TreatmentViewModel));
    }

    private void DoDelete(){
        if(Id > 0){
            TreatmentServiceProxy.Current.DeleteTreatment(Id);
            Shell.Current.GoToAsync("//Treatments");
        }
    }

    private void DoEdit(TreatmentViewModel? tvm){
        if (tvm == null){
            return;
        }
        var selectedTreatmentId = tvm?.Id ?? 0;
        Shell.Current.GoToAsync($"//TreatmentDetails?treatmentId={selectedTreatmentId}");
    } 

    public TreatmentViewModel(){
        Model = new Treatment();
        SetupCommands();
    }

    //conversion constructor
    public TreatmentViewModel(Treatment? _model){
        Model = _model;
        SetupCommands();
    }

    public void AddOrUpdate()
    {
        if(Model != null)
        {
            TreatmentServiceProxy.Current.AddOrUpdate(Model);           
        }
        
    }
}
