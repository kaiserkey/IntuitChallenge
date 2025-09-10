using Serilog;

namespace Intuit.Api.Logging
{
    public class LoggerConfig
    {
        public static void ConfigureLogging() // Configuración de Serilog
        {
            Log.Logger = new LoggerConfiguration() // Configuración del logger
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Ignora logs de Microsoft por debajo de Warning
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) // Ignora logs de System por debajo de Warning
                .MinimumLevel.Debug() // Nivel mínimo de logs a capturar
                .Enrich.FromLogContext() // añade información adicional a los logs
                .WriteTo.Console() // Escribe los logs en la consola
                .WriteTo.File("./LogFile/Log-.txt", rollingInterval: RollingInterval.Day) // Escribe los logs en un archivo de texto con el formato Log-yyyyMMdd.txt
                .CreateLogger(); // Crea el logger
        }
    }
}
