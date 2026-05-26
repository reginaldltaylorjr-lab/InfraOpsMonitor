using Microsoft.AspNetCore.SignalR;
using InfraOpsMonitor.Hubs;
using InfraOpsMonitor.Data;

namespace InfraOpsMonitor.Services
{
    public class InfrastructureSimulationService
    {
        private readonly InfraOpsDbContext _context;
        private readonly IHubContext<MonitoringHub> _hubContext;
        private readonly Random _random = new();

        public InfrastructureSimulationService(InfraOpsDbContext context, IHubContext<MonitoringHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task UpdateSystemMetricsAsync()
        {
            var servers = _context.Servers.ToList();

            foreach (var server in servers)
            {
                server.CpuUsage = _random.Next(25, 96);
                server.MemoryUsage = _random.Next(35, 96);

                if (server.CpuUsage >= 85 || server.MemoryUsage >= 85)
                {
                    server.HealthStatus = "Critical";
                }
                else if (server.CpuUsage >= 70 || server.MemoryUsage >= 70)
                {
                    server.HealthStatus = "Warning";
                }
                else
                {
                    server.HealthStatus = "Healthy";
                }
            }

            var services = _context.Services.ToList();

            foreach (var service in services)
            {
                service.UptimePercentage = _random.Next(85, 100);
                service.LastChecked = DateTime.Now;

                if (service.UptimePercentage < 90)
                {
                    service.Status = "Critical";
                }
                else if (service.UptimePercentage < 97)
                {
                    service.Status = "Warning";
                }
                else
                {
                    service.Status = "Healthy";
                }
            }

            _context.SaveChanges();

            await _hubContext.Clients.All.SendAsync(
                "ReceiveMonitoringUpdate",
                $"Infrastructure metrics refreshed at {DateTime.Now:MM/dd/yyy hh:mm:ss tt}");
        }
    }
}