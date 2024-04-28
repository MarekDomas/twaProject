namespace twaProject.Classes;
using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public DbSet<WebUser> WebUser { get; set; }
    public DbSet<Projekt> Projekt { get; set; }
    public DbSet<Task> Task { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebDb;Trusted_Connection=True;");
    }
}