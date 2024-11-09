using System;

namespace Library.Clinic.Models;

public class Treatment
{
    public int Id {get; set;}
    public string Name {get; set;}

    public double Price {get; set;}

    public Treatment(){
        Name = string.Empty;
        Price = 0.00;
    }
}
