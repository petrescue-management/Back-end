using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class PickerFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public PickerFormController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

        #region SEARCH
        [HttpGet]
        [Route("api/search-picker-form")]
        public IActionResult SearchPickerForm([FromQuery] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<PickerFormDomain>().SearchPickerForm(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any picker form !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("api/get-picker-form-by-id/{id}")]
        public IActionResult GetPickerFormById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PickerFormDomain>().GetPickerFormById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region UPDATE STATUS
        [HttpPut]
        [Route("api/update-picker-form-status")]
        public IActionResult UpdatePickerFormStatus(UpdateStatusModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<PickerFormDomain>().UpdatePickerFormStatus(model, Guid.Parse(currentUserId));
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
        [Route("api/create-picker-form")]
        public IActionResult CreatePickerForm(CreatePickerFormModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result =  _uow.GetService<PickerFormDomain>().CreatePickerForm(model, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

    }
}
