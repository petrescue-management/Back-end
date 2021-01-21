using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class PetController : BaseController
    {
        public PetController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [Route("api/get-breeds-by-type")]
        public IActionResult GetBreedsByType([FromQuery] Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetBreedsByType(id);
                if (result != null)
                    return Success(result);
                return Success("This type do not have any breeds !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-breed-by-id")]
        public IActionResult GetBreedById([FromQuery] Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetBreedById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
