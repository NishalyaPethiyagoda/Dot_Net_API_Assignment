using backendAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace backendAPI.Data;

public class AppDbContext: DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext>options): base(options)
	{
			
	}

	public DbSet<Farm> Farms { get; set; }

    
    public DbSet<Worker> Workers { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{

    //    modelBuilder.Entity<Worker>()
    //        .Property(u => u.CertifiedDate)
    //        .HasConversion(
    //            v => v.ToString("yyyy-MM-dd"),
    //            v => DateOnly.Parse(v)
    //        );
    //}
    public DbSet<FarmWorkers> FarmWorkers { get; set; }
	public DbSet<WorkerDesignations> WorkerDesignations { get; set; }
}
