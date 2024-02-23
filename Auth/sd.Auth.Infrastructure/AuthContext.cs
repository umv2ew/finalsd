using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sd.Auth.Domain;

namespace sd.Auth.Infrastructure;

public class AuthContext : IdentityDbContext<AppUser>
{
    public AuthContext() { }
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}