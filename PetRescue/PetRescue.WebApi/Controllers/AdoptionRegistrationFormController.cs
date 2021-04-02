﻿using Microsoft.AspNetCore.Authorization;
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
    public class AdoptionRegistrationFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public AdoptionRegistrationFormController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/search-adoption-registration-form")]
        public IActionResult SearchAdoptionRegistrationForm([FromQuery] SearchModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _uow.GetService<AdoptionRegistrationFormDomain>().SearchAdoptionRegistrationForm(model, currentCenterId);
                if (result != null)
                    return Success(result);
                return Success("Do not have any adoption registration forms !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-adoption-registration-form-by-id/{id}")]
        public IActionResult GetAdoptionRegistrationFormById(Guid id)
        {
            try
            {
                var result = _uow.GetService<AdoptionRegistrationFormDomain>().GetAdoptionRegistrationFormById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("api/update-adoption-registration-form-status")]
        public async Task<IActionResult> UpdateAdoptionRegistrationFormStatusAsync(UpdateStatusModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<AdoptionRegistrationFormDomain>().UpdateAdoptionRegistrationFormStatus(model,Guid.Parse(currentUserId));
                await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenAdoptionFormToBeChangeStatus(path, result.InsertedBy, result.AdoptionRegistrationId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("api/create-adoption-registration-form")]
        public async Task<IActionResult> CreateUpdateAdoptionRegisterFormStatus(CreateAdoptionRegistrationFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<AdoptionRegistrationFormDomain>().CreateAdoptionRegistrationForm(model, Guid.Parse(currentUserId));
                await _uow.GetService<NotificationTokenDomain>().NotificationForManagerWhenHaveNewAdoptionRegisterForm(path, result.PetProfile.CenterId, result.AdoptionRegistrationId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-list-adoption-form-by-userID")]
        public IActionResult GetListAdoptionFormByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<AdoptionRegistrationFormDomain>();
                var result = _domain.GetListAdoptionByUserId(Guid.Parse(currentUserId));
                if(result.Count > 0)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        
    }
}