using FirebaseAdmin.Messaging;
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

        [Authorize(Roles = "manager")]
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


        [HttpPut]
        [Route("api/update-adoption-register-form-status")]
        public IActionResult UpdateAdoptionRegisterFormStatus(UpdateStatusModel model)
        {
            try
            {
                var result = _uow.GetService<AdoptionRegisterFormDomain>().UpdateAdoptionRegisterFormStatus(model);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [Route("api/create-adoption-register-form")]
        public async Task<IActionResult> CreateUpdateAdoptionRegisterFormStatus(CreateAdoptionRegisterFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var result = _uow.GetService<AdoptionRegisterFormDomain>().CreateAdoptionRegisterForm(model);
                var firebaseExtensions = new FireBaseExtentions();
                var petDomain = _uow.GetService<PetDomain>();
                var currentPet = petDomain.GetPetById(model.PetId);
                var userDomain = _uow.GetService<UserDomain>();
                var notificationToken = userDomain.GetManagerDeviceTokenByCenterId(currentPet.CenterId);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.NEW_REGISTRATON_ADOPTION_FORM_TITLE,
                        Body = NotificationBodyHelper.NEW_REGISTRATION_ADOPTION_FORM_BODY,
                    },
                };
                message.Token = notificationToken.DeviceToken;
                await fcm.SendAsync(message);
                app.Delete();
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
