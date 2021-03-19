using FirebaseAdmin.Messaging;
﻿using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Google;
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
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class CenterRegistrationFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public CenterRegistrationFormController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }
        #region SEARCH
        [HttpGet]
        [Route("api/search-center-registration-form")]
        public IActionResult SearchCenterRegistrationForm([FromHeader] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<CenterRegistrationFormDomain>().SearchCenterRegistrationForm(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any center registration forms !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("api/get-center-registration-form-by-id/{id}")]
        public IActionResult GetCenterRegistrationFormById(Guid id)
        {
            try
            {
                var result = _uow.GetService<CenterRegistrationFormDomain>().GetCenterRegistrationFormById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CREATE
        [HttpPost]
        [Route("api/create-center-registration-form")]
        public async Task<IActionResult> CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                string result = _uow.GetService<CenterRegistrationFormDomain>().CreateCenterRegistrationForm(model);
                await _uow.GetService<NotificationTokenDomain>().NotificationForAdminWhenHaveNewCenterRegisterForm(path);
                if (result.Contains("is already"))
                    return BadRequest(result);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region PROCESS FORM
        [Authorize(Roles ="sysadmin")]
        [HttpPost]
        [Route("api/procress-center-registration-form")]
        public IActionResult ProcressCenterRegistrationForm(UpdateStatusModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<CenterRegistrationFormDomain>().ProcressCenterRegistrationForm(model, Guid.Parse(currentUserId));
                if(result != -1)
                {
                    return Success(result);
                }
                return BadRequest();
                
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
    }
}
