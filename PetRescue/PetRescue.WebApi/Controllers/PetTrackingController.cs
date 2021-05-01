using Microsoft.AspNetCore.Authorization;
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
    public class PetTrackingController : BaseController
    {
        public PetTrackingController(IUnitOfWork uow) : base(uow)
        {
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-pet-tracking-by-id")]
        public IActionResult GetPetTrackingById([FromQuery]Guid petTrackingId)
        {
            try
            {
                var _domain = _uow.GetService<PetTrackingDomain>();
                var result = _domain.GetPetTrackingById(petTrackingId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-pet-tracking-by-petprofileid")]
        public IActionResult GetListPetTrackingByPetProfileId([FromQuery] Guid petProfileId)
        {
            try
            {
                var _domain = _uow.GetService<PetTrackingDomain>();
                var result = _domain.GetListPetTrackingByPetProfileId(petProfileId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("api/create-pet-tracking")]
        public IActionResult CreatePetTracking([FromBody]PetTrackingCreateModel model) 
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<PetTrackingDomain>();
                var result = _domain.Create(model, Guid.Parse(currentUserId));
                if(result != null)
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
        [HttpPost]
        [Route("api/create-pet-tracking-by-user")]
        public IActionResult CreatePetTrackingByUser([FromBody] CreatePetTrackingByUserModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<PetTrackingDomain>();
                var result = _domain.CreatePetTrackingByUser(model, Guid.Parse(currentUserId));
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
    }
}
