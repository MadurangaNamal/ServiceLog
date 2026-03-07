using Microsoft.EntityFrameworkCore;
using ServiceLog.Data;
using ServiceLog.Models;

namespace ServiceLog.Repositories;

public class ServiceLogRepository(ApplicationDbContext dbContext, ILogger<ServiceLogRepository> logger)
    : IServiceLogRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<ServiceLogRepository> _logger = logger;


    public async Task<Vehicle> GetVehicleDetialsAsync(int vehicleId)
    {
        _logger.LogDebug("Getting vehicle details for id {VehicleId}", vehicleId);

        try
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with id {VehicleId} not found.", vehicleId);
                throw new KeyNotFoundException($"Vehicle with id:{vehicleId} is not found.");
            }

            _logger.LogDebug("Found vehicle with id {VehicleId}", vehicleId);

            return vehicle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle details for id {VehicleId}", vehicleId);
            throw;
        }
    }

    public async Task<IEnumerable<Vehicle>> GetVehiclesForUserAsync(string userId)
    {
        _logger.LogDebug("Getting vehicles for user id {UserId}", userId);

        ArgumentNullException.ThrowIfNullOrEmpty(userId);

        try
        {
            var vehicles = await _dbContext.Vehicles
                .AsNoTracking()
                .OrderBy(v => v.Category)
                .Where(v => v.UserId == userId)
                .ToListAsync();

            _logger.LogDebug("Found {Count} vehicles for user id {UserId}", vehicles.Count, userId);

            return vehicles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicles for user id {UserId}", userId);
            throw;
        }
    }

    public async Task AddNewVehicleAsync(Vehicle vehicle)
    {
        _logger.LogDebug("Adding new vehicle for user id {UserId}", vehicle?.UserId);

        ArgumentNullException.ThrowIfNull(vehicle);

        try
        {
            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Created vehicle with id {VehicleId} for user id {UserId}", vehicle.Id, vehicle.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new vehicle for user id {UserId}", vehicle.UserId);
            throw;
        }
    }

    public async Task UpdateVehicleDetailsAsync(int vehicleId, Vehicle vehicle)
    {
        _logger.LogDebug("Updating vehicle id {VehicleId}", vehicleId);

        ArgumentNullException.ThrowIfNull(vehicle);

        try
        {
            var vehicleDetails = await _dbContext.Vehicles.FindAsync(vehicleId);

            if (vehicleDetails == null)
            {
                _logger.LogWarning("Vehicle with id {VehicleId} not found.", vehicleId);
                throw new KeyNotFoundException($"Vehicle with id:{vehicleId} is not found.");
            }

            vehicleDetails.EngineNumber = vehicle.EngineNumber;
            vehicleDetails.ChasisNumber = vehicle.ChasisNumber;
            vehicleDetails.RegistrationNumber = vehicle.RegistrationNumber;
            vehicleDetails.Category = vehicle.Category;
            vehicleDetails.Brand = vehicle.Brand;
            vehicleDetails.Model = vehicle.Model;
            vehicleDetails.Mileage = vehicle.Mileage;
            vehicleDetails.Year = vehicle.Year;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated vehicle id {VehicleId}", vehicleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle id {VehicleId}", vehicleId);
            throw;
        }
    }

    public async Task DeleteVehicleDetailsAsync(int vehicleId)
    {
        _logger.LogDebug("Deleting vehicle id {VehicleId}", vehicleId);

        try
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with id {VehicleId} not found.", vehicleId);
                throw new KeyNotFoundException($"Vehicle with id:{vehicleId} is not found.");
            }

            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted vehicle id {VehicleId}", vehicleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle id {VehicleId}", vehicleId);
            throw;
        }
    }

    ////

    public async Task<ServiceRecord> GetServiceRecordAsync(int recordId)
    {
        _logger.LogDebug("Getting service record with id {RecordId}", recordId);

        try
        {
            var serviceRecord = await _dbContext.ServiceRecords.FindAsync(recordId);

            if (serviceRecord == null)
            {
                _logger.LogWarning("Service record with id {RecordId} not found.", recordId);
                throw new KeyNotFoundException($"Service record with id: {recordId} not found.");
            }

            _logger.LogDebug("Found service record with id {RecordId}", recordId);

            return serviceRecord;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service record with id {RecordId}", recordId);
            throw;
        }
    }

    public async Task<IEnumerable<ServiceRecord>> GetServiceRecordsForVehicleAsync(int vehicleId)
    {
        _logger.LogDebug("Getting service records for vehicle id {VehicleId}", vehicleId);

        try
        {
            var records = await _dbContext.ServiceRecords
                .AsNoTracking()
                .OrderBy(sr => sr.ServiceDate)
                .Where(sr => sr.VehicleId == vehicleId)
                .ToListAsync();

            _logger.LogDebug("Found {Count} service records for vehicle id {VehicleId}", records.Count, vehicleId);

            return records;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service records for vehicle id {VehicleId}", vehicleId);
            throw;
        }
    }

    public async Task CreateServiceRecord(ServiceRecord serviceRecord)
    {
        _logger.LogDebug("Creating new service record for vehicle id {VehicleId}", serviceRecord?.VehicleId);

        if (serviceRecord == null)
        {
            _logger.LogWarning("CreateServiceRecord called with null serviceRecord");
            ArgumentNullException.ThrowIfNull(serviceRecord);
        }

        try
        {
            _dbContext.ServiceRecords.Add(serviceRecord);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Created service record with id {RecordId} for vehicle id {VehicleId}", serviceRecord.Id, serviceRecord.VehicleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service record for vehicle id {VehicleId}", serviceRecord.VehicleId);
            throw;
        }
    }

    public async Task UpdateServiceRecord(int recordId, ServiceRecord serviceRecord)
    {
        _logger.LogDebug("Updating service record id {RecordId}", recordId);

        if (serviceRecord == null)
        {
            _logger.LogWarning("UpdateServiceRecord called with null serviceRecord for id {RecordId}", recordId);
            ArgumentNullException.ThrowIfNull(serviceRecord);
        }

        try
        {
            var record = await _dbContext.ServiceRecords.FindAsync(recordId);

            if (record == null)
            {
                _logger.LogWarning("Service record with id {RecordId} not found.", recordId);

                throw new KeyNotFoundException($"Service record with id: {recordId} not found.");
            }

            record.ServiceDate = serviceRecord.ServiceDate;
            record.VehicleId = serviceRecord.VehicleId;
            record.ServiceType = serviceRecord.ServiceType;
            record.CurrentMileage = serviceRecord.CurrentMileage;
            record.Cost = serviceRecord.Cost;
            record.Notes = serviceRecord.Notes;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated service record id {RecordId}", recordId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service record id {RecordId}", recordId);
            throw;
        }
    }

    public async Task DeleteServiceRecordAsync(int recordId)
    {
        _logger.LogDebug("Deleting service record id {RecordId}", recordId);

        try
        {
            var record = await _dbContext.ServiceRecords.FindAsync(recordId);

            if (record == null)
            {
                _logger.LogWarning("Service record with id {RecordId} not found.", recordId);
                throw new KeyNotFoundException($"Service record with id: {recordId} not found.");
            }

            _dbContext.ServiceRecords.Remove(record);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Deleted service record id {RecordId}", recordId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service record id {RecordId}", recordId);
            throw;
        }
    }
}
