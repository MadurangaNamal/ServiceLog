using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Models;
using ServiceLog.Repositories;

namespace ServiceLog.Controllers;

[Authorize]
public class VehicleServiceController : Controller
{
    private readonly IServiceLogRepository _serviceLogRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public VehicleServiceController(IServiceLogRepository serviceLogRepository, UserManager<ApplicationUser> userManager)
    {
        _serviceLogRepository = serviceLogRepository ?? throw new ArgumentNullException(nameof(serviceLogRepository));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IActionResult> Index(int? vehicleId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found. Please log in");
            return View();
        }

        var userVehicles = await _serviceLogRepository.GetVehiclesForUserAsync(user.Id.ToString());
        var selectedVehicle = vehicleId == null ? userVehicles.FirstOrDefault() : userVehicles.FirstOrDefault(v => v.Id == vehicleId);

        var model = new ServiceDashboardViewModel
        {
            Vehicles = userVehicles,
            Vehicle = selectedVehicle,
            ServiceRecords = selectedVehicle?.ServiceRecords ?? []
        };

        return View(model);
    }
}
