using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            string FILEPATH_NOTI = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");

            string FILEPATH_CONFIG_TIME = 
                Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToNotificationForFinderForm.json");

            var fileJsonNoti = File.ReadAllText(FILEPATH_NOTI);
            
            var fileJsonConfigTime = File.ReadAllText(FILEPATH_CONFIG_TIME);
           
            if(fileJsonNoti != null && fileJsonConfigTime != null)
            {
                var objJsonNoti = JObject.Parse(fileJsonNoti);
                JArray notiArrary = (JArray)objJsonNoti["Notifications"];

                if (notiArrary.Count != 0)
                {
                    var objJsonConfigTime = JObject.Parse(fileJsonConfigTime);

                    var tmp = notiArrary.ElementAt(0);

                    _logger.LogInformation(int.Parse(objJsonConfigTime["ReNotiTime"].Value<string>()) + "-"
                        + int.Parse(objJsonConfigTime["DestroyNotiTime"].Value<string>()));

                    if (DateTime.UtcNow.Minute == 
                        tmp["CurrentTime"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["ReNotiTime"].Value<string>())).Minute)
                    {
                        _domain.ReNotification(Guid.Parse(tmp["FinderFormId"].Value<string>()), tmp["Path"].Value<string>());
                        _logger.LogInformation("noti lại nè heeee !!!!" );
                    }

                    if (DateTime.UtcNow.Minute == 
                        tmp["CurrentTime"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["DestroyNotiTime"].Value<string>())).Minute)
                    {
                        File.WriteAllText(FILEPATH_NOTI, "");
                        if (_domain.GetFinderFormById(Guid.Parse(tmp["FinderFormId"].Value<string>())).FinderFormStatus == 1)
                        {
                            _domain.DestroyNotification(Guid.Parse(tmp["FinderFormId"].Value<string>()),
                                Guid.Parse(tmp["InsertedBy"].Value<string>()), tmp["Path"].Value<string>());
                            _logger.LogInformation("xóa gòi nè heeee !!!!");
                        }
                    }
                }
            }
        }
    }
}
