using Microsoft.AspNetCore.Hosting;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class NotificationDomain : BaseDomain
    {
        private readonly IHostingEnvironment env;
        public NotificationDomain(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            env = environment;
        }
        
    }
}
