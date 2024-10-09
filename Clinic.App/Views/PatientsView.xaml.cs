using Library.Clinic.Models;
using Library.Clinic.Services;
using System.ComponentModel;

namespace App.Clinic.Views;

[QueryProperty(nameof(PatientId), "patientId")]
public partial class PatientsView : ContentPage
{
	public PatientsView()
	{
		InitializeComponent();
		
	}
    public int PatientId { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Patients");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        var patientToAdd = BindingContext as Patient;
        if (patientToAdd != null)
        {
            PatientServiceProxy
            .Current
            .AddOrUpdatePatient(patientToAdd);
        }

        Shell.Current.GoToAsync("//Patients");
    }

    private void PatientsView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //TODO: this really needs to be in a view model
        if(PatientId > 0)
        {
            BindingContext = PatientServiceProxy.Current
                .Patients.FirstOrDefault(p => p.Id == PatientId);
        } else
        {
            BindingContext = new Patient();
        }
        
    }
}