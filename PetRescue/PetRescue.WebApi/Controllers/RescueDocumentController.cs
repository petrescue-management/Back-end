using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
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
    [Route("/api/rescue-documents/")]
    public class RescueDocumentController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly RescueDocumentDomain _rescueDocumentDomain;
        public RescueDocumentController(IUnitOfWork uow, IHostingEnvironment environment, RescueDocumentDomain rescueDocumentDomain) : base(uow)
        {
            this._env = environment;
            this._rescueDocumentDomain = rescueDocumentDomain;
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("get-list-pet-document")]
        public IActionResult GetListRescueDocumentByCenterId([FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _rescueDocumentDomain.GetListRescueDocumentByCenterId(Guid.Parse(currentCenterId),page, limit);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("get-list-petprofile-by-petdocumentid")]
        public IActionResult GetListPetProfileByRescueDocumentId([FromQuery] Guid petDocumentId)
        {
            try
            {
                var result = _rescueDocumentDomain.GetListPetProfileByRescueDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("get-pet-document-by-id")]
        public IActionResult GetRescueDocumentById([FromQuery]Guid petDocumentId)
        {
            try
            {
                var result = _rescueDocumentDomain.GetRescueDocumentByRescueDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpPost]
        [Route("create-pet-document")]
        public IActionResult CreateRescueDocument([FromBody] RescueDocumentCreateModel model)
        {
            try
            {
                var result = _rescueDocumentDomain.CreateRescueDocument(model);
                if (result)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("update-pet-document")]
        public IActionResult UpdateRescueDocument([FromBody] RescueDocumentUpdateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _rescueDocumentDomain.Edit(model, Guid.Parse(currentUserId));
                if (result)
                {
                    return Success(result);
                }
                return BadRequest();   
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        
    }
}
