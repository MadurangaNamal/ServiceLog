using System.ComponentModel.DataAnnotations;

namespace ServiceLog.Models;

public enum ServiceType
{
    [Display(Name = "Oil Change")]
    OilChange,

    [Display(Name = "Brakes")]
    Brakes,

    [Display(Name = "Tires/Wheels")]
    TiresWheels,

    [Display(Name = "Fluids")]
    Fluids,

    [Display(Name = "Filters")]
    Filters,

    [Display(Name = "Battery")]
    BatteryElectrical,

    [Display(Name = "Engine TuneUp")]
    EngineTuneUp,

    [Display(Name = "Transmission")]
    Transmission,

    [Display(Name = "Suspension/Steering")]
    SuspensionSteering,

    [Display(Name = "Exhaust")]
    ExhaustEmissions,

    [Display(Name = "AC Repair")]
    ACHeating,

    [Display(Name = "Diagnostic/Inspection")]
    DiagnosticInspection,

    [Display(Name = "Other")]
    Other
}
