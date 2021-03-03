using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IAdoptionRegisterFormRepository : IBaseRepository<AdoptionRegisterForm, string>
    {
        AdoptionRegisterForm GetAdoptionRegisterFormById(Guid id);
    }

    public partial class AdoptionRegisterFormRepository : BaseRepository<AdoptionRegisterForm, string>, IAdoptionRegisterFormRepository
    {
        public AdoptionRegisterFormRepository(DbContext context) : base(context)
        {
        }

        public AdoptionRegisterForm GetAdoptionRegisterFormById(Guid id)
        {
            var form = Get().Where(f => f.AdoptionRegisterId.Equals(id))
                .Select(f => new AdoptionRegisterForm
                {
                    AdoptionRegisterId = f.AdoptionRegisterId,
                    PetId = f.PetId,
                    UserName = f.UserName,
                    Phone = f.Phone,
                    Email = f.Email,
                    Job = f.Job,
                    Address = f.Address,
                    HouseType = f.HouseType,
                    FrequencyAtHome = f.FrequencyAtHome,
                    HaveChildren = f.HaveChildren,
                    ChildAge = f.ChildAge,
                    BeViolentTendencies = f.BeViolentTendencies,
                    HaveAgreement = f.HaveAgreement,
                    HavePet = f.HavePet,
                    AdoptionRegisterStatus = f.AdoptionRegisterStatus,
                    InsertedBy = f.InsertedBy,
                    InsertedAt = f.InsertedAt,
                    UpdatedBy = f.UpdatedBy,
                    UpdateAt = f.UpdateAt
                }).FirstOrDefault();
            return form;   
        }
    }
}

