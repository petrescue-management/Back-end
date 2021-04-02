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
    public class CenterController : BaseController
    {
        public CenterController(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        [HttpGet]
        [Route("api/search-center")]
        public IActionResult SearchCenter([FromQuery] SearchModel model)
        {
            try
            {
                var result = _uow.GetService<CenterDomain>().SearchCenter(model);
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
        [HttpGet]
        [Route("api/get-center-by-id/{id}")]
        public IActionResult GetCenterById(Guid id)
        {
            try
            {
                var result = _uow.GetService<CenterDomain>().GetCenterById(id);
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
        [Route("api/delete-center")]
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
        [HttpPut]
        [Route("api/update-center")]
        public IActionResult UpdateCenter(UpdateCenterModel model)
        {
            try
            {
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _uow.GetService<CenterDomain>().UpdateCenter(model, Guid.Parse(currentUserId));
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
        //[Authorize(Roles =RoleConstant.ADMIN)]
        [HttpGet]
        [Route("api/get-statistic-about-center")]
        public IActionResult GetStatisticAboutCenter([FromQuery] Guid centerId)
        {
            try
            {
                var _domain = _uow.GetService<CenterDomain>();
                //var 
                //var result = _domain.GetStatisticAboutCenter(centerId);
                var result = "";
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/get-list-all-center")]
        public IActionResult GetListAllCenter()
        {
            try
            {
                var _domain = _uow.GetService<CenterDomain>();
                var result = _domain.GetListCenter();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPut]
        [Route("api/change-status-of-center")]
        public IActionResult ChangeStatusOfCenter([FromBody]UpdateCenterStatus model)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<CenterDomain>();
                var result = _domain.ChangeStateOfCenter(model, Guid.Parse(currentCenterId));
                if(result == 1)
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
