using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IAdoptionRegistrationFormRepository : IBaseRepository<AdoptionRegistrationForm, string>
    {
        AdoptionRegistrationForm GetAdoptionRegistrationFormById(Guid id);

        AdoptionRegistrationForm UpdateAdoptionRegistrationFormStatus(UpdateStatusModel model, Guid updateBy);

        AdoptionRegistrationForm CreateAdoptionRegistrationForm(CreateAdoptionRegistrationFormModel model, Guid insertBy);

    }

    public partial class AdoptionRegistrationFormRepository : BaseRepository<AdoptionRegistrationForm, string>, IAdoptionRegistrationFormRepository
    {
        public AdoptionRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        #region GET BY ID
        public AdoptionRegistrationForm GetAdoptionRegistrationFormById(Guid id)
        {
            var form = Get().Where(f => f.AdoptionRegistrationId.Equals(id))
                .Select(f => new AdoptionRegistrationForm
                {
                    AdoptionRegistrationId = f.AdoptionRegistrationId,
                    PetDocumentId = f.PetDocumentId,
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
                    AdoptionRegistrationStatus = f.AdoptionRegistrationStatus,
                    InsertedBy = f.InsertedBy,
                    InsertedAt = f.InsertedAt,
                    UpdatedBy = f.UpdatedBy,
                    UpdatedAt = f.UpdatedAt
                }).FirstOrDefault();
            return form;   
        }
        #endregion


        #region UPDATE STATUS
        private AdoptionRegistrationForm PrepareUpdate(UpdateStatusModel model, Guid updateBy)
        {
            var form = Get()
                  .Where(f=> f.AdoptionRegistrationId.Equals(model.Id))
                  .Select(f => new AdoptionRegistrationForm
                  {
                      AdoptionRegistrationId = f.AdoptionRegistrationId,
                      PetDocumentId = f.PetDocumentId,
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
                      AdoptionRegistrationStatus = model.Status,
                      InsertedBy = f.InsertedBy,
                      InsertedAt = f.InsertedAt,
                      UpdatedBy = updateBy,
                      UpdatedAt = DateTime.UtcNow
                  }).FirstOrDefault();
            return form;
        }

        public AdoptionRegistrationForm UpdateAdoptionRegistrationFormStatus(UpdateStatusModel model, Guid updateBy)
        {
            var form = PrepareUpdate(model, updateBy);
            Update(form);
            return form;
        }

        #endregion

        #region CREATE

        private AdoptionRegistrationForm PrepareCreate(CreateAdoptionRegistrationFormModel model,Guid insertBy)
        {

            var form = new AdoptionRegistrationForm
            {
                AdoptionRegistrationId = Guid.NewGuid(),
                PetDocumentId = model.PetDocumentId,
                UserName = model.UserName,
                Phone = model.Phone,
                Email = model.Email,
                Job = model.Job,
                Address = model.Address,
                HouseType = model.HouseType,
                FrequencyAtHome = model.FrequencyAtHome,
                HaveChildren = model.HaveChildren,
                ChildAge = model.ChildAge,
                BeViolentTendencies = model.BeViolentTendencies,
                HaveAgreement = model.HaveAgreement,
                HavePet = model.HavePet,
                AdoptionRegistrationStatus = model.AdoptionRegistrationStatus,
                InsertedBy = insertBy,
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null
            };
            return form;
        }

        public AdoptionRegistrationForm CreateAdoptionRegistrationForm(CreateAdoptionRegistrationFormModel model,Guid insertBy)
        {
            var form = PrepareCreate(model, insertBy);

            Create(form);

            return form;
        }
        #endregion



    }
}

