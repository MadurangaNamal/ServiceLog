using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Models;

public class Vehicle
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Model { get; set; } = default!;

    [MaxLength(100)]
    public string Brand { get; set; } = default!;

    [Required]
    public string RegistrationNumber { get; set; } = default!;
    public Category Category { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string? EngineNumber { get; set; }
    public string? ChasisNumber { get; set; }
    public ICollection<ServiceRecord> ServiceRecords { get; set; } = [];
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
}
