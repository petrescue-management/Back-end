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
    public class ConfigController : BaseController
    {
        public ConfigController(IUnitOfWork uow) : base(uow)
        {
        }

        #region GET TIME TO NOTIFICATION
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/get-time-to-notification")]
        public IActionResult GetTimeToNotification()
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().GetTimeToNotification();                
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
        [Route("api/config-time-to-notification")]
        public IActionResult ConfigTimeToNotification([FromQuery] int reNotiTime, int destroyNotiTime, int remindTime)
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().ConfigTimeToNotification(reNotiTime, destroyNotiTime, remindTime);
                if (result == false)
                    return BadRequest("Time for Destroy Notification must be larger than Time for Re-Notification !");
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
