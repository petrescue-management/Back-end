
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    public class PetProfileController : BaseController
    {
        public PetProfileController(IUnitOfWork uow) : base(uow)
        {
        }
        #region GET
        [HttpGet]
        [Route("api/get-pet-breeds-by-type-id/")]
        public IActionResult GetPetBreedsByTypeId(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetPetBreedsByTypeId(id);
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
        [Route("api/get-pet_breed-by-id/")]
        public IActionResult GetPetBreedById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetPetBreedById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-all-pet-fur_colors")]
        public IActionResult GetAllPetFurColors()
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetAllPetFurColors();
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
        [Route("api/get-pet-fur_color-by-id/")]
        public IActionResult GetPetFurColorById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetPetFurColorById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-all-pet-types")]
        public IActionResult GetAllPetTypes()
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetAllPetTypes();
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
        [Route("api/get-pet-type-by-id/")]
        public IActionResult GetPetTypeById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetProfileDomain>().GetPetTypeById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/search-pet-profile")]
        public IActionResult SearchPetProfile([FromQuery] SearchPetProfileModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _uow.GetService<PetProfileDomain>().SearchPetProfile(model, Guid.Parse(currentCenterId));
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
        [Route("api/get-list-pet-to-be-registered-for-adoption")]
        public IActionResult GetListPetToBeRegisteredForAdoption([FromQuery] PetProfileFilter filter)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetProfileDomain>();
                var result = _domain.GetListPetsToBeRegisteredForAdoption(Guid.Parse(currentCenterId), filter);
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
        [Route("api/get-list-adoption-register-form-by-petid/")]
        public IActionResult GetListPetToBeRegisteredForAdoption([FromQuery] Guid petId)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var result = _domain.GetListAdoptionRegisterFormByPetId(petId);
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
        [HttpGet]
        [Route("api/get-pet-by-typename")]
        public IActionResult GetPetByTypeName()
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var result = _domain.GetPetByTypeName();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-pet")]
        public IActionResult GetPet([FromQuery] PetProfileFilter filter, [FromQuery] string[] fields, [FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                if(fields.Length == 0)
                {
                    fields = new string[] { PetFieldConst.INFO };
                }
                var result = _domain.GetPet(filter, fields, page, limit);
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-document-pet")]
        public IActionResult GetDocumentPetBy([FromQuery]Guid petDocumentId)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var result = _domain.GetDocumentPetById(petDocumentId);
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
        [Route("api/create-pet-profile")]
        public IActionResult CreatePetProfile([FromBody] CreatePetProfileModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetProfileDomain>();
                var result = _domain.CreatePetProfile(model, Guid.Parse(currentUserId), Guid.Parse(currentCenterId));
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
        [Route("api/create-new-pet-breed")]
        public IActionResult CreateBreed([FromBody] PetBreedCreateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var newPetBreed = _domain.CreatePetBreed(model);
                if (newPetBreed != -1)
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
        [Route("api/create-new-pet-type")]
        public IActionResult CreateType([FromBody] PetTypeCreateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var newPetType = _domain.CreatePetType(model);
                if (newPetType != -1)
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
        [Route("api/create-new-pet-fur-color")]
        public IActionResult CreateFurColor([FromBody] PetFurColorCreateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var newPetFurColor = _domain.CreatePetFurColor(model);
                if (newPetFurColor != -1)
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
        [Route("api/update-fur-color/")]
        public IActionResult UpdateFurColor([FromBody] PetFurColorUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var petFurColor = _domain.UpdatePetFurColor(model);
                if (petFurColor != -1)
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
        [Route("api/update-breed/{id}")]
        public IActionResult UpdateBreed([FromBody] PetBreedUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var petBreed = _domain.UpdatePetBreed(model);
                if (petBreed != -1)
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
        //[Authorize(Roles =RoleConstant.Admin)]
        [HttpPut]
        [Route("api/update-type/")]
        public IActionResult UpdateType([FromBody] PetTypeUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetProfileDomain>();
                var petType = _domain.UpdatePetType(model);
                if (petType != -1)
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
        [Route("api/update-pet-profile/")]
        public IActionResult UpdatePetProfile([FromBody] UpdatePetProfileModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var petProfileService = _uow.GetService<PetProfileDomain>();
                var result = petProfileService.UpdatePetProfile(model, Guid.Parse(currentUserId));
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
        #endregion
    }
}
