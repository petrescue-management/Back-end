using Microsoft.AspNetCore.Authorization;
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
    [Route("/api/adoption-report-trackings/")]
    public class AdoptionReportTrackingController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly AdoptionReportTrackingDomain _adoptionReportTrackingDomain;
        public AdoptionReportTrackingController(IUnitOfWork uow, IHostingEnvironment environment, AdoptionReportTrackingDomain adoptionReportTrackingDomain) : base(uow)
        {
            this._env = environment;
            this._adoptionReportTrackingDomain = adoptionReportTrackingDomain;
        }
        [HttpGet]
        [Route("get-by-adoption-report-tracking-id")]
        public IActionResult GetByAdoptionReportTrackingId([FromQuery]Guid adoptionReportTrackingId)
        {
            try
            {
                var result = _adoptionReportTrackingDomain.GetByAdoptionReportTrackingId(adoptionReportTrackingId);
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("get-list-adoption-report-tracking-by-userid")]
        public IActionResult GetListAdoptionReportTrackingByUserId([FromQuery] Guid petProfileId)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                if(petProfileId.Equals(Guid.Empty) || petProfileId != null)
                {
                    var result = _adoptionReportTrackingDomain.GetListAdoptionReportTrackingByUserId(Guid.Parse(currentUserId), petProfileId);
                    return Success(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("create-adoption-report-tracking")]
        public IActionResult CreateAdoptionReportTracking ([FromBody]AdoptionReportTrackingCreateModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _adoptionReportTrackingDomain.Create(model, Guid.Parse(currentUserId), path);
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
        [Authorize]
        [HttpPut]
        [Route("update-adoption-report-tracking")]
        public IActionResult UpdateAdoptionReportTracking([FromBody] AdoptionReportTrackingUpdateModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _adoptionReportTrackingDomain.Edit(model, Guid.Parse(currentUserId));
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
