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
    public class PetDocumentController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public PetDocumentController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            this._env = environment;
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/get-list-pet-document")]
        public IActionResult GetListPetDocument()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.GetListPetDocumentByCenterId(Guid.Parse(currentCenterId));
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("api/get-list-petprofile-by-petdocumentid")]
        public IActionResult GetListPetProfileByPetDocumentId([FromQuery] Guid petDocumentId)
        {
            try
            {
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.GetListPetProfileByPetDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpGet]
        [Route("api/get-pet-document-by-id")]
        public IActionResult GetPetDocumentById([FromQuery]Guid petDocumentId)
        {
            try
            {
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.GetPetDocumentByPetDocumentId(petDocumentId);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpPost]
        [Route("api/create-pet-document")]
        public IActionResult CreatePetDocument([FromBody] PetDocumentCreateModel model)
        {
            try
            {
                var path = _env.ContentRootPath;
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.CreatePetDocument(model, Guid.Parse(currentCenterId), Guid.Parse(currentUserId), path);
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
        public IActionResult UpdatePetDocument([FromBody]PetDocumentUpdateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.Edit(model, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        
    }
}
