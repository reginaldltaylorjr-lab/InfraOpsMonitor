using Microsoft.EntityFrameworkCore;
using InfraOpsMonitor.Data;
using InfraOpsMonitor.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<InfraOpsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InfraOpsDbContext>();

    if (!context.Servers.Any())
    {
        var servers = new List<Server>
        {
            new Server
            {
                ServerName = "APP-SRV-01",
                IpAddress = "10.10.1.15",
                EnvironmentName = "Production",
                HealthStatus = "Healthy",
                CpuUsage = 42,
                MemoryUsage = 58
            },
            new Server
            {
                ServerName = "DB-SRV-01",
                IpAddress = "10.10.1.25",
                EnvironmentName = "Production",
                HealthStatus = "Warning",
                CpuUsage = 71,
                MemoryUsage = 82
            },
            new Server
            {
                ServerName = "API-SRV-02",
                IpAddress = "10.10.2.10",
                EnvironmentName = "QA",
                HealthStatus = "Critical",
                CpuUsage = 92,
                MemoryUsage = 88
            }
        };

        context.Servers.AddRange(servers);
        context.SaveChanges();

        context.Services.AddRange(
            new Service
            {
                ServiceName = "Authentication API",
                ServiceType = "API",
                Status = "Healthy",
                UptimePercentage = 99,
                LastChecked = DateTime.Now.AddMinutes(-2),
                ServerId = servers[0].Id
            },
            new Service
            {
                ServiceName = "Inventory Sync Service",
                ServiceType = "Background Worker",
                Status = "Warning",
                UptimePercentage = 96,
                LastChecked = DateTime.Now.AddMinutes(-7),
                ServerId = servers[1].Id
            },
            new Service
            {
                ServiceName = "Order Processing API",
                ServiceType = "API",
                Status = "Critical",
                UptimePercentage = 87,
                LastChecked = DateTime.Now.AddMinutes(-15),
                ServerId = servers[2].Id
            },
            new Service
            {
                ServiceName = "Reporting Dashboard",
                ServiceType = "Web App",
                Status = "Healthy",
                UptimePercentage = 98,
                LastChecked = DateTime.Now.AddMinutes(-4),
                ServerId = servers[0].Id
            }
        );

        context.Incidents.AddRange(
            new Incident
            {
                IncidentTitle = "Order Processing API timeout",
                Description = "API response times exceeded acceptable threshold in QA environment.",
                Severity = "Critical",
                Status = "Open",
                AssignedEngineer = "Reginald Taylor",
                CreatedAt = DateTime.Now.AddMinutes(-35)
            },
            new Incident
            {
                IncidentTitle = "Database memory utilization high",
                Description = "Production database server memory usage exceeded 80%.",
                Severity = "High",
                Status = "Acknowledged",
                AssignedEngineer = "Systems Team",
                CreatedAt = DateTime.Now.AddHours(-2)
            },
            new Incident
            {
                IncidentTitle = "Inventory sync delayed",
                Description = "Background sync service processing slower than expected.",
                Severity = "Medium",
                Status = "In Progress",
                AssignedEngineer = "Cloud Ops",
                CreatedAt = DateTime.Now.AddHours(-5)
            }
        );

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
