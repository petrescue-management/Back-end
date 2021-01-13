using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
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

        [HttpPost]
        [Route("api/SearchCenterRegistrationForm")]
        public IActionResult SearchCenterRegistrationForm(SearchViewModel model)
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

        [HttpPost]
        [Route("api/GetCenterRegistrationFormById")]
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
    }
}
