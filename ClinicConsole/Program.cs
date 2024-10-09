using System;
using Library.Clinics.Models;
using Library.Clinics.Services;

namespace Clinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input;
            bool isContinue = true;

            do{
                //menu
                Console.WriteLine("Select an option: "); 
                Console.WriteLine("A. Add a new patient"); 
                Console.WriteLine("B. Add a new physician");  
                Console.WriteLine("C. Remove a patient");
                Console.WriteLine("D. Remove a physician");
                Console.WriteLine("E. Create a new appointment");
                Console.WriteLine("F. Cancel an appointment");
                Console.WriteLine("G. View all patients and physicians");
                Console.WriteLine("H. View all appointments");
                Console.WriteLine("I. Add new diagnoses");
                Console.WriteLine("J. Add new prescription");
                Console.WriteLine("Q. Quit");   
                Console.Write("> ");

                input = Console.ReadLine() ?? string.Empty; 
                Console.WriteLine("");

                if(char.TryParse(input, out char choice))
                    switch(choice){
                        //add user
                        case 'a':
                        case 'A':
                            DateOnly patientBirthday; 
                            string patientName; 
                            string patientAddress; 
                            string patientRace; 
                            char patientGender;
                            

                            Console.WriteLine("Enter patient's name: ");
                            patientName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter patient's birthday (e.g 2001-09-12): "); 
                            if(DateOnly.TryParse(Console.ReadLine(), out patientBirthday)){
                                Console.WriteLine("Enter patient's address: ");
                                patientAddress = Console.ReadLine() ?? string.Empty;

                                Console.WriteLine("Enter patient's race: ");
                                patientRace = Console.ReadLine() ?? string.Empty;

                                Console.WriteLine("Enter patient's gender (one character): ");
                                if(char.TryParse(Console.ReadLine(), out patientGender)){


                                    var newPatient = new Patient{ Name = patientName, Birthday = patientBirthday, Address = patientAddress, Race = patientRace, Gender = patientGender };
                                    PatientServiceProxy.Current.AddOrUpdatePatient(newPatient);
                                }
                                else{
                                    Console.WriteLine("Please enter a single character.");
                                    Console.WriteLine("");
                                }
                            }
                            else{
                                Console.WriteLine("Please enter a valid date.");
                                Console.WriteLine("");
                            }

                            break; 
                        //add physician
                        case 'b':
                        case 'B':
                            string physicianName; 
                            int physicianLicense;
                            DateOnly gradDate;
                            string specializations;
                            Console.WriteLine("Enter physician's name: ");
                            physicianName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Enter physician's license number: ");
                            if(int.TryParse(Console.ReadLine(), out physicianLicense)){
                                Console.WriteLine("Enter physician's graduation date: ");
                                
                                if(DateOnly.TryParse(Console.ReadLine(), out gradDate)){
                                    Console.WriteLine("Enter any specializations, leave blank if none: ");
                                    specializations = Console.ReadLine() ?? string.Empty;

                                    var newPhysician = new Physician{ Name = physicianName, GradDate = gradDate, LicenseNumber = physicianLicense, Specializations = specializations};
                                    PhysicianServiceProxy.Current.AddPhysician(newPhysician);
                                }
                                else{
                                    Console.WriteLine("Please enter a valid date. ");
                                    Console.WriteLine("");
                                }
                            }
                            else{
                                Console.WriteLine("Please enter a valid integer.");
                                Console.WriteLine("");
                            }

                            break;
                        //delete patient
                        case 'c':
                        case 'C':
                            if(PatientServiceProxy.Current.Patients.Count == 0){
                                Console.WriteLine("There are no patients enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Enter patient's ID number");
                            PatientServiceProxy.Current.Patients.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );

                            int deletedPatient = int.Parse(Console.ReadLine() ?? "-1");
                            PatientServiceProxy.Current.DeletePatient(deletedPatient);
                            break;
                        //delete physician
                        case 'd':
                        case 'D':
                            if(PhysicianServiceProxy.Current.Physicians.Count == 0){
                                Console.WriteLine("There are no physicians enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Enter physician's ID number");
                            PhysicianServiceProxy.Current.Physicians.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );

                            int deletedPhysician = int.Parse(Console.ReadLine() ?? "-1");
                            PhysicianServiceProxy.Current.DeletePhysician(deletedPhysician);
                            break;
                        //create appointment
                        case 'e':
                        case 'E':
                            int physicianID;
                            int patientID;

                            if(PatientServiceProxy.Current.Patients.Count == 0){
                                Console.WriteLine("There are no patients enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            if(PhysicianServiceProxy.Current.Physicians.Count == 0){
                                Console.WriteLine("There are no physicians enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Enter patient's ID: ");
                            PatientServiceProxy.Current.Patients.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );
                            if(int.TryParse(Console.ReadLine(), out patientID)){

                                Console.WriteLine("Enter physician's Id: "); 
                                PhysicianServiceProxy.Current.Physicians.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );
                                 
                                if(int.TryParse(Console.ReadLine(), out physicianID)){
                                    var patient = PatientServiceProxy.Current.Patients.FirstOrDefault(p => p.Id == patientID);
                                    var physician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p.Id == physicianID);

                                    if(patient != null && physician != null){
                                        Console.WriteLine("Enter an appointment date and time (e.g., 2024-09-10 14:30)");
                                        DateTime schedule;

                                        if(DateTime.TryParse(Console.ReadLine(), out schedule)){
                                            Appointment newAppointment = new Appointment{ Physician = physician, Patient = patient, Schedule = schedule}; 
                                            AppointmentServiceProxy.Current.CreateAppointment(newAppointment);
                                        }
                                        else {
                                            Console.WriteLine("Please enter a valid date and time.");
                                            Console.WriteLine("");
                                        }
                                    }
                                    else{
                                        Console.WriteLine("Patient or physician does not exist.");
                                        Console.WriteLine("");
                                    }
                                }
                                else{
                                    Console.WriteLine("Please enter a valid physician ID.");
                                    Console.WriteLine("");
                                }
                            }
                            else{
                                Console.WriteLine("Please enter a valid patient ID.");
                                Console.WriteLine("");
                            }
                            break;
                        //cancel appointment
                        case 'f':
                        case 'F':
                            if(AppointmentServiceProxy.Current.Appointments.Count == 0){
                                Console.WriteLine("There are no appointments.");
                                Console.WriteLine("");
                                break;
                            }
                            Console.WriteLine("Enter appointment's ID number");
                            AppointmentServiceProxy.Current.Appointments.ForEach( x => Console.WriteLine($"{x.Id}. Physician: {x.Physician.Name}, Patient: {x.Patient.Name}, Schedule: {x.Schedule}") );

                            int cancelledApp = int.Parse(Console.ReadLine() ?? "-1");
                            AppointmentServiceProxy.Current.CancelAppointment(cancelledApp);
                            break;
                        //print patients and physicians
                        case 'g':
                        case 'G':
                            Console.WriteLine("Patients: ");
                            PatientServiceProxy.Current.Patients.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}, Birthday: {x.Birthday}, Address: {x.Address}, Race: {x.Race}, Gender: {x.Gender}") );
                            Console.WriteLine("Physicians: ");
                            PhysicianServiceProxy.Current.Physicians.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}, License #: {x.LicenseNumber}, Grad Date: {x.GradDate}, Specializations: {x.Specializations}") );
                            Console.WriteLine("");
                            break;
                        //print appointments
                        case 'h':
                        case 'H':
                            if(AppointmentServiceProxy.Current.Appointments.Count == 0){
                                Console.WriteLine("There are no appointments.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Appointments: ");
                            AppointmentServiceProxy.Current.Appointments.ForEach( x => Console.WriteLine($"{x.Id}. Physician: {x.Physician.Name}, Patient: {x.Patient.Name}, Schedule: {x.Schedule}"));
                            Console.WriteLine("");
                            break;
                        //new diagnoses
                        case 'i':
                        case 'I':
                            if(PatientServiceProxy.Current.Patients.Count == 0){
                                Console.WriteLine("There are no patients enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Enter patient's ID number");
                            PatientServiceProxy.Current.Patients.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );

                            int patientForDiagnoses = int.Parse(Console.ReadLine() ?? "-1");

                            Console.WriteLine("Enter diagnoses: ");
                            string diagnoses = Console.ReadLine() ?? string.Empty;

                            PatientServiceProxy.Current.AddDiagnoses(patientForDiagnoses, diagnoses);
                            break;
                        //new prescription
                        case 'j':
                        case 'J':
                            if(PatientServiceProxy.Current.Patients.Count == 0){
                                Console.WriteLine("There are no patients enrolled.");
                                Console.WriteLine("");
                                break;
                            }

                            Console.WriteLine("Enter patient's ID number");
                            PatientServiceProxy.Current.Patients.ForEach( x => Console.WriteLine($"{x.Id}. {x.Name}") );

                            int patientForPrescription = int.Parse(Console.ReadLine() ?? "-1");

                            Console.WriteLine("Enter prescription: ");
                            string prescription = Console.ReadLine() ?? string.Empty;

                            PatientServiceProxy.Current.AddPrescription(patientForPrescription, prescription);
                            break;
                        case 'q':
                        case 'Q':
                            Console.WriteLine("Quitting program.");
                            isContinue = false;
                            break;
                        default: 
                            Console.WriteLine("Invalid choice.");
                            Console.WriteLine("");
                            break;
                        }
                else{
                    Console.WriteLine($"{input} is not a char. ");
                    Console.WriteLine("");
                }
            } while(isContinue);
        }
    }
}