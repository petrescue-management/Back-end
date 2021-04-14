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

        #region GET TIME TO NOTIFICATION FOR FINDER FORM
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/get-time-to-notification-for-finder-form")]
        public IActionResult GetTimeToNotificationForFinderForm()
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().GetTimeToNotificationForFinderForm();                
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CONFIG TIME TO NOTIFICATION FOR FINDER FORM
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/config-time-to-notification-for-finder-form")]
        public IActionResult ConfigTimeToNotificationForFinderForm([FromQuery] int ReNotiTime, int DestroyNotiTime)
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().ConfigTimeToNotificationForFinderForm(ReNotiTime, DestroyNotiTime);
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

        #region GET TIME TO REMIND FOR REPORT AFTER ADOPT
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/get-time-to-remind-for-report-after-adopt")]
        public IActionResult GetTimeToRemindForReportAfterAdopt()
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().GetTimeToRemindForReportAfterAdopt();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CONFIG TIME TO REMIND FOR REPORT AFTER ADOPT
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/config-time-to-remind-for-report-after-adopt")]
        public IActionResult ConfigTimeToRemindForReportAfterAdopt([FromQuery] int RemindTime)
        {
            try
            {
                var result = _uow.GetService<ConfigDomain>().ConfigTimeToRemindForReportAfterAdopt(RemindTime);
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
