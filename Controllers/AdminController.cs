using InfraOpsMonitor.Data;
using InfraOpsMonitor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraOpsMonitor.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly InfraOpsDbContext _context;

        public AdminController(InfraOpsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalServers = _context.Servers.Count();
            ViewBag.TotalServices = _context.Services.Count();
            ViewBag.TotalIncidents = _context.Incidents.Count();
            ViewBag.CriticalServices = _context.Services.Count(s => s.Status == "Critical");
            ViewBag.OpenIncidents = _context.Incidents.Count(i => i.Status != "Resolved");

            return View();
        }

        public IActionResult Servers()
        {
            var servers = _context.Servers.ToList();
            return View(servers);
        }

        public IActionResult CreateServer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateServer(Server server)
        {
            if (ModelState.IsValid)
            {
                _context.Servers.Add(server);
                _context.SaveChanges();
                return RedirectToAction(nameof(Servers));
            }

            return View(server);
        }

        public IActionResult EditServer(int id)
        {
            var server = _context.Servers.Find(id);

            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditServer(Server server)
        {
            if (ModelState.IsValid)
            {
                _context.Servers.Update(server);
                _context.SaveChanges();
                return RedirectToAction(nameof(Servers));
            }

            return View(server);
        }

        public IActionResult DeleteServer(int id)
        {
            var server = _context.Servers.Find(id);

            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        [HttpPost, ActionName("DeleteServer")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteServerConfirmed(int id)
        {
            var server = _context.Servers.Find(id);

            if (server != null)
            {
                _context.Servers.Remove(server);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Servers));
        }

        public IActionResult Incidents()
        {
            var incidents = _context.Incidents.ToList();
            return View(incidents);
        }

        public IActionResult CreateIncident()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateIncident(Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Incidents.Add(incident);
                _context.SaveChanges();
                return RedirectToAction(nameof(Incidents));
            }

            return View(incident);
        }

        public IActionResult EditIncident(int id)
        {
            var incident = _context.Incidents.Find(id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditIncident(Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Incidents.Update(incident);
                _context.SaveChanges();
                return RedirectToAction(nameof(Incidents));
            }

            return View(incident);
        }

        public IActionResult DeleteIncident(int id)
        {
            var incident = _context.Incidents.Find(id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        [HttpPost, ActionName("DeleteIncident")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteIncidentConfirmed(int id)
        {
            var incident = _context.Incidents.Find(id);

            if (incident != null)
            {
                _context.Incidents.Remove(incident);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Incidents));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResolveIncident(int id)
        {
            var incident = _context.Incidents.Find(id);

            if (incident == null)
            {
                return NotFound();
            }

            incident.Status = "Resolved";

            // Optional additions:
            // incident.ResolvedAt = DateTime.Now;
            // incident.ResolvedBy = User.Identity?.Name;

            _context.SaveChanges();

            return RedirectToAction(nameof(Incidents));
        }
    }
}