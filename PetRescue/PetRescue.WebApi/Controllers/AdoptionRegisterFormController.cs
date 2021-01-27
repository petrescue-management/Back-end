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
    public class AdoptionRegisterFormController : BaseController
    {
        public AdoptionRegisterFormController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [Route("api/search-adoption-register-form")]
        public IActionResult SearchAdoptionRegisterForm([FromQuery] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<AdoptionRegisterFormDomain>().SearchAdoptionRegisterForm(model);
                if (result != null)
                    return Success(result);
                return Success("Do not have any adoption register forms !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
