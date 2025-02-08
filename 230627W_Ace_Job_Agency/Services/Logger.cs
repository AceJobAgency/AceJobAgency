using _230627W_Ace_Job_Agency.Model;
using _230627W_Ace_Job_Agency.ViewModels;

public static class Logger {
    public static async Task LogActivity(string email, string action, AuthDbContext _context) {
        var auditLog = new AuditLog {
            Email = email,
            Action = action,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
}