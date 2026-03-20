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
        var selectedVehicle = vehicleId == null ? userVehicles.FirstOrDefault()
            : userVehicles.FirstOrDefault(v => v.Id == vehicleId);

        var model = new ServiceDashboardViewModel
        {
            Vehicles = userVehicles,
            Vehicle = selectedVehicle,
            ServiceRecords = selectedVehicle?.ServiceRecords ?? []
        };

        return View(model);
    }

    public async Task<IActionResult> CreateServiceRecord(int vehicleId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found. Please log in");
            return View();
        }

        var vehicles = await _serviceLogRepository.GetVehiclesForUserAsync(user.Id);

        var vehicle = vehicles.FirstOrDefault(v => v.Id == vehicleId);

        if (vehicle == null)
            return NotFound();

        var model = new ServiceRecord
        {
            VehicleId = vehicleId,
            ServiceDate = DateTime.UtcNow,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceRecord serviceRecord)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                await _serviceLogRepository.CreateServiceRecordAsync(serviceRecord);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in.");
                return View(serviceRecord);
            }

            return RedirectToAction("Index", new { vehicleId = serviceRecord.VehicleId });
        }

        return View("CreateServiceRecord", serviceRecord);
    }

    public async Task<IActionResult> UpdateServiceRecord(int id)
    {
        var serviceRecord = await _serviceLogRepository.GetServiceRecordAsync(id);
        return View(serviceRecord);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, ServiceRecord serviceRecord)
    {
        if (ModelState.IsValid)
        {
            await _serviceLogRepository.UpdateServiceRecordAsync(id, serviceRecord);
            return RedirectToAction("Index", new { vehicleId = serviceRecord.VehicleId });
        }

        return View("UpdateServiceRecord", serviceRecord);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var serviceRecord = await _serviceLogRepository.GetServiceRecordAsync(id);

        if (serviceRecord == null)
            return NotFound($"Service record with ID:{id} not found for the user");

        var userVehicles = await _serviceLogRepository.GetVehiclesForUserAsync(user!.Id.ToString());

        if (userVehicles.All(v => v.Id != serviceRecord.VehicleId))
            return Forbid();

        await _serviceLogRepository.DeleteServiceRecordAsync(id);

        return RedirectToAction("Index");
    }
}
