using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
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
                Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");

            var fileJsonNoti = File.ReadAllText(FILEPATH_NOTI);
            
            var fileJsonConfigTime = File.ReadAllText(FILEPATH_CONFIG_TIME);
           
            if(fileJsonNoti != null && fileJsonConfigTime != null)
            {
                var objJsonNoti = JObject.Parse(fileJsonNoti);
                JArray notiArrary = (JArray)objJsonNoti["Notifications"];

                if (notiArrary.Count != 0)
                {
                    var objJsonConfigTime = JObject.Parse(fileJsonConfigTime);

                    foreach (var noti in notiArrary.Children().ToList()) {
                        if (noti["InsertedAt"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["ReNotiTimeForRescue"].Value<string>())).Minute
                            == DateTime.UtcNow.Minute)
                        {
                            _domain.ReNotification(Guid.Parse(noti["FinderFormId"].Value<string>()), noti["Path"].Value<string>());
                            /*_logger.LogInformation("noti lại nè heeee !!!!");*/
                        }

                        if (noti["InsertedAt"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["DestroyNotiTimeForRescue"].Value<string>())).Minute
                            == DateTime.UtcNow.Minute)
                        {
                            if (_domain.GetFinderFormById(Guid.Parse(noti["FinderFormId"].Value<string>())).FinderFormStatus == FinderFormStatusConst.PROCESSING)
                            {
                                _domain.DestroyNotification(Guid.Parse(noti["FinderFormId"].Value<string>()),
                                    Guid.Parse(noti["InsertedBy"].Value<string>()), noti["Path"].Value<string>());
                                /*_logger.LogInformation("xóa gòi nè heeee !!!!");*/
                            }
                        }
                    }
                }
            }
        }
    }
}
