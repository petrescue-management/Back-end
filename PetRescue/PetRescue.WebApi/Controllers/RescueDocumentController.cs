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
    public class RescueDocumentController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public RescueDocumentController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            this._env = environment;
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/get-list-pet-document")]
        public IActionResult GetListRescueDocumentByCenterId([FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<RescueDocumentDomain>();
                var result = _domain.GetListRescueDocumentByCenterId(Guid.Parse(currentCenterId),page, limit);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("api/get-list-petprofile-by-petdocumentid")]
        public IActionResult GetListPetProfileByRescueDocumentId([FromQuery] Guid petDocumentId)
        {
            try
            {
                var _domain = _uow.GetService<RescueDocumentDomain>();
                var result = _domain.GetListPetProfileByRescueDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("api/get-pet-document-by-id")]
        public IActionResult GetRescueDocumentById([FromQuery]Guid petDocumentId)
        {
            try
            {
                var _domain = _uow.GetService<RescueDocumentDomain>();
                var result = _domain.GetRescueDocumentByRescueDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpPost]
        [Route("api/create-pet-document")]
        public IActionResult CreateRescueDocument([FromBody] RescueDocumentCreateModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<RescueDocumentDomain>();
                var result = _domain.CreateRescueDocument(model, Guid.Parse(currentCenterId));
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
        [Route("api/update-pet-document")]
        public IActionResult UpdateRescueDocument([FromBody] RescueDocumentUpdateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<RescueDocumentDomain>();
                var result = _domain.Edit(model, Guid.Parse(currentUserId));
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
