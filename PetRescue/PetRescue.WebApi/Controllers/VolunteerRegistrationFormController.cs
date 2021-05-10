using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
using PetRescue.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.ViewModels
{
    [ApiController]
    [Route("/api/volunteer-registration-forms/")]
    public class VolunteerRegistrationFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly VolunteerRegistrationFormDomain _volunteerRegistrationFormDomain;
        public VolunteerRegistrationFormController(IUnitOfWork uow, IHostingEnvironment environment, VolunteerRegistrationFormDomain volunteerRegistrationFormDomain) : base(uow)
        {
            this._env = environment;
            this._volunteerRegistrationFormDomain = volunteerRegistrationFormDomain;
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("get-list-volunteer-registration-form")]
        public IActionResult GetListVolunteerRegistrationForm()
        {
            try
            {
                var result = _volunteerRegistrationFormDomain.GetListVolunteerRegistrationForm();
                return Success(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [Route("create-volunteer-registration-form")]
        public async Task<IActionResult> CreateVolunteerRegistrationForm([FromBody]VolunteerRegistrationFormCreateModel model)
        {
            try
            {
                var path = _env.ContentRootPath;
                var _domain = _uow.GetService<VolunteerRegistrationFormDomain>();
                var result = await _domain.Create(model, path);
                if(!result.Contains("This"))
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex) 
            {
                return Error(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("progressing-volunteer-registration-form")]
        public IActionResult ProgressingVolunteerRegistrationForm([FromBody] VolunteerRegistrationFormUpdateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _volunteerRegistrationFormDomain.Edit(model, Guid.Parse(currentUserId));

                if (result.Contains("This"))
                {
                    return BadRequest(result);
                }else if (result.Equals("reject"))
                {
                    return Success(result);
                }
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

    }
}
