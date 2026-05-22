using InfraOpsMonitor.Models;

namespace InfraOpsMonitor.ViewModels
{
    public class DashboardViewModel
    {
        public List<Service> Services { get; set; } = new();
        public List<Server> Servers { get; set; } = new();
        public List<Incident> Incidents { get; set; } = new();

        public int TotalServices => Services.Count;
        public int HealthyServices => Services.Count(s => s.Status == "Healthy");
        public int WarningServices => Services.Count(s => s.Status == "Warning");
        public int CriticalServices => Services.Count(s => s.Status == "Critical");
        public int OpenIncidents => Incidents.Count(i => i.Status != "Resolved");
    }
}