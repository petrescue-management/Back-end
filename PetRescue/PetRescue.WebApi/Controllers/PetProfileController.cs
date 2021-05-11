
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Models;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using static PetRescue.Data.ViewModels.PetProfileModel;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    [Route("/api/pet-profiles/")]
    public class PetProfileController : BaseController
    {
        private readonly PetProfileDomain _petProfileDomain;
        private readonly IHostingEnvironment _env;
        public PetProfileController(IUnitOfWork uow, PetProfileDomain petProfileDomain, IHostingEnvironment env) : base(uow)
        {
            this._petProfileDomain = petProfileDomain;
            this._env = env;
        }
        #region GET
        [HttpGet]
        [Route("get-pet-breeds-by-type-id/")]
        public IActionResult GetPetBreedsByTypeId(Guid id)
        {
            try
            {
                var result = _petProfileDomain.GetPetBreedsByTypeId(id);
                if (result != null)
                    return Success(result);
                return Success("This pet type do not have any pet breeds !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-pet_breed-by-id/")]
        public IActionResult GetPetBreedById(Guid id)
        {
            try
            {
                var result = _petProfileDomain.GetPetBreedById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-all-pet-fur_colors")]
        public IActionResult GetAllPetFurColors()
        {
            try
            {
                var result = _petProfileDomain.GetAllPetFurColors();
                if (result != null)
                    return Success(result);
                return Success("Do not have any pet fur colors !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-pet-fur_color-by-id/")]
        public IActionResult GetPetFurColorById(Guid id)
        {
            try
            {
                var result =_petProfileDomain.GetPetFurColorById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-all-pet-types")]
        public IActionResult GetAllPetTypes()
        {
            try
            {
                var result = _petProfileDomain.GetAllPetTypes();
                if (result != null)
                    return Success(result);
                return Success("Do not have any pet types !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-all-pet-breeds")]
        public IActionResult GetAllPetBreed()
        {
            try
            {
                var result = _petProfileDomain.GetAllPetBreeds();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-pet-type-by-id/")]
        public IActionResult GetPetTypeById(Guid id)
        {
            try
            {
                var result = _petProfileDomain.GetPetTypeById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("search-pet-profile")]
        public IActionResult SearchPetProfile([FromQuery] SearchPetProfileModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _petProfileDomain.SearchPetProfile(model, Guid.Parse(currentCenterId));
                if (result != null)
                    return Success(result);
                return Success("Not have any pet profile !");
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("get-list-pet-to-be-registered-for-adoption")]
        public IActionResult GetListPetToBeRegisteredForAdoption([FromQuery] PetProfileFilter filter)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _petProfileDomain.GetListPetsToBeRegisteredForAdoption(Guid.Parse(currentCenterId), filter);
                if (result != null)
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
        [HttpGet]
        [Route("get-list-adoption-register-form-by-petid/")]
        public IActionResult GetListPetToBeRegisteredForAdoption([FromQuery] Guid petId)
        {
            try
            {
                var result = _petProfileDomain.GetListAdoptionRegisterFormByPetId(petId);
                if (result != null)
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
        [Authorize]
        [HttpGet]
        [Route("get-adoption-by-userId")]
        public IActionResult GetAdoptionByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _petProfileDomain.GetListAdoptionPetByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("get-adoption-by-centerid")]
        public IActionResult GetAdoptionByCenterId([FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _petProfileDomain.GetListAdoptionByCenterId(Guid.Parse(currentCenterId), page, limit);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-adoption-by-petprofileid")]
        public IActionResult GetAdoptionByPetProfileId([FromQuery] Guid petProfileId)
        {
            try
            {
                var result = _petProfileDomain.GetAdoptionByPetId(petProfileId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-pet-by-typename")]
        public IActionResult GetPetByTypeName([FromQuery]PetProfileFilter filter) 
        {
            try
            {
                var result = _petProfileDomain.GetPetByTypeName(filter);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-pet")]
        public IActionResult GetPet([FromQuery] PetProfileFilter filter, [FromQuery] string[] fields, [FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                if(fields.Length == 0)
                {
                    fields = new string[] { PetFieldConst.INFO };
                }
                var result = _petProfileDomain.GetPet(filter, fields, page, limit);
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-document-pet")]
        public IActionResult GetRescueDocumentByPetId([FromQuery]Guid petProfileId)
        {
            try
            {
                var result = _petProfileDomain.GetRescueDocumentByPetId(petProfileId);
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
        #region POST
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPost]
        [Route("create-pet-profile")]
        public IActionResult CreatePetProfile([FromBody] CreatePetProfileModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _petProfileDomain.CreatePetProfile(model, Guid.Parse(currentUserId), Guid.Parse(currentCenterId));
                if (result != null)
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
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost]
        [Route("create-new-pet-breed")]
        public IActionResult CreateBreed([FromBody] PetBreedCreateModel model)
        {
            try
            {
                var newPetBreed = _petProfileDomain.CreatePetBreed(model);
                if (newPetBreed)
                {
                    return Success(newPetBreed);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost]
        [Route("create-new-pet-type")]
        public IActionResult CreateType([FromBody] PetTypeCreateModel model)
        {
            try
            {
                var newPetType = _petProfileDomain.CreatePetType(model);
                if (newPetType)
                {
                    return Success(newPetType);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles = RoleConstant.Admin)]
        [HttpPost]
        [Route("create-new-pet-fur-color")]
        public IActionResult CreateFurColor([FromBody] PetFurColorCreateModel model)
        {
            try
            {
                var newPetFurColor = _petProfileDomain.CreatePetFurColor(model);
                if (newPetFurColor)
                {
                    return Success(newPetFurColor);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        #region PUT
        //[Authorize(Roles = RoleConstant.Admin)]
        [HttpPut]
        [Route("update-fur-color/")]
        public IActionResult UpdateFurColor([FromBody] PetFurColorUpdateModel model)
        {
            try
            {
                var petFurColor = _petProfileDomain.UpdatePetFurColor(model);
                if (petFurColor)
                {
                    return Success(petFurColor);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles = RoleConstant.Admin)]
        [HttpPut]
        [Route("update-breed/{id}")]
        public IActionResult UpdateBreed([FromBody] PetBreedUpdateModel model)
        {
            try
            {
                var petBreed = _petProfileDomain.UpdatePetBreed(model);
                if (petBreed)
                {
                    return Success(petBreed);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles = RoleConstant.Admin)]
        [HttpPut]
        [Route("update-type/")]
        public IActionResult UpdateType([FromBody] PetTypeUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var petType = _domain.UpdatePetType(model);
                if (petType)
                {
                    return Success(petType);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("update-pet-profile/")]
        public IActionResult UpdatePetProfile([FromBody] UpdatePetProfileModel model)
        {
            try
            {
                var path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _petProfileDomain.UpdatePetProfile(model, Guid.Parse(currentUserId), path);
                if (result != null)
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
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("pick-pet")]
        public IActionResult PickPet([FromBody] PetAdoptionCreateModel model)
        {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
            var path = _env.ContentRootPath;
            var result =  _petProfileDomain.PickPet(model.AdoptionRegistrationFormId, Guid.Parse(currentUserId), path);
            try
            {
                if (result != null)
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
        #endregion
    }
}
