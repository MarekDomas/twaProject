namespace twaProject.Classes;
using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public DbSet<WebUser> WebUsers { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebDb;Trusted_Connection=True;");
    }
}