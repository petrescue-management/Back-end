using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class RescueReportController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public RescueReportController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

        #region SEARCH
        [HttpGet]
        [Route("api/search-rescue-report")]
        public IActionResult SearchRescueReport([FromQuery] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<RescueReportDomain>().SearchRescueReport(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any rescue report !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("api/get-rescue-report-by-id/{id}")]
        public IActionResult GetRescueReportById(Guid id)
        {
            try
            {
                var result = _uow.GetService<RescueReportDomain>().GetRescueReportById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region UPDATE STATUS
        [HttpPut]
        [Route("api/update-rescue-report-status")]
        public IActionResult UpdateRescueReportStatus(UpdateStatusModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<RescueReportDomain>().UpdateRescueReportStatus(model, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CREATE
        [HttpPost]
        [Route("api/create-rescue-report")]
        public async Task<IActionResult> CreateRescueReportAsync(CreateRescueReportModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result =  await _uow.GetService<RescueReportDomain>().CreateRescueReportAsync(model, Guid.Parse(currentUserId), path);
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
