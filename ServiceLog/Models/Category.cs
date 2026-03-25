using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Models;

public enum Category
{
    [Display(Name = "Passenger Car")]
    PassengerCar,

    [Display(Name = "Motorcycle")]
    Motorcycle,

    [Display(Name = "Light Truck")]
    LightTruck,

    [Display(Name = "Heavy Truck")]
    HeavyTruck,

    [Display(Name = "Bus")]
    Bus,

    [Display(Name = "Recreational Vehicle")]
    RecreationalVehicle,

    [Display(Name = "Off Road Vehicle")]
    OffRoadVehicle,

    [Display(Name = "Specialty Vehicle")]
    SpecialtyVehicle,
}
