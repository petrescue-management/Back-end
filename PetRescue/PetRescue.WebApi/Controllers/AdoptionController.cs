using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class AdoptionController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public AdoptionController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

       /* [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/search-adoption")]
        public IActionResult SearchAdoption([FromQuery] SearchModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _uow.GetService<AdoptionDomain>().SearchAdoption(model, currentCenterId);
                if (result != null)
                    return Success(result);
                return Success("Do not have any adoptions !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }*/

/*        [HttpGet]
        [Route("api/get-adoption-by-id/{id}")]
        public IActionResult GetAdoptionById(Guid id)
        {
            try
            {
                var result = _uow.GetService<AdoptionDomain>().GetAdoptionById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }*/
        [HttpGet]
        [Route("api/get-adoption-by-adoptionid")]
        public IActionResult GetAdoptionByAdoptionId(Guid id)
        {
            try
            {
                var result = _uow.GetService<AdoptionDomain>().GetAdoptionByAdoptionId(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/get-adoption-by-centerid")]
        public IActionResult GetAdoptionByCenterId()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<AdoptionDomain>();
                var result = _domain.GetListAdoptionByCenterId(Guid.Parse(currentCenterId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-adoption-by-userId")]
        public IActionResult GetAdoptionByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<AdoptionDomain>();
                var result = _domain.GetListAdoptionByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-adoption-by-petprofileid")]
        public IActionResult GetAdoptionByPetProfileId([FromQuery]Guid petProfileId)
        {
            try
            {
                var _domain = _uow.GetService<AdoptionDomain>();
                var result = _domain.GetAdoptionByPetId(petProfileId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        #region CREATE
        [Authorize]
        [HttpPost]
        [Route("api/create-adoption")]
        public IActionResult CreateAdoption([FromBody] AdoptionCreateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var path = _env.ContentRootPath;
                var result = _uow.GetService<AdoptionDomain>()
                    .CreateAdoption(model.AdoptionRegistrationFormId, Guid.Parse(currentUserId), path);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region UPDATE STATUS
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("api/update-adoption-status")]
        public IActionResult UpdateAdoptionStatus(CancelModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var path = _env.ContentRootPath;
                var result = _uow.GetService<AdoptionDomain>().UpdateAdoptionStatusAsync(model, Guid.Parse(currentUserId), path);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion
    }
}
