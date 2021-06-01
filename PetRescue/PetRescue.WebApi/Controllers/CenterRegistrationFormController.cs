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
    [Route("/api/center-registration-forms/")]
    public class CenterRegistrationFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly CenterRegistrationFormDomain _centerRegistrationFormDomain;
        private readonly NotificationTokenDomain _notificationTokenDomain;
        public CenterRegistrationFormController(IUnitOfWork uow, IHostingEnvironment environment, CenterRegistrationFormDomain centerRegistrationFormDomain, NotificationTokenDomain notificationTokenDomain) : base(uow)
        {
            _env = environment;
            this._centerRegistrationFormDomain = centerRegistrationFormDomain;
            this._notificationTokenDomain = notificationTokenDomain;
        }
        #region SEARCH
        [HttpGet]
        [Route("search-center-registration-form")]
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
        [Route("get-center-registration-form-by-id/{id}")]
        public IActionResult GetCenterRegistrationFormById(Guid id)
        {
            try
            {
                var result = _centerRegistrationFormDomain.GetCenterRegistrationFormById(id);
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
        [Route("create-center-registration-form")]
        public async Task<IActionResult> CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                string result = _centerRegistrationFormDomain.CreateCenterRegistrationForm(model);
                await _notificationTokenDomain.NotificationForAdminWhenHaveNewCenterRegisterForm(path);
                if (result.Contains("is already"))
                {
                    return BadRequest(result);
                }
                await _notificationTokenDomain.NotificationForAdminWhenHaveNewCenterRegisterForm(path);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region PROCESS FORM
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpPost]
        [Route("procress-center-registration-form")]
        public IActionResult ProcressCenterRegistrationForm(UpdateRegistrationCenter model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _centerRegistrationFormDomain.ProcressCenterRegistrationForm(model, Guid.Parse(currentUserId));
                if (!result.Equals("") && !result.Contains("This"))
                {
                    return Success(result);
                }
                return BadRequest(result);
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
    }
}
