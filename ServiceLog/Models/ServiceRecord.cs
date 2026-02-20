using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Models;

public class ServiceRecord
{
    [Key]
    public int Id { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime ServiceDate { get; set; } = DateTime.UtcNow;
    public int CurrentMileage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; } = 0.00m;
    public string? Notes { get; set; }

    public int VehicleId { get; set; }

    [ForeignKey("VehicleId")]
    public Vehicle? Vehicle { get; set; }
}
