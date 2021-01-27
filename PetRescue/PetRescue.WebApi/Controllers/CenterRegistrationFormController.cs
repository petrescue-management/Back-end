using Microsoft.AspNetCore.Authorization;
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
    public class CenterRegistrationFormController : BaseController
    {
        public CenterRegistrationFormController(IUnitOfWork uow) : base(uow)
        {
        }

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
                return Error(ex);
            }
        }

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
                return Error(ex);
            }
        }

        [HttpPut]
        [Route("api/update-center-registration-form")]
        public IActionResult UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model)
        {
            try
            {
                _uow.GetService<CenterRegistrationFormDomain>().UpdateCenterRegistrationForm(model);
                return Success("This center registration form is updated !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [Route("api/create-center-registration-form")]
        public IActionResult CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            try
            {
                string result = _uow.GetService<CenterRegistrationFormDomain>().CreateCenterRegistrationForm(model);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        //[Authorize(Roles ="sysadmin")]
        [HttpPost]
        [Route("api/procressing-center-registration-form")]
        public IActionResult ProcressingCenterRegistrationForm(UpdateCenterRegistrationFormModel model)
        {
            try
            {
                var centerRegistrationFormDomain = _uow.GetService<CenterRegistrationFormDomain>();
                var newForm = centerRegistrationFormDomain.ProcressingCenterRegistrationForm(model);
                if(newForm!= null)
                {
                    if(newForm.CenterRegistrationStatus == CenterRegistrationFormConst.APPROVE)
                    {
                        MailArguments mailArguments = MailFormat.MailModel(newForm.Email, MailConstant.ApproveRegistrationCenter(newForm.Email));
                        bool result =  MailExtensions.Send(mailArguments, null, true, null);
                        if (result)
                        {
                            _uow.saveChanges();
                            return Success(newForm.CenterRegistrationStatus);
                        }
                        return BadRequest();
                    }
                    else if(newForm.CenterRegistrationStatus == CenterRegistrationFormConst.REJECT)
                    {
                        MailArguments mailArguments = MailFormat.MailModel(newForm.Email, MailConstant.RejectRegistrationCenter(newForm.Email));
                        bool result = MailExtensions.Send(mailArguments, null, true, null);
                        if (result)
                        {
                            _uow.saveChanges();
                            return Success(newForm.CenterRegistrationStatus);
                        }
                        return BadRequest();
                    }
                }
                return BadRequest();
                
            }catch(Exception e)
            {
                return Error(e);
            }
        }
    }
}
