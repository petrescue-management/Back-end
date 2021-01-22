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
        [Route("api/get-pet-breeds-by-type-id/{id}")]
        public IActionResult GetPetBreedsByTypeId(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetBreedsByTypeId(id);
                if (result != null)
                    return Success(result);
                return Success("This pet type do not have any pet breeds !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-pet_breed-by-id/{id}")]
        public IActionResult GetPetBreedById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetBreedById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        [HttpGet]
        [Route("api/get-all-pet-fur_colors")]
        public IActionResult GetAllPetFurColors()
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetAllPetFurColors();
                if (result != null)
                    return Success(result);
                return Success("Do not have any pet fur colors !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-pet-fur_color-by-id/{id}")]
        public IActionResult GetPetFurColorById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetFurColorById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-all-pet-types")]
        public IActionResult GetAllPetTypes()
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetAllPetTypes();
                if (result != null)
                    return Success(result);
                return Success("Do not have any pet types !");
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        [Route("api/get-pet-type-by-id/{id}")]
        public IActionResult GetPetTypeById(Guid id)
        {
            try
            {
                var result = _uow.GetService<PetDomain>().GetPetTypeById(id);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}
