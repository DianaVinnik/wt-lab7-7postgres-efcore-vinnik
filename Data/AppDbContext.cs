using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_vinnik.Models;

namespace wt_lab7_7postgres_efcore_vinnik.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account 
            { 
                Id = 1, 
                Owner = "Иван Иванов", 
                AccountNumber = "BY001", 
                Balance = 2500, 
                AccountType = "Сберегательный", 
                IsActive = true, 
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) 
            },
            new Account 
            { 
                Id = 2, 
                Owner = "Анна Петрова", 
                AccountNumber = "BY002", 
                Balance = 5000, 
                AccountType = "Кредитный", 
                IsActive = true, 
                CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc) 
            }
        );
    }
}