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

    public DbSet<FarmWorkers> FarmWorkers { get; set; }

	public DbSet<WorkerDesignations> WorkerDesignations { get; set; }
}
