using ServiceLog.Models;

namespace ServiceLog.Repositories;

public interface IServiceLogRepository
{
    Task<ServiceRecord> GetServiceRecordAsync(int recordId);
    Task<IEnumerable<ServiceRecord>> GetServiceRecordsForVehicleAsync(int vehicleId);
    Task CreateServiceRecord(ServiceRecord serviceRecord);
    Task UpdateServiceRecord(int recordId, ServiceRecord serviceRecord);
    Task DeleteServiceRecordAsync(int recordId);
}
