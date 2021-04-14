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
    public class MyCronJob2 : CronJobService
    {
        private readonly ILogger<MyCronJob2> _logger;

        private readonly AdoptionDomain _domain;

        public MyCronJob2(IScheduleConfig<MyCronJob2> config, ILogger<MyCronJob2> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _domain = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AdoptionDomain>();
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 starts.");

            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            Remind();
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 2 is working.");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 2 is stopping.");
            return base.StopAsync(cancellationToken);
        }

        public void Remind()
        {
            string FILEPATH_REMIND = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "RemindReportAfterAdopt.json");

            string FILEPATH_CONFIG_TIME =
                Path.Combine(Directory.GetCurrentDirectory(), "JSON", "ConfigTimeToNotification.json");

            var fileJsonNoti = File.ReadAllText(FILEPATH_REMIND);

            var fileJsonConfigTime = File.ReadAllText(FILEPATH_CONFIG_TIME);

            if (fileJsonNoti != null)
            {
                var objJson = JObject.Parse(fileJsonNoti);
                var remindArrary = objJson.GetValue("Reminders") as JArray;

                if (remindArrary.Count != 0)
                {
                    var objJsonConfigTime = JObject.Parse(fileJsonConfigTime);

                    foreach (var remind in remindArrary.Children().ToList())
                    {
                        if(remind["AdoptedAt"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["RemindTimeAfterAdopt"].Value<string>())).Minute
                            == DateTime.UtcNow.Minute)
                        {
                            _domain.Remind(Guid.Parse(remind["OwnerId"].Value<string>()), remind["Path"].Value<string>());
                           /* _logger.LogInformation("month 1");*/
                        }

                        if (remind["AdoptedAt"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["RemindTimeAfterAdopt"].Value<string>()) * 2).Minute
                            == DateTime.UtcNow.Minute)
                        {
                            _domain.Remind(Guid.Parse(remind["OwnerId"].Value<string>()), remind["Path"].Value<string>());
                            /*_logger.LogInformation("month 2");*/
                        }

                        if (remind["AdoptedAt"].Value<DateTime>().AddMinutes(int.Parse(objJsonConfigTime["RemindTimeAfterAdopt"].Value<string>()) * 3).Minute 
                            == DateTime.UtcNow.Minute)
                        {
                            _domain.Remind(Guid.Parse(remind["OwnerId"].Value<string>()), remind["Path"].Value<string>());
                            /*_logger.LogInformation("month 3");*/

                            remindArrary.Remove(remind);

                            if (remindArrary.Count == 0)
                                remindArrary = new JArray();

                            string output = Newtonsoft.Json.JsonConvert.SerializeObject(objJson, Newtonsoft.Json.Formatting.Indented);
                            File.WriteAllText(FILEPATH_REMIND, output);
                        }
                    }

                  
                }
            }
        }
    }
}
