using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
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
    public class PetDocumentController : BaseController
    {
        public PetDocumentController(IUnitOfWork uow) : base(uow)
        {
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("api/get-list-pet-document")]
        public IActionResult GetListPetDocument()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.GetListPetDocumentByCenterId(Guid.Parse(currentCenterId));
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("api/update-pet-document")]
        public IActionResult UpdatePetDocument([FromBody]PetDocumentUpdateModel model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<PetDocumentDomain>();
                var result = _domain.Edit(model);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
    }
}
