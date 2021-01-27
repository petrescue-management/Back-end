using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        //[HttpGet]
        //[Route("api/send-message")]
       
        //[Authorize(Roles ="manager")]
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
                return Error(ex);
            }
        }
        //[Authorize(Roles ="manager")]
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
                return Error(ex);
            }
        }
        //[Authorize(Roles ="manager")]
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
                return Error(ex);
            }
        }
        //[Authorize(Roles ="manager")]
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
                return Error(ex);
            }
        }
        //[Authorize(Roles ="manager")]
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
                return Error(ex);
            }
        }
        //[Authorize(Roles ="manager")]
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
        #endregion
        #region POST
        //[Authorize(Roles =("manager"))]
        [HttpPost]
        [Route("api/create-new-pet")]
        public IActionResult CreatePet([FromBody] PetCreateModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var newPet = petDomain.CreateNewPet(model);
                if(newPet != null)
                {
                    _uow.saveChanges();
                    return Success(newPet.PetStatus);
                }
                return BadRequest();
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("sysadmin"))]
        [HttpPost]
        [Route("api/create-new-pet-breed")]
        public IActionResult CreateBreed([FromBody]PetBreedCreateModel model) 
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var newPetBreed = petDomain.CreatePetBreed(model);
                if (newPetBreed != null)
                {
                    _uow.saveChanges();
                    return Success(newPetBreed.PetBreedName);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("sysadmin"))]
        [HttpPost]
        [Route("api/create-new-pet-type")]
        public IActionResult CreateType([FromBody] PetTypeCreateModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var newPetType = petDomain.CreatePetType(model);
                if (newPetType != null)
                {
                    _uow.saveChanges();
                    return Success(newPetType.PetTypeName);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("sysadmin"))]
        [HttpPost]
        [Route("api/create-new-pet-fur-color")]
        public IActionResult CreatFurColor([FromBody] PetFurColorCreateModel model)
        {
            try 
            {
                var petDomain = _uow.GetService<PetDomain>();
                var newPetFurColor = petDomain.CreatePetFurColor(model);
                if(newPetFurColor != null)
                {
                    _uow.saveChanges();
                    return Success(newPetFurColor.PetFurColorName);
                }
                return BadRequest();
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        #region PUT
        //[Authorize(Roles =("sysadmin"))]
        [HttpPut]
        [Route("api/update-fur-color")]
        public IActionResult UpdateFurColor([FromBody] PetFurColorUpdateModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var petFurColor = petDomain.UpdatePetFurColor(model);
                if (petFurColor != null)
                {
                    _uow.saveChanges();
                    return Success(petFurColor.PetFurColorName);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("sysadmin"))]
        [HttpPut]
        [Route("api/update-breed")]
        public IActionResult UpdateBreed([FromBody] PetBreedUpdateModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var petBreed = petDomain.UpdatePetBreed(model);
                if (petBreed != null)
                {
                    _uow.saveChanges();
                    return Success(petBreed.PetBreedName);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("sysadmin"))]
        [HttpPut]
        [Route("api/update-type")]
        public IActionResult UpdateType([FromBody] PetTypeUpdateModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var petType = petDomain.UpdatePetType(model);
                if (petType != null)
                {
                    _uow.saveChanges();
                    return Success(petType.PetTypeName);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        //[Authorize(Roles =("manager"))]
        [HttpPut]
        [Route("api/update-pet")]
        public IActionResult UpdatePet([FromBody] PetDetailModel model)
        {
            try
            {
                var petDomain = _uow.GetService<PetDomain>();
                var newPet = petDomain.UpdatePet(model);
                if (newPet != null)
                {
                    _uow.saveChanges();
                    return Success(newPet.PetId);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
       
        //public class Notification1
        //{
        //    private string serverKey = "AAAA4kDCFr8:APA91bGSoL_h6JYN5CZ7bDhwYW97wU19Hsj_sCTZCUTrYLRvAxnKrVeznIt090oOu7oVymF2dSNkB3d5FRAGdVLDOcS1dK1wTbn64TJ2eYu4C34lW2C5X-CkkjUDW_W19kWnAagSNMG1";
        //    private string senderId = "971749070527";
        //    private string webAddr = "https://fcm.googleapis.com/fcm/send";

        //    public string SendNotification(string DeviceToken, string title, string msg)
        //    {
        //        var result = "-1";
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
        //        httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
        //        httpWebRequest.Method = "POST";

        //        var payload = new
        //        {
        //            to = DeviceToken,
        //            priority = "high",
        //            content_available = true,
        //            notification = new
        //            {
        //                body = msg,
        //                title = title
        //            },
        //        };
        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string json = JsonConvert.SerializeObject(payload);
        //            streamWriter.Write(json);
        //            streamWriter.Flush();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadToEnd();
        //        }
        //        return result;
        //    }
        //}

    }
}
