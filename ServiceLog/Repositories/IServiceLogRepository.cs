using ServiceLog.Models;

namespace ServiceLog.Repositories;

public interface IServiceLogRepository
{
    #region Vehicles
    Task<Vehicle> GetVehicleDetialsAsync(int vehicleId);
    Task<IEnumerable<Vehicle>> GetVehiclesForUserAsync(string userId);
    Task AddNewVehicleAsync(Vehicle vehicle);
    Task UpdateVehicleDetailsAsync(int vehicleId, Vehicle vehicle);
    Task DeleteVehicleDetailsAsync(int vehicleId);
    #endregion

    #region Service Records
    Task<ServiceRecord> GetServiceRecordAsync(int recordId);
    Task<IEnumerable<ServiceRecord>> GetServiceRecordsForVehicleAsync(int vehicleId);
    Task CreateServiceRecord(ServiceRecord serviceRecord);
    Task UpdateServiceRecord(int recordId, ServiceRecord serviceRecord);
    Task DeleteServiceRecordAsync(int recordId);
    #endregion
}
