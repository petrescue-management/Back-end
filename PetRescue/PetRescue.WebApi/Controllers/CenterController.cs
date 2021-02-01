using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Models;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return Error(ex);
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
                return Error(ex);
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
                var result = _uow.GetService<CenterDomain>().DeleteCenter(id);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
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
                var result = _uow.GetService<CenterDomain>().UpdateCenter(model);
                _uow.saveChanges();
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion
    }
}
