using Microsoft.Extensions.DependencyInjection;
using TeamViewerLogReader.Service.Interfaces;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Business;
using TeamViewerLogReader.Service;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Data.Repositories;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.SqlClient;
using TeamViewerLogReader.Domain.Repositories;
using Microsoft.Win32;

namespace TeamViewerLogReader.WinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            AddApplicationToStartup();

            ApplicationConfiguration.Initialize();
            using (var scope = serviceProvider.CreateScope())
            {
                var form = scope.ServiceProvider.GetRequiredService<Form1>();
                Application.Run(form);
            }
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
            services.AddTransient<Form1>();
            services.AddTransient<AdminUser>();
        }

        private static void AddApplicationToStartup()
        {
            string appName = "TeamViewerLogReader.WinFormsApp";
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(appName, $"\"{appPath}\"");
            }
        }
    }
}