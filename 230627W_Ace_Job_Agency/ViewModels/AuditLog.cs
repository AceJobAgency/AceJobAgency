namespace _230627W_Ace_Job_Agency.ViewModels {
    public class AuditLog {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}