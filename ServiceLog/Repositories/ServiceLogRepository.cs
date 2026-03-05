using Microsoft.EntityFrameworkCore;
using ServiceLog.Data;
using ServiceLog.Models;

namespace ServiceLog.Repositories;

public class ServiceLogRepository(ApplicationDbContext dbContext, ILogger<ServiceLogRepository> logger)
    : IServiceLogRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ILogger<ServiceLogRepository> _logger = logger;

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
