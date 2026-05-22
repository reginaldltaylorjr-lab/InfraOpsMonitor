namespace InfraOpsMonitor.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string ServerName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public string HealthStatus { get; set; } = string.Empty;
        public int CpuUsage { get; set; }
        public int MemoryUsage { get; set; }

        public List<Service> Services { get; set; } = new();
    }
}