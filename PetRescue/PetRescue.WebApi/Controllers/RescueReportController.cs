using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class RescueReportController : BaseController
    {
        public RescueReportController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [Route("api/search-rescue-report")]
        public IActionResult SearchRescueReport([FromQuery] SearchViewModel model)
        {
            try
            {
                var result = _uow.GetService<RescueReportDomain>().SearchRescueReport(model);
                if (result != null)
                    return Success(result);
                return Success("Not have any rescue report !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-rescue-report-by-id/{id}")]
        public IActionResult GetRescueReportById(Guid id)
        {
            try
            {
                var result = _uow.GetService<RescueReportDomain>().GetRescueReportById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPut]
        [Route("api/update-rescue-report")]
        public IActionResult UpdateRescueReport(UpdateRescueReportModel model)
        {
            try
            {
                _uow.GetService<RescueReportDomain>().UpdateRescueReport(model);
                return Success("This rescue report is updated !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost]
        [Route("api/create-rescue-report")]
        public IActionResult CreateRescueReport(CreateRescueReportModel model)
        {
            try
            {
                _uow.GetService<RescueReportDomain>().CreateRescueReport(model);
                return Success("This rescue report is processing !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
