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
    [Route("/api/pet-trackings/")]
    public class PetTrackingController : BaseController
    {
        private readonly PetTrackingDomain _petTrackingDomain;
        public PetTrackingController(IUnitOfWork uow, PetTrackingDomain petTrackingDomain) : base(uow)
        {
            this._petTrackingDomain = petTrackingDomain;
        }
        [Authorize]
        [HttpGet]
        [Route("get-pet-tracking-by-id")]
        public IActionResult GetPetTrackingById([FromQuery]Guid petTrackingId)
        {
            try
            {
                var result = _petTrackingDomain.GetPetTrackingById(petTrackingId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("get-pet-tracking-by-petprofileid")]
        public IActionResult GetListPetTrackingByPetProfileId([FromQuery] Guid petProfileId)
        {
            try
            {
                var result = _petTrackingDomain.GetListPetTrackingByPetProfileId(petProfileId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("create-pet-tracking")]
        public IActionResult CreatePetTracking([FromBody]PetTrackingCreateModel model) 
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _petTrackingDomain.Create(model, Guid.Parse(currentUserId));
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
        [Route("create-pet-tracking-by-user")]
        public IActionResult CreatePetTrackingByUser([FromBody] CreatePetTrackingByUserModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _petTrackingDomain.CreatePetTrackingByUser(model, Guid.Parse(currentUserId));
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
