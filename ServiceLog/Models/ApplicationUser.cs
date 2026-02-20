using Microsoft.AspNetCore.Identity;

namespace ServiceLog.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
