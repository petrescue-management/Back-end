using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Extensions;
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
    public class AdoptionRegisterFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public AdoptionRegisterFormController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/search-adoption-register-form")]
        public IActionResult SearchAdoptionRegisterForm([FromQuery] SearchModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _uow.GetService<AdoptionRegisterFormDomain>().SearchAdoptionRegisterForm(model, currentCenterId);
                if (result != null)
                    return Success(result);
                return Success("Do not have any adoption register forms !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-adoption-register-form-by-id/{id}")]
        public IActionResult GetAdoptionRegisterFormById(Guid id)
        {
            try
            {
                var result = _uow.GetService<AdoptionRegisterFormDomain>().GetAdoptionRegisterFormById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("api/update-adoption-register-form-status")]
        public async Task<IActionResult> UpdateAdoptionRegisterFormStatusAsync(UpdateStatusModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<AdoptionRegisterFormDomain>().UpdateAdoptionRegisterFormStatus(model,Guid.Parse(currentUserId));
                await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenAdoptionFormToBeChangeStatus(path, result.InsertedBy, result.AdoptionRegisterStatus);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("api/create-adoption-register-form")]
        public async Task<IActionResult> CreateUpdateAdoptionRegisterFormStatus(CreateAdoptionRegisterFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<AdoptionRegisterFormDomain>().CreateAdoptionRegisterForm(model, Guid.Parse(currentUserId));
                await _uow.GetService<NotificationTokenDomain>().NotificationForManagerWhenHaveNewAdoptionRegisterForm(path, result.Pet.CenterId);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
