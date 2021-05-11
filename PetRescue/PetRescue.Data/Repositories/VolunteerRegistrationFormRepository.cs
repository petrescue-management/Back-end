using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IVolunteerRegistrationFormRepository : IBaseRepository<VolunteerRegistrationForm, string>
    {
        VolunteerRegistrationForm Create(VolunteerRegistrationFormCreateModel model);
        VolunteerRegistrationForm Edit(VolunteerRegistrationForm entity, VolunteerRegistrationFormUpdateModel model);
    }
    public partial class VolunteerRegistrationFormRepository : BaseRepository<VolunteerRegistrationForm, string>, IVolunteerRegistrationFormRepository
    {
        public VolunteerRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        public VolunteerRegistrationForm Create(VolunteerRegistrationFormCreateModel model)
        {
            var form = PrepareCreate(model);
            return Create(form).Entity;
        }
        private VolunteerRegistrationForm PrepareCreate(VolunteerRegistrationFormCreateModel model)
        {
            var form = new VolunteerRegistrationForm
            {
                Dob = model.Dob,
                Email = model.Email,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                Phone = model.Phone,
                VolunteerRegistrationFormStatus = VolunteerRegistrationFormConst.PROCESSING,
                VolunteerRegistrationFormId = Guid.NewGuid(),
                InsertedAt = DateTime.UtcNow,
                VolunteerRegistrationFormImgUrl= model.ImageUrl,
            };
            return form;
        }

        public VolunteerRegistrationForm Edit(VolunteerRegistrationForm entity, VolunteerRegistrationFormUpdateModel model)
        {
            var form = PrepareEdit(entity, model);
            return Update(form).Entity;
        }
        private VolunteerRegistrationForm PrepareEdit(VolunteerRegistrationForm entity, VolunteerRegistrationFormUpdateModel model)
        {
            //entity.Status = model.Status;
            return entity;
        }
    }
}
