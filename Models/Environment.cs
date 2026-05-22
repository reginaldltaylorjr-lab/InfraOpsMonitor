namespace InfraOpsMonitor.Models
{
    public class Environment
    {
        public int Id { get; set; }
        public string EnvironmentName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string OwnerTeam { get; set; } = string.Empty;
    }
}