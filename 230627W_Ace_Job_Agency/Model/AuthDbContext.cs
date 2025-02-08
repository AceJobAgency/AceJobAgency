using _230627W_Ace_Job_Agency.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _230627W_Ace_Job_Agency.Model {
    public class AuthDbContext : IdentityDbContext<ApplicationUser> {
        private readonly IConfiguration _configuration;
        public DbSet<AuditLog> AuditLogs { get; set; }
        public AuthDbContext(IConfiguration configuration) {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString") ?? "Data Source=app.db";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}