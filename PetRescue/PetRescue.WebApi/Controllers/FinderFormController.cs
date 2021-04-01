using Microsoft.AspNetCore.Hosting;
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
    public class FinderFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        public FinderFormController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }

        #region SEARCH
        [HttpGet]
        [Route("api/search-finder-form")]
        public IActionResult SearchFinderForm([FromQuery] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<FinderFormDomain>().SearchFinderForm(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any finder form !");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("api/get-finder-form-by-id/{id}")]
        public IActionResult GetFinderFormById(Guid id)
        {
            try
            {
                var result = _uow.GetService<FinderFormDomain>().GetFinderFormById(id);
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
        [Route("api/update-finder-form-status")]
        public IActionResult UpdateFinderFormStatus(UpdateStatusModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<FinderFormDomain>().UpdateFinderFormStatus(model, Guid.Parse(currentUserId));
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
        [Route("api/create-finder-form")]
        public IActionResult CreateFinderForm(CreateFinderFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result =  _uow.GetService<FinderFormDomain>().CreateFinderForm(model, Guid.Parse(currentUserId), path);

                
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
