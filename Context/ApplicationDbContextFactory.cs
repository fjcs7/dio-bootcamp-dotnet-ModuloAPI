using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ModuloAPI.Context;

namespace ModuloAPI;

public class ApplicationDbContextFactory //: IDesignTimeDbContextFactory<AgendaContext>
{
    public AgendaContext CreateDbContext(string[] args)
    {
        // 1. Build Configuration manually
        // This ensures the EF tools can reliably find the appsettings.json files
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string environmentString = string.Empty;
        Console.WriteLine($"=====================================>>>>>{environment}");
        if(String.IsNullOrEmpty(environment) || environment != "Production")
        {
            environmentString = ".Development";
        }
        
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings{environmentString}.json", optional: false, reloadOnChange: true)
            .Build();

        // 2. Get the connection string using the key name
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The 'DefaultConnection' connection string was not found in configuration.");
        }

        // 3. Configure the DbContext options
        var builder = new DbContextOptionsBuilder<AgendaContext>();
        builder.UseSqlServer(connectionString);

        // 4. Return a new instance of your DbContext initialized with the options
        return new AgendaContext(builder.Options);
    }
}