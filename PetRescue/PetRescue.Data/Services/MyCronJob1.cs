using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetRescue.Data.Domains;
using PetRescue.Data.ViewModels;

namespace PetRescue.Data.Services
{
    public class MyCronJob1 : CronJobService
    {
        private readonly ILogger<MyCronJob1> _logger;

        private readonly FinderFormDomain _domain;

        public MyCronJob1(IScheduleConfig<MyCronJob1> config, ILogger<MyCronJob1> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _domain = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FinderFormDomain>();
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");

            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            Recall();
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }

        public void Recall()
        {
            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");
            var objectJson = File.ReadAllText(FILEPATH);
            NotificationToVolunteers json = JsonConvert.DeserializeObject<NotificationToVolunteers>(objectJson);
            if(json != null)
            {
                if (DateTime.UtcNow.Minute == json.CurrentTime.AddMinutes(1).Minute)
                {
                    _domain.ReNotification(json.FinderFormId, json.InsertedBy, json.path);
                    _logger.LogInformation("noti lại nè heeee !!!!");
                }
                if(DateTime.UtcNow.Minute == json.CurrentTime.AddMinutes(3).Minute)
                {
                    File.WriteAllText(FILEPATH , "");
                    _domain.UpdateFinderFormStatus(new UpdateStatusModel {Id = json.FinderFormId, Status = 3}, Guid.Empty);
                    _logger.LogInformation("xóa gòi nè heeee !!!!");
                }
            }
        }
    }
}
