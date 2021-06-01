using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
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
    [Route("/api/config/")]
    public class ConfigController : BaseController
    {
        private readonly ConfigDomain _configDomain;
        public ConfigController(IUnitOfWork uow, ConfigDomain configDomain) : base(uow)
        {
            this._configDomain = configDomain;
        }

        #region GET TIME TO NOTIFICATION
        [HttpGet]
        [Route("get-system-parameters")]
        public IActionResult GetTimeToNotification()
        {
            try
            {
                var result = _configDomain.GetTimeToNotification();                
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CONFIG TIME TO NOTIFICATION
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost]
        [Route("configure-systems-parameters")]
        public IActionResult ConfigTimeToNotification([FromQuery] int reNotiTimeForOnline, int reNotiTimeForAll, int notiTimeForDestroy,
            int remindTime, int imgFinder, int imgPicker, double nearestDistance)
        {
            try
            {
                var result = _configDomain.ConfigTimeToNotification(reNotiTimeForOnline, reNotiTimeForAll, notiTimeForDestroy, remindTime, imgFinder, imgPicker, nearestDistance);
                if (result == false)
                    return BadRequest("Time for Destroy Notification must be larger than Time for Re-Notification All" +
                        "and Time for Re-notification All must be larger than Time for Re-notification Online!");
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

    }
}
