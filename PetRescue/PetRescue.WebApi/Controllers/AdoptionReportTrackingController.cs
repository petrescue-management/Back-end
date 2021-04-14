using Microsoft.AspNetCore.Hosting;
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
    public class AdoptionReportTrackingController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public AdoptionReportTrackingController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            this._env = environment;
        }
        [HttpGet]
        [Route("api/get-by-adoption-report-tracking-id")]
        public IActionResult GetByAdoptionReportTrackingId([FromQuery]Guid adoptionReportTrackingId)
        {
            try
            {
                var _domain = _uow.GetService<AdoptionReportTrackingDomain>();
                var result = _domain.GetByAdoptionReportTrackingId(adoptionReportTrackingId);
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [Route("api/create-adoption-report-tracking")]
        public IActionResult CreateAdoptionReportTracking ([FromBody]AdoptionReportTrackingCreateModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<AdoptionReportTrackingDomain>();
                var result = _domain.Create(model, Guid.Parse(currentUserId), path);
                if (result)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPut]
        [Route("api/Update-adoption-report-tracking")]
        public IActionResult UpdateAdoptionReportTracking([FromBody] AdoptionReportTrackingUpdateModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<AdoptionReportTrackingDomain>();
                var result = _domain.Edit(model, Guid.Parse(currentUserId));
                if (result)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
