using App.Clinic.ViewModels;
using Library.Clinic.Services;
using Library.Clinic.Models;

namespace App.Clinic.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentView : ContentPage
{
    public int AppointmentId { get; set; }
	public AppointmentView()
	{
		InitializeComponent();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Appointments");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as AppointmentViewModel)?.AddOrUpdate();
        Shell.Current.GoToAsync("//Appointments");
    }

    private void AppointmentView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //TODO: this really needs to be in a view model
        if(AppointmentId > 0)
        {
            var model = AppointmentServiceProxy.Current
                .Appointments.FirstOrDefault(p => p.Id == AppointmentId);
            if(model != null)
            {
                BindingContext = new AppointmentViewModel(model);
            } else
            {
                BindingContext = new AppointmentViewModel();
            }
            
        } else
        {
            BindingContext = new AppointmentViewModel();
        }
        
    }

    private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        (BindingContext as AppointmentViewModel)?.RefreshTime();
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e){
        if (sender is CheckBox checkBox && checkBox.BindingContext is Treatment treatmentOption)
        {
            (BindingContext as AppointmentViewModel)?.AddorRemoveTreatments(treatmentOption);
        }
    }


}