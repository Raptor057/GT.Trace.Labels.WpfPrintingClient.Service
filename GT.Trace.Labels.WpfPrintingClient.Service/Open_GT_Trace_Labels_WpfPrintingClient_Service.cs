
using System.Diagnostics;

namespace GT.Trace.Labels.WpfPrintingClient.Service
{
    internal class Open_GT_Trace_Labels_WpfPrintingClient_Service : BackgroundService
    {
        private readonly ILogger<Open_GT_Trace_Labels_WpfPrintingClient_Service> _logger;
        private Process? _runningProcess; // Almacena la instancia del proceso en ejecución


        public Open_GT_Trace_Labels_WpfPrintingClient_Service(ILogger<Open_GT_Trace_Labels_WpfPrintingClient_Service> logger)
        {
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Ruta donde buscar el archivo exe
                string appFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Apps");

                // Buscar el archivo exe
                string[] exeFiles = Directory.GetFiles(appFolderPath, "GT.Trace.Labels.WpfPrintingClient.exe", SearchOption.AllDirectories);

                if (exeFiles.Any())
                {
                    string exeFilePath = exeFiles.First();

                    if (_runningProcess == null || _runningProcess.HasExited)
                    {
                        // Si no hay una instancia en ejecución o está cerrada, abrir una nueva
                        //_runningProcess = Process.Start(exeFilePath);

                        // Si no hay una instancia en ejecución o está cerrada, abrir una nueva minimizada
                        ProcessStartInfo startInfo = new ProcessStartInfo(exeFilePath);
                        startInfo.WindowStyle = ProcessWindowStyle.Minimized;

                        _runningProcess = Process.Start(startInfo);
                        _logger.LogInformation("Ejecutando {0}", exeFilePath);
                    }
                    else if (Process.GetProcessesByName("GT.Trace.Labels.WpfPrintingClient").Length > 1)
                    {
                        // Obtener todas las instancias en ejecución del proceso
                        var processes = Process.GetProcessesByName("GT.Trace.Labels.WpfPrintingClient");

                        // Cerrar todas las instancias excepto una
                        for (int i = 1; i < processes.Length; i++)
                        {
                            processes[i].Kill();
                            _logger.LogWarning("Cerrando instancia adicional de GT.Trace.Labels.WpfPrintingClient.exe");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("El archivo GT.Trace.Labels.WpfPrintingClient.exe no se encontró en {0}", appFolderPath);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
