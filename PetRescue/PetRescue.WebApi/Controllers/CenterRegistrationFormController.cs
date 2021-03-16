using FirebaseAdmin.Messaging;
﻿using FirebaseAdmin;
using FirebaseAdmin.Messaging;
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
                var userDomain = _uow.GetService<UserDomain>();
                var listToken = userDomain.GetListDeviceTokenByRoleAndApplication(RoleConstant.Admin, ApplicationNameHelper.SYSTEMADMINAPP);
                //send notification to sysadmin
                if (listToken.Count > 0)
                {
                    var firebaseExtensions = new FireBaseExtentions();
                    var app = firebaseExtensions.GetFirebaseApp(path);
                    var fcm = FirebaseMessaging.GetMessaging(app);
                    Message message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.NEW_REGISTRATION_CENTER_FORM_TITLE,
                            Body = NotificationBodyHelper.NEW_REGISTRATION_CENTER_FORM_BODY,
                        },
                    };
                    foreach (var token in listToken)
                    {
                        message.Token = token.DeviceToken;
                        await fcm.SendAsync(message);
                    }
                    app.Delete();
                }
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
                var form = _uow.GetService<CenterRegistrationFormDomain>().ProcressCenterRegistrationForm(model, Guid.Parse(currentUserId));

                if(form != null)
                {
                    if(form.CenterRegistrationStatus == CenterRegistrationFormStatusConst.APPROVED)
                    {
                        MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveRegistrationCenter(form.Email), MailConstant.APPROVE_REGISTRATION_FORM);
                        bool result =  MailExtensions.SendBySendGrid(mailArguments, null, null);
                        if (result)
                        {
                            _uow.saveChanges();
                            return Success(form.CenterRegistrationStatus);
                        }
                        return BadRequest();
                    }
                    else if(form.CenterRegistrationStatus == CenterRegistrationFormStatusConst.REJECTED)
                    {
                        MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.RejectRegistrationCenter(form.Email), MailConstant.REJECT_REGISTRATION_FORM);
                        bool result = MailExtensions.SendBySendGrid(mailArguments, null, null);
                        if (result)
                        {
                            _uow.saveChanges();
                            return Success(form.CenterRegistrationStatus);
                        }
                        return BadRequest();
                    }
                }
                return BadRequest();
                
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        //[HttpGet]
        //[Route("Test212")]
        //public IActionResult Test()
        //{
        //    try
        //    {
        //        MailMessage message = new MailMessage();
        //        message.IsBodyHtml = true;
        //        message.Subject = "Subject";
        //        message.To.Add("pjnochjo3095@gmail.com");
        //        message.Body = MailConstant.ApproveRegistrationCenter("petrescue2021@gmail.com");
        //        message.From = new MailAddress("petrescue2021@gmail.com", "Rescue Them");
        //        SmtpClient client = new SmtpClient("smtp.sendgrid.net");
        //        client.UseDefaultCredentials = false;
        //        client.Credentials = new NetworkCredential("apikey", "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");
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.Port = 587; // I have tried with 25 and 2525
        //        client.Timeout = 99999;
        //        client.EnableSsl = false;
        //        client.Send(message);
        //        return Success("Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex.Message);
        //    }
            
        //}
    }
}
