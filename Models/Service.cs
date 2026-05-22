using Microsoft.AspNetCore.Hosting.Server;

namespace InfraOpsMonitor.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int UptimePercentage { get; set; }
        public DateTime LastChecked { get; set; }

        public int ServerId { get; set; }
        public Server? Server { get; set; }
    }
}