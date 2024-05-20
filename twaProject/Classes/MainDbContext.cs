namespace twaProject.Classes;
using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public DbSet<WebUser> WebUser { get; set; }
    public DbSet<Projekt> Projekt { get; set; }
    public DbSet<Task> Task { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
}