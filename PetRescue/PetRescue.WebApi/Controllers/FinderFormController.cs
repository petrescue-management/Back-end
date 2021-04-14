using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        [HttpPost]
        [Route("api/update-finder-form-status")]
        public IActionResult UpdateFinderFormStatus(UpdateStatusModel model)
        {
            try
            {
                var path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<FinderFormDomain>().UpdateFinderFormStatusAsync(model, Guid.Parse(currentUserId), path);
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
        [Authorize]
        [HttpPost]
        [Route("api/cancel-finder-form")]
        public IActionResult CancelFinderForm([FromBody]CancelViewModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<FinderFormDomain>();
                var result = _domain.CancelFinderForm(model, Guid.Parse(currentUserId));
                if (result)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }catch(Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("api/get-list-finder-form")]
        public IActionResult GetListFinderForm()
        {
            try
            {
                var _domain = _uow.GetService<FinderFormDomain>();
                var result = _domain.GetAllListFinderForm();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("api/get-list-finder-form-by-status")]
        public IActionResult GetListFinderFormByStatus([FromQuery]int status)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<FinderFormDomain>();
                var result = _domain.GetListByStatus(Guid.Parse(currentUserId),status);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("api/get-list-finder-form-by-userid")]
        public IActionResult GetListFinderFormByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<FinderFormDomain>();
                var result = _domain.GetListByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("api/get-list-finder-form-finish-by-userid")]
        public IActionResult GetListFinderFormFinishByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var _domain = _uow.GetService<FinderFormDomain>();
                var result = _domain.GetListFinderFormFinishByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
