using Microsoft.AspNetCore.Authorization;
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
    public class PickerFormController : BaseController
    {
        public PickerFormController(IUnitOfWork uow) : base(uow)
        {
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpPost]
        [Route("api/create-picker-form")]
        public IActionResult CreatePickerForm([FromBody]PickerFormCreateModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<PickerFormDomain>();
                var result = _domain.Create(model, Guid.Parse(currentUserId));
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
