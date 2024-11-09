using App.Clinic.ViewModels;
using Library.Clinic.Models;
using Library.Clinic.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Clinic.Views;

public partial class TreatmentManagementView : ContentPage, INotifyPropertyChanged
{
	public TreatmentManagementView()
	{
		InitializeComponent();
        BindingContext = new TreatmentManagementViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//MainPage");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TreatmentDetails?treatmentId=0");
    }

    private void TreatmentManagement_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as TreatmentManagementViewModel)?.Refresh();
    }

    private void RefreshClicked(object sender, EventArgs e)
    {
        (BindingContext as TreatmentManagementViewModel)?.Refresh();
    }
}