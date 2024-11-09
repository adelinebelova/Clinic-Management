using Library.Clinic.Models;
using App.Clinic.ViewModels;
using Library.Clinic.Services;
using System.ComponentModel;

namespace App.Clinic.Views;

[QueryProperty(nameof(TreatmentId), "treatmentId")]
public partial class TreatmentView : ContentPage
{
	public TreatmentView()
	{
		InitializeComponent();
	}

    public int TreatmentId {get;set;}

        private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Treatments");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        (BindingContext as TreatmentViewModel)?.AddOrUpdate();
        Shell.Current.GoToAsync("//Treatments");
    }

    private void TreatmentView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //TODO: this really needs to be in a view model
        if(TreatmentId > 0)
        {
            var model = TreatmentServiceProxy.Current
                .Treatments.FirstOrDefault(p => p.Id == TreatmentId);
            if(model != null)
            {
                BindingContext = new TreatmentViewModel(model);
            } else
            {
                BindingContext = new TreatmentViewModel();
            }
            
        } else
        {
            BindingContext = new TreatmentViewModel();
        }
        
    }
}