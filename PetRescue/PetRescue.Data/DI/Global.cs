using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetRescue.Data.Domains;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using test2;

namespace PetRescue.Data.DI
{
    public static partial class G
    {
        public static IMapper Mapper{ get; private set;}
        private static List<Action<IMapperConfigurationExpression>> MapperConfigs = 
            new List<Action<IMapperConfigurationExpression>>();
        public static void ConfigureIoC(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<DbContext, PetRescueContext>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<UserDomain>()
                .AddScoped<JWTDomain>()
                .AddScoped<ICenterRepository, CenterRepository>()
                .AddScoped<CenterDomain>()
                .AddScoped<ICenterRegistrationFormRepository, CenterRegistrationFormRepository>()
                .AddScoped<CenterRegistrationFormDomain>()
                .AddScoped<IUserRoleRepository, UserRoleRepository>()
                .AddScoped<UserRoleDomain>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUserProfileRepository, UserProfileRepository>()
                .AddScoped<IPetBreedRepository, PetBreedRepository>()
                .AddScoped<IPetFurColorRepository, PetFurColorRepository>()
                .AddScoped<IPetTypeRepository, PetTypeRepository>()
                .AddScoped<IPetProfileRepository, PetProfileRepository>()
                .AddScoped<IAdoptionRepository, AdoptionRepository>()
                .AddScoped<AdoptionDomain>()
                .AddScoped<IAdoptionRegistrationFormRepository, AdoptionRegistrationFormRepository>()
                .AddScoped<AdoptionRegistrationFormDomain>()
                .AddScoped<INotificationTokenRepository, NotificationTokenRepository>()
                .AddScoped<NotificationTokenDomain>()
                .AddScoped<IFinderFormRepository, FinderFormRepository>()
                       .AddScoped<FinderFormDomain>()
                .AddScoped<IPickerFormRepository, PickerFormRepository>()
                       .AddScoped<PickerFormDomain>()
                .AddScoped<IVolunteerRegistrationFormRepository, VolunteerRegistrationFormRepository>()
                .AddScoped<VolunteerRegistrationFormDomain>()
                .AddScoped<PetProfileDomain>()
                .AddScoped<IPetTrackingRepository, PetTrackingRepository>()
                .AddScoped<PetTrackingDomain>()
                .AddScoped<IPetDocumentRepository, PetDocumentRepository>()
                .AddScoped<PetDocumentDomain>()
                .AddScoped<IMyScopedService, MyScopedService>()
            .AddScoped<ConfigDomain>();
            ;
        }
       
    }
}
