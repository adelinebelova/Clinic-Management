using Library.Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Clinic.Services
{
    public class AppointmentServiceProxy
    {
        private static object _lock = new object();
        private int lastKey
        {
            get
            {
                if (Appointments.Any())
                {
                    return Appointments.Select(x => x.Id).Max();
                }
                return 0;
            }
        }
        public List<Appointment> Appointments { get; private set; }

        private static AppointmentServiceProxy _instance;
        public static AppointmentServiceProxy Current
        {
            get
            {

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppointmentServiceProxy();
                    }
                }
                return _instance;
            }
        }

        private AppointmentServiceProxy()
        {
            Appointments = new List<Appointment>();
        }

        public void AddOrUpdate(Appointment a)
        {
            var isAdd = false;
            if(a.Id <= 0)
            {
                a.Id = lastKey + 1;
                isAdd = true;
            }

            if(isAdd)
            {
                Appointments.Add(a);
            }

        }

        public void CancelAppointment(int id) {
            var appointmentToRemove = Appointments.FirstOrDefault(p => p.Id == id);

            if (appointmentToRemove != null)
            {
                Appointments.Remove(appointmentToRemove);
            }
        }
    }
}