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
    }
}