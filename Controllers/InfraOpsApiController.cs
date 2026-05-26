using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfraOpsMonitor.Data;

namespace InfraOpsMonitor.Controllers
{
    [Route("api")]
    [ApiController]
    public class InfraOpsApiController : ControllerBase
    {
        private readonly InfraOpsDbContext _context;

        public InfraOpsApiController(InfraOpsDbContext context)
        {
            _context = context;
        }

        [HttpGet("servers")]
        public IActionResult GetServers()
        {
            var servers = _context.Servers.ToList();
            return Ok(servers);
        }

        [HttpGet("services")]
        public IActionResult GetServices()
        {
            var services = _context.Services
                .Include(s => s.Server)
                .Select(s => new
                {
                    s.Id,
                    s.ServiceName,
                    s.ServiceType,
                    s.Status,
                    s.UptimePercentage,
                    s.LastChecked,
                    ServerName = s.Server != null ? s.Server.ServerName : null,
                    EnvironmentName = s.Server != null ? s.Server.EnvironmentName : null
                })
                .ToList();

            return Ok(services);
        }

        [HttpGet("incidents")]
        public IActionResult GetIncidents()
        {
            var incidents = _context.Incidents.ToList();
            return Ok(incidents);
        }

        [HttpGet("dashboard/summary")]
        public IActionResult GetSummary()
        {
            var services = _context.Services.ToList();
            var incidents = _context.Incidents.ToList();

            var summary = new
            {
                TotalServices = services.Count,
                HealthyServices = services.Count(s => s.Status == "Healthy"),
                WarningServices = services.Count(s => s.Status == "Warning"),
                CriticalServices = services.Count(s => s.Status == "Critical"),
                OpenIncidents = incidents.Count(i => i.Status != "Resolved")
            };

            return Ok(summary);
        }
    }
}