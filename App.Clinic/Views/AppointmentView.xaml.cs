using Library.Clinic.Models;
using Library.Clinic.Services;
using System.ComponentModel;

namespace App.Clinic.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentView : ContentPage
{
	public AppointmentView()
	{
		InitializeComponent();
	}
    public int AppointmentId { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Appointments");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        var appointmentToAdd = BindingContext as Appointment;
        if (appointmentToAdd != null)
        {
            AppointmentServiceProxy
            .Current
            .AddorUpdateAppointment(appointmentToAdd);
        }

        Shell.Current.GoToAsync("//Appointments");
    }

    private void AppointmentView_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if(AppointmentId > 0)
        {
            BindingContext = AppointmentServiceProxy.Current
                .Appointments.FirstOrDefault(p => p.Id == AppointmentId);
        } else
        {
            BindingContext = new Appointment();
        }
        
    }
}