
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

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class PetController : BaseController
    {
        public PetController(IUnitOfWork uow) : base(uow)
        {
        }
        #region GET
        [HttpGet]
        [Route("api/get-pet-breeds-by-type-id/{id}")]
        public IActionResult GetPetBreedsByTypeId(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetBreedsByTypeId(id);
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
        [Route("api/get-pet_breed-by-id/{id}")]
        public IActionResult GetPetBreedById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetBreedById(id);
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
                var result = _uow.GetService<PetDomain>().GetAllPetFurColors();
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
        [Route("api/get-pet-fur_color-by-id/{id}")]
        public IActionResult GetPetFurColorById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetFurColorById(id);
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
                var result = _uow.GetService<PetDomain>().GetAllPetTypes();
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
        [Route("api/get-pet-type-by-id/{id}")]
        public IActionResult GetPetTypeById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetTypeById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-pet")]
        public IActionResult GetPet([FromQuery] PetFilter filter, [FromQuery] string[] fields, [FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                
                var petDomain = _uow.GetService<PetDomain>();
                if (fields.Length == 0)
                {
                    fields = new string[] { PetFieldConst.INFO };
                }
                var result = petDomain.GetPet(filter, fields, page, limit);
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
        [HttpGet]
        [Route("api/get-list-pet-to-be-registered-for-adoption")]
        public IActionResult GetListPetToBeRegisteredForAdoption()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetDomain>();
                var result = _domain.GetListPetsToBeRegisteredForAdoption(Guid.Parse(currentCenterId));
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }   
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/get-list-adoption-register-form-by-petid/{id}")]
        public IActionResult GetListPetToBeRegisteredForAdoption([FromQuery]Guid petId)
        {
            try
            {
                var _domain = _uow.GetService<PetDomain>();
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
        #endregion
        #region POST
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPost]
        [Route("api/create-new-pet")]
        public IActionResult CreatePet([FromBody] PetCreateModel model)
        {
            try
            {
                var currentUserId  = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetDomain>();
                var newPet = _domain.CreateNewPet(model,Guid.Parse(currentUserId), Guid.Parse(currentCenterId));
                if(newPet != null)
                {
                    return Success(newPet);
                }
                return BadRequest();
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost]
        [Route("api/create-new-pet-breed")]
        public IActionResult CreateBreed([FromBody]PetBreedCreateModel model) 
        {
            try
            {
                var _domain = _uow.GetService<PetDomain>();
                var newPetBreed = _domain.CreatePetBreed(model);
                if (newPetBreed != null)
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
                var _domain = _uow.GetService<PetDomain>();
                var newPetType = _domain.CreatePetType(model);
                if (newPetType != null)
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
                var _domain = _uow.GetService<PetDomain>();
                var newPetFurColor = _domain.CreatePetFurColor(model);
                if(newPetFurColor != null)
                {
                    return Success(newPetFurColor);
                }
                return BadRequest();
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        #region PUT
        //[Authorize(Roles = RoleConstant.Admin)]
        [HttpPut]
        [Route("api/update-fur-color/{id}")]
        public IActionResult UpdateFurColor([FromBody] PetFurColorUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetDomain>();
                var petFurColor = _domain.UpdatePetFurColor(model);
                if (petFurColor != null)
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
                var _domain = _uow.GetService<PetDomain>();
                var petBreed = _domain.UpdatePetBreed(model);
                if (petBreed != null)
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
        [Route("api/update-type/{id}")]
        public IActionResult UpdateType([FromBody] PetTypeUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<PetDomain>();
                var petType = _domain.UpdatePetType(model);
                if (petType != null)
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
        [Route("api/update-pet/{id}")]
        public IActionResult UpdatePet([FromBody] PetDetailModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var petDomain = _uow.GetService<PetDomain>();
                var newPet = petDomain.UpdatePet(model, Guid.Parse(currentUserId));
                if (newPet != null)
                {
                    return Success(newPet);
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
