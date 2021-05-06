﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Route("/api/users/")]
    public class UserController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly UserDomain _userDomain;
        public UserController(IUnitOfWork uow, IHostingEnvironment environment, UserDomain userDomain) : base(uow)
        {
            this._env = environment;
            this._userDomain = userDomain;
        }
        #region GET
        [Authorize]
        [HttpGet]
        public IActionResult GetUserDetail()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var result = _userDomain.GetUserDetail(token.ToString().Split(" ")[1]);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
        [Authorize(Roles =RoleConstant.MANAGER)]
        [HttpGet("get-list-volunteer-profile-of-center")]
        public IActionResult GetListVolunteerProfileOfCenter([FromQuery]bool IsActive)
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var result = _userDomain.GetListProfileOfVolunter(Guid.Parse(currentCenterId), IsActive);
                return Success(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet("get-list-of-member-profiles")]
        public IActionResult GetListOfMemberProfile([FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var result = _userDomain.GetListProfileMember(page, limit);
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpGet("get-profile-by-userid")]
        public IActionResult GetProfileByUserId (Guid userId)
        {
            try
            {
                var result = _userDomain.GetProfileByUserId(userId);
                if (result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        #endregion
        #region POST
        [Authorize]
        [HttpPost("update-profile")]
        public IActionResult UpdateProfileForUser([FromBody] UserProfileUpdateModel model)
        {
            try
            {
                var newUserProfile = _userDomain.UpdateUserProfile(model);
                if (newUserProfile)
                {
                    return Success(newUserProfile);
                }
                return BadRequest();
            } catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPost("create-role-volunteer-for-user")]
        public IActionResult CreateRoleForUser([FromBody] CreateVolunteerModel model)
        {
            try
            {
                var _currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _userDomain.AddVolunteerToCenter(new AddNewRoleModel
                {
                    Email = model.Email,
                    CenterId = Guid.Parse(_currentCenterId),
                    RoleName = RoleConstant.VOLUNTEER,
                    InsertBy = Guid.Parse(_currentUserId),
                    DoB = model.Dob,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    LastName = model.LastName,
                    Phone = model.Phone,
                });
                if (!result.Contains("This"))
                {
                    return Success(result);
                }
                return BadRequest(result);

            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        #region DELETE
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpDelete("remove-role-volunteer-for-user")]
        public async Task<IActionResult> RemoveRoleForUser ([FromQuery] RemoveRoleVolunteerModel model)
        {
            try
            {
                var _currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var path = _env.ContentRootPath;
                var result = await _userDomain.RemoveVolunteerOfCenter(new RemoveVolunteerRoleModel
                {
                    CenterId = Guid.Parse(_currentCenterId),
                    InsertBy = Guid.Parse(_currentUserId),
                    UserId = model.UserId,
                    Description = model.Description
                }, path);
                if (result.Equals(""))
                {
                    return Success("");
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }

}
