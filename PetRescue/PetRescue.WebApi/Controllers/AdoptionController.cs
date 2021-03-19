using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
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
    public class AdoptionController : BaseController
    {
        public AdoptionController(IUnitOfWork uow) : base(uow)
        {
        }

        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/search-adoption")]
        public IActionResult SearchAdoption([FromQuery] SearchModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _uow.GetService<AdoptionDomain>().SearchAdoption(model, currentCenterId);
                if (result != null)
                    return Success(result);
                return Success("Do not have any adoptions !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-adoption-by-id/{id}")]
        public IActionResult GetAdoptionById(Guid id)
        {
            try
            {
                var result = _uow.GetService<AdoptionDomain>().GetAdoptionById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPut]
        [Route("api/update-adoption-status")]
        public IActionResult UpdateAdoptionStatus(UpdateStatusModel model)
        {
            try
            {
                var result = _uow.GetService<AdoptionDomain>().UpdateAdoptionStatus(model);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
