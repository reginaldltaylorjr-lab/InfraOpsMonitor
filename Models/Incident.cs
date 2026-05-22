namespace InfraOpsMonitor.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string IncidentTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AssignedEngineer { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}