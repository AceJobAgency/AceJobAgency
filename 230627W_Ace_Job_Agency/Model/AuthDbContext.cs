using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _230627W_Ace_Job_Agency.Model {
    public class AuthDbContext : IdentityDbContext {
        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
        public AuthDbContext(IConfiguration configuration) {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString") ?? "Data Source=app.db";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}