using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IAdoptionRegisterFormRepository : IBaseRepository<AdoptionRegisterForm, string>
    {
    }

    public partial class AdoptionRegisterFormRepository : BaseRepository<AdoptionRegisterForm, string>, IAdoptionRegisterFormRepository
    {
        public AdoptionRegisterFormRepository(DbContext context) : base(context)
        {
        }


    }
}

