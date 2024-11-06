using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App.Clinic.ViewModels;

public class PatientViewModel
{
    public Patient? Model { get; set;}
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

    public ICommand? DeleteCommand {get; set;}
    public ICommand? EditCommand { get; set; }

    //Allows us to execute Edit and Delete with the Id associated with the row, instead 
    //of having to click on the row before hitting the button.
    public void SetupCommands(){
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as PatientViewModel));
    }

    private void DoDelete(){
        if(Id > 0){
            PatientServiceProxy.Current.DeletePatient(Id);
            Shell.Current.GoToAsync("//Patients");
        }
    }

    private void DoEdit(PatientViewModel? pvm){
        if (pvm == null){
            return;
        }
        var selectedPatientId = pvm?.Id ?? 0;
        Shell.Current.GoToAsync($"//PatientDetails?patientId={selectedPatientId}");
    }

    public void DoAdd(){
        if (Model != null)
        {
            PatientServiceProxy
            .Current
            .AddOrUpdatePatient(Model);
        }

        Shell.Current.GoToAsync("//Patients");
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

    public DateOnly Birthday{
        get => Model?.Birthday ?? DateOnly.MinValue; 

        set{
            if(Model != null){
                Model.Birthday = value;
            }
        }
    }

    public string Address{
        get => Model?.Address ?? string.Empty;
        set{
            if(Model != null){
                Model.Address = value;
            }
        }
    }

    public string Race{
        get => Model?.Race ?? string.Empty;
        set{
            if(Model != null){
                Model.Race = value;
            }
        }
    }

    public string Gender{
        get => Model?.Gender ?? string.Empty;
        set{
            if(Model != null){
                Model.Gender = value;
            }
        }
    }

    public string InsuranceProvider{
        get => Model?.InsuranceProvider ?? "N/A"; 
        set{
            if(Model != null){
                Model.InsuranceProvider = value;
            }
        }
    }

    public string PolicyNumber {
        get => Model?.PolicyNumber ?? "N/A"; 
        set{
            if(Model != null){
                Model.PolicyNumber = value;
            }
        }
    }

        public List<string> InsuranceProviders { get; set; } = new List<string>
        {
            "N/A", "UnitedHealthcare", "Blue Cross Blue Shield", "Cigna", "Humana", "Aetna", "Tricare" 
        };

    public PatientViewModel(){
        Model = new Patient();
        SetupCommands();
    }

    //conversion constructor
    public PatientViewModel(Patient? _model){
        Model = _model;
        SetupCommands();
    }
}
