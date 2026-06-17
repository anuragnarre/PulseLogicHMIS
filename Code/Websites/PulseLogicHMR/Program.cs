using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace PulseLogicHMR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ── Task 5: Bootstrap Serilog BEFORE host creation ────────────────────
            // Initialised early so that startup exceptions are also captured.
            // Rolling file: one log per day, retained 30 days.
            // Namespace-level overrides: suppress verbose Microsoft/EF logs in production
            // while keeping full Debug-level logging for PulseLogicHMR own code.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft",         LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
                .MinimumLevel.Override("PulseLogicHMR",     LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path:                   "logs/pulselogic-.log",
                    rollingInterval:        RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} " +
                        "Thread={ThreadId} Machine={MachineName} | {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("PulseLogicHMR starting up (v{Version})",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

                // This project uses the classic IWebHostBuilder pattern.
                // UseSerilog() is available on IHostBuilder (Generic Host), not IWebHostBuilder.
                // For IWebHostBuilder, Serilog is wired in via ConfigureLogging — which replaces
                // all default Microsoft logging providers with Serilog's ILogger adapter.
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .ConfigureLogging(logging =>
                    {
                        // Remove all built-in providers (Console, Debug, EventSource …)
                        // so that Serilog is the single logging backend.
                        logging.AddSerilog(dispose: true);
                    })
                    .UseStartup<Startup>()
                    .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly during startup.");
            }
            finally
            {
                // Flush all buffered log events before the process exits.
                Log.CloseAndFlush();
            }
        }
    }
}
