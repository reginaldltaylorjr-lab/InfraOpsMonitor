using InfraOpsMonitor.Data;
using InfraOpsMonitor.Models;
using InfraOpsMonitor.Services;
using InfraOpsMonitor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace InfraOpsMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly InfraOpsDbContext _context;
        private readonly InfrastructureSimulationService _simulationService;

        public HomeController(InfraOpsDbContext context, InfrastructureSimulationService simulationService)
        {
            _context = context;
            _simulationService = simulationService;
        }

        public IActionResult Index(
            string searchTerm,
            string serviceStatusFilter,
            string environmentFilter,
            string incidentSeverityFilter,
            string incidentStatusFilter
            )
        {
            _simulationService.UpdateSystemMetrics();
            
            var servers =  _context.Servers.ToList();

            var services = _context.Services
                .Include(s => s.Server)
                .ToList();

            var incidents = _context.Incidents.ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                services = services
                    .Where(s =>
                        (!string.IsNullOrEmpty(s.ServiceName) &&
                         s.ServiceName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ||
                        (!string.IsNullOrEmpty(s.ServiceType) &&
                         s.ServiceType.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ||
                        (s.Server != null &&
                         !string.IsNullOrEmpty(s.Server.ServerName) &&
                         s.Server.ServerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();

                servers = servers
                    .Where(s =>
                        (!string.IsNullOrEmpty(s.ServerName) &&
                         s.ServerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ||
                        (!string.IsNullOrEmpty(s.IpAddress) &&
                         s.IpAddress.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();

                incidents = incidents
                    .Where(i =>
                        (!string.IsNullOrEmpty(i.IncidentTitle) &&
                         i.IncidentTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ||
                        (!string.IsNullOrEmpty(i.Description) &&
                         i.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ||
                        (!string.IsNullOrEmpty(i.AssignedEngineer) &&
                         i.AssignedEngineer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(serviceStatusFilter))
            {
                services = services
                    .Where(s => !string.IsNullOrEmpty(s.Status) && s.Status == serviceStatusFilter)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(environmentFilter))
            {
                servers = servers
                    .Where(s => !string.IsNullOrEmpty(s.EnvironmentName) && s.EnvironmentName == environmentFilter)
                    .ToList();

                services = services
                    .Where(s => s.Server != null &&
                                !string.IsNullOrEmpty(s.Server.EnvironmentName) &&
                                s.Server.EnvironmentName == environmentFilter)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(incidentSeverityFilter))
            {
                incidents = incidents
                    .Where(i => !string.IsNullOrEmpty(i.Severity) && i.Severity == incidentSeverityFilter)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(incidentStatusFilter))
            {
                incidents = incidents
                    .Where(i => !string.IsNullOrEmpty(i.Status) && i.Status == incidentStatusFilter)
                    .ToList();
            }

            var viewModel = new DashboardViewModel
            {
                Servers = servers,
                Services = services,
                Incidents = incidents
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
