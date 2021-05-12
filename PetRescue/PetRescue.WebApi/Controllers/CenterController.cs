using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Models;
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
    [Route("/api/centers/")]
    public class CenterController : BaseController
    {
        private readonly CenterDomain _centerDomain;
        public CenterController(IUnitOfWork uow, CenterDomain centerDomain) : base(uow)
        {
            this._centerDomain = centerDomain;
        }

        #region SEARCH
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet]
        [Route("search-center")]
        public IActionResult SearchCenter([FromQuery] SearchModel model)
        {
            try
            {
                var result = _centerDomain.SearchCenter(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any centers");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET BY ID
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("get-center-by-id/{id}")]
        public IActionResult GetCenterById(Guid id)
        {
            try
            {
                var result = _centerDomain.GetCenterById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("delete-center")]
        public IActionResult DeleteCenter(Guid id)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<CenterDomain>().DeleteCenter(id, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region UPDATE
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("update-center")]
        public IActionResult UpdateCenter(UpdateCenterModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _centerDomain.UpdateCenter(model, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region GET COUNT FOR CENTER HOMEPAGE
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpGet]
        [Route("get-count-for-center-home-page")]
        public IActionResult GetCountForCenterHomePage()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _centerDomain.GetCountForCenterHomePage(Guid.Parse(currentCenterId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
        [HttpGet]
        [Route("get-list-all-center")]
        public IActionResult GetListAllCenter()
        {
            try
            {
                var result = _centerDomain.GetListCenter();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("get-list-distance-centers")]
        public IActionResult GetListDistanceCenter([FromQuery] Guid finderFormId)
        {
            try
            {
                var result = _centerDomain.GetListCenterDistance(finderFormId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("change-status-of-center")]
        public IActionResult ChangeStatusOfCenter([FromBody]UpdateCenterStatus model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _centerDomain.ChangeStateOfCenter(model, Guid.Parse(currentCenterId));
                if(result)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
