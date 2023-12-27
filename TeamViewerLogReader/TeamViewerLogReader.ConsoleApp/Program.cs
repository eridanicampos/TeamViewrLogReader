using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamViewerLogReader.Business;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.ConsoleApp;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Data.Repositories;
using TeamViewerLogReader.Domain.Repositories;
using TeamViewerLogReader.Service;
using TeamViewerLogReader.Service.Interfaces;
public class Program
{
    static void Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();

        var services = new ServiceCollection();
        ConfigureServices(services, configuration);
        var serviceProvider = services.BuildServiceProvider();
        var logMonitor = serviceProvider.GetService<LogMonitor>();
        logMonitor.MonitorLogFile();

    }
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ILogRederService, LogRederService>();
        services.AddTransient<ILogRederBusiness, LogRederBusiness>();
        services.AddTransient<ILogEntryRepository, LogEntryRepository>();
        services.AddTransient<IUserTvLogService, UserTvLogService>();
        services.AddTransient<IUserTvLogBusiness, UserTvLogBusiness>();
        services.AddTransient<IUserTvLogRepository, UserTvLogRepository>();
        services.AddSingleton(new DataContext(configuration.GetConnectionString("DefaultConnection")));
        services.AddTransient<LogMonitor>();
    }
}