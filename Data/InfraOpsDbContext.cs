using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InfraOpsMonitor.Models;

namespace InfraOpsMonitor.Data
{
    public class InfraOpsDbContext : IdentityDbContext
    {
        public InfraOpsDbContext(DbContextOptions<InfraOpsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Models.Environment> Environments { get; set; }
    }
}