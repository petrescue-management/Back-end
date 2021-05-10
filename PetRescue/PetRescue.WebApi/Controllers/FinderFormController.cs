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
    [Route("/api/finder-forms/")]
    public class FinderFormController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly FinderFormDomain _finderFormDomain;
        public FinderFormController(IUnitOfWork uow, IHostingEnvironment environment, FinderFormDomain finderFormDomain) : base(uow)
        {
            _env = environment;
            this._finderFormDomain = finderFormDomain;
        }

        #region SEARCH
        [HttpGet]
        [Route("search-finder-form")]
        public IActionResult SearchFinderForm([FromQuery] SearchModel model)
        {
            try
            {
                var result = _finderFormDomain.SearchFinderForm(model);
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
        [Route("get-finder-form-by-id/{id}")]
        public IActionResult GetFinderFormById(Guid id)
        {
            try
            {
                var result = _finderFormDomain.GetFinderFormById(id);
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
        [Route("update-finder-form-status")]
        public async Task<IActionResult> UpdateFinderFormStatus(UpdateStatusModel model)
        {
            try
            {
                var path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = await _finderFormDomain.UpdateFinderFormStatusAsync(model, Guid.Parse(currentUserId), path);
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region CREATE
        [Authorize]
        [HttpPost]
        [Route("create-finder-form")]
        public async Task<IActionResult> CreateFinderForm(CreateFinderFormModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = await _finderFormDomain.CreateFinderForm(model, Guid.Parse(currentUserId), path);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
        [Authorize]
        [HttpPut]
        [Route("cancel-finder-form")]
        public async Task<IActionResult> CancelFinderForm([FromBody]CancelViewModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var currentRole = HttpContext.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).Select(s=>s.Value).ToList();
                var path = _env.ContentRootPath;
                var result = await _finderFormDomain.CancelFinderForm(model, Guid.Parse(currentUserId), currentRole, path);
                if (result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }catch(Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("get-list-finder-form")]
        public IActionResult GetListFinderForm()
        {
            try
            {
                var result = _finderFormDomain.GetAllListFinderForm();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("get-list-finder-form-by-status")]
        public IActionResult GetListFinderFormByStatus([FromQuery]int status)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _finderFormDomain.GetListByStatus(Guid.Parse(currentUserId),status);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("get-list-finder-form-by-userid")]
        public IActionResult GetListFinderFormByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _finderFormDomain.GetListByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize(Roles = RoleConstant.VOLUNTEER)]
        [HttpGet]
        [Route("get-list-finder-form-finish-by-userid")]
        public IActionResult GetListFinderFormFinishByUserId()
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _finderFormDomain.GetListFinderFormFinishByUserId(Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
