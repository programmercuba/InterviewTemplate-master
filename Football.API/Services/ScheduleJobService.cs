using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Football.API.Services
{
    public class ScheduleJobService : BackgroundService, IHostedService
    {
        private readonly ILogger<ScheduleJobService> _logger;
        private Timer? _timer = null;
        private IServiceProvider _serviceProvider;
        


        public ScheduleJobService(IServiceProvider services, ILogger<ScheduleJobService> logger)
        {
            _logger = logger;
            _serviceProvider = services;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(IncorrectAlignmentService, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));           
        }

        //Método que comprueba 5 minutos antes del inicio de cada match si hay alguna alineación indebida 
        private void IncorrectAlignmentService(object state) 
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<FootballContext>();
                    var scopedProcessingService =
                        scope.ServiceProvider.GetRequiredService<IIncorrectAlignmentService>();
                    if (context != null)
                    {
                        foreach (var ctx in context.Matches)
                        {                            
                            if (DateTime.Now.Minute - ctx?.Date.Minute == 5)
                            {
                                if (scopedProcessingService.IsIncorrectAlignment()) Console.WriteLine("IncorrectAlignment");
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<ScheduleJobService>>();
                    logger.LogError(ex, "An error occurred.");
                }
            }
        }
    }
}
