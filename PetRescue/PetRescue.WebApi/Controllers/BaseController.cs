using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IUnitOfWork _uow;

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        protected IActionResult Error<T>(T obj)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, obj);
        }
        protected IActionResult Success<T>(T obj)
        {
            return StatusCode((int)HttpStatusCode.OK, obj);
        }
    }
}
