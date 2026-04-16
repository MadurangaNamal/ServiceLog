using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLog.Models;
using ServiceLog.Repositories;

namespace ServiceLog.Controllers;

[Authorize]
public class VehiclesController : Controller
{
    private readonly IServiceLogRepository _serviceLogRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public VehiclesController(IServiceLogRepository serviceLogRepository, UserManager<ApplicationUser> userManager)
    {
        _serviceLogRepository = serviceLogRepository ?? throw new ArgumentNullException(nameof(serviceLogRepository));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            var vehicles = await _serviceLogRepository.GetVehiclesForUserAsync(user.Id);
            return View(vehicles);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found. Please log in.");
            return View();
        }
    }

    public async Task<IActionResult> AddNewVehicle()
    {
        return View(new Vehicle());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                vehicle.UserId = user.Id;
                await _serviceLogRepository.AddNewVehicleAsync(vehicle);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in.");
                return View(vehicle);
            }

            return RedirectToAction("Index");
        }

        return View("AddNewVehicle", vehicle);
    }

    public async Task<IActionResult> UpdateVehicle(int Id)
    {
        var vehicle = await _serviceLogRepository.GetVehicleDetialsAsync(Id);
        return View(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int Id, Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            await _serviceLogRepository.UpdateVehicleDetailsAsync(Id, vehicle);
            return RedirectToAction("Index");
        }

        return View("UpdateVehicle", vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        var user = await _userManager.GetUserAsync(User);
        var userVehicles = await _serviceLogRepository.GetVehiclesForUserAsync(user!.Id.ToString());

        if (userVehicles.All(v => v.Id != Id))
            return Forbid();

        await _serviceLogRepository.DeleteVehicleDetailsAsync(Id);

        return RedirectToAction("Index");
    }
}
