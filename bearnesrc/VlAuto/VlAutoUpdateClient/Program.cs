using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace VlAutoUpdateClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var services = new ServiceCollection();
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.File("VlAutoApp.txt")
                .CreateLogger();
            services.AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Information);
                x.AddSerilog(logger: serilogLogger, dispose: true);
            });
            services.AddTransient<VlHttpClientHandler>();
            services.AddTransient<HttpClientResolver>(serviceProvider => key =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(key);
                return new HttpClient(httpClient);
            });
            services.AddHttpClient(HttpClientNameEnum.Default.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(30);
            }).ConfigurePrimaryHttpMessageHandler<VlHttpClientHandler>();
            services.AddHttpClient(HttpClientNameEnum.Download.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(300);
            }).ConfigurePrimaryHttpMessageHandler<VlHttpClientHandler>();

            services.AddTransient<FormMain>();


            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var form1 = serviceProvider.GetRequiredService<FormMain>();
            Application.Run(form1);
        }
    }
}
