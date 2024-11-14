using Library.Clinic.Models;
using Library.Clinic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Clinic.ViewModels;

//This will allow us to display the treatment options and select checkboxes without modifying Treatments
//universally. 
public class TreatmentOptionsViewModel: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Treatment Treatment {get; set;}

    public string Name => Treatment?.Name;
    public double Price => Treatment?.Price ?? 0;

    private bool isselected; 
    public bool isSelected{
        get => isselected; 
        set{
            if(isselected != value){
                isselected = value;
            }
        }
    }

    public TreatmentOptionsViewModel(Treatment treatment)
    {
        Treatment = treatment;
    }
}
