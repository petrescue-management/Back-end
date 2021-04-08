using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Extensions;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class ConfigController : BaseController
    {
        public ConfigController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [Route("api/config-time-to-notification-for-finder-form")]
        public IActionResult ConfigTimeToNotificationForFinderForm([FromQuery] int ReNotiTime, int DestroyNotiTime)
        {
            try
            {
                var _domain = _uow.GetService<ConfigDomain>().ConfigTimeToNotificationForFinderForm(ReNotiTime, DestroyNotiTime);
                if (_domain == false)
                    return BadRequest("Time for Destroy Notification must be larger than Time for Re-Notification !");
                return Success(_domain);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/config-time-to-remind-for-report-after-adopt")]
        public IActionResult ConfigTimeToRemindForReportAfterAdopt([FromQuery] int RemindTime)
        {
            try
            {
                var _domain = _uow.GetService<ConfigDomain>().ConfigTimeToRemindForReportAfterAdopt(RemindTime);
                return Success(_domain);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
