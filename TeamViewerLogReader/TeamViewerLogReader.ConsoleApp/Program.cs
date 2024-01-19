using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using TeamViewerLogReader.Business;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.ConsoleApp;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Data.Repositories;
using TeamViewerLogReader.Domain.Repositories;
using TeamViewerLogReader.Log;
using TeamViewerLogReader.Log.Interfaces;
using TeamViewerLogReader.Service;
using TeamViewerLogReader.Service.Interfaces;
public class Program
{
    static void Main()
    {
        var logger = NLog.LogManager.GetCurrentClassLogger();
        try
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);

            AddApplicationToStartup();

            var serviceProvider = services.BuildServiceProvider();
            var logMonitor = serviceProvider.GetService<LogMonitor>();
            logMonitor.MonitorLogFile();
        }
        catch (Exception ex)
        {
            // Log exceptions
            logger.Error(ex, "Error starting the application");
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            NLog.LogManager.Shutdown();
        }
        

    }
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new DataContext(configuration.GetConnectionString("DefaultConnection")));
        services.AddSingleton(new ClickHouseDataContext(configuration.GetConnectionString("ClickHouseConnection")));
        services.AddTransient<ILogRederService, LogRederService>();
        services.AddTransient<ILogRederBusiness, LogRederBusiness>();
        services.AddTransient<ILogEntryRepository, LogEntryRepository>();
        services.AddTransient<ILogEntryRepositoryClickHouse, LogEntryRepositoryClickHouse>();
        services.AddTransient<IUserTvLogService, UserTvLogService>();
        services.AddTransient<IUserTvLogBusiness, UserTvLogBusiness>();
        services.AddTransient<IUserTvLogRepository, UserTvLogRepository>();
        services.AddTransient<ILoggerService, NLogService>();

        services.AddTransient<LogMonitor>();
    }
        
    private static void AddApplicationToStartup()
    {
        string appName = "TeamViewerLogReader.ConsoleApp";
        string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.SetValue(appName, $"\"{appPath}\"");
        }
    }
}