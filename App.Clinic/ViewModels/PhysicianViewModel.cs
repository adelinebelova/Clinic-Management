using System;
using System.Runtime.CompilerServices;
using Library.Clinic.Models;
using System.Windows.Input;
using Library.Clinic.Services;
using Library.Clinic.DTO;

namespace App.Clinic.ViewModels;

public class PhysicianViewModel
{
    public PhysicianDTO? Model { get; set;}
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

    public ICommand? DeleteCommand { get; set; }
    public ICommand? EditCommand { get; set; }

    public string Name{
        //you can use lambda expressions to return the getter here
        get => Model?.Name ?? string.Empty;
            
        set{
            if(Model != null){
                Model.Name = value;
            }
        }
    }

    public void SetupCommands()
    {
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as PhysicianViewModel));
    }

    private void DoDelete()
    {
        if (Id > 0)
        {
            PhysicianServiceProxy.Current.DeletePhysician(Id);
            Shell.Current.GoToAsync("//Physicians");
        }
    }

    private void DoEdit(PhysicianViewModel? pvm)
    {
        if (pvm == null)
        {
            return;
        }
        var selectedPhysicianId = pvm?.Id ?? 0;
        Shell.Current.GoToAsync($"//PhysicianDetails?physicianId={selectedPhysicianId}");
    }

    public async void DoAdd(){
        if (Model != null)
            {
                await PhysicianServiceProxy
                .Current
                .AddOrUpdatePhysician(Model);
            }

            await Shell.Current.GoToAsync("//Physicians");
    }

    public int LicenseNumber{
        //you can use lambda expressions to return the getter here
        get => Model?.LicenseNumber ?? 0;
            
        set{
            if(Model != null){
                Model.LicenseNumber = value;
            }
        }
    }

    public DateTime GradDate{
        get => Model.GradDate; 

        set{
            if(Model != null){
                Model.GradDate = value;
            }
        }
    }

    public string Specializations{
        //you can use lambda expressions to return the getter here
        get => Model?.Specializations ?? string.Empty;
            
        set{
            if(Model != null){
                Model.Specializations = value;
            }
        }
    }

    public PhysicianViewModel(){
        Model = new PhysicianDTO();
        SetupCommands();
    }

    //conversion constructor
    public PhysicianViewModel(PhysicianDTO? _model){
        Model = _model;
        SetupCommands();
    }
}
