namespace ServiceLog.Models;

public class ServiceDashboardViewModel
{
    public IEnumerable<Vehicle>? Vehicles { get; set; }

    public Vehicle? Vehicle { get; set; }

    public IEnumerable<ServiceRecord>? ServiceRecords { get; set; }
}
