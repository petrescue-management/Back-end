using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class CenterDomain : BaseDomain
    {
        public CenterDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchCenter(SearchModel model)
        {
            var records = uow.GetService<ICenterRepository>().Get().AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                records = records.Where(c => c.CenterName.Contains(model.Keyword));


            if (model.Status != 0)
                records = records.Where(c => c.CenterStatus.Equals(model.Status));

            List<CenterModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(c => new CenterModel
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    Lat = c.Lat,
                    Long = c.Lng,
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertAt = c.InsertAt,
                    UpdateAt = c.UpdateAt,
                    
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public CenterModel GetCenterById(Guid id)
        {
            var center = uow.GetService<ICenterRepository>().GetCenterById(id);
            return center;
        }
        #endregion

        #region DELETE
        public CenterModel DeleteCenter(Guid id, Guid updateBy)
        {
            var center = uow.GetService<ICenterRepository>().DeleteCenter(id, updateBy);
            uow.saveChanges();
            return center;
        }
        #endregion

        #region CREATE
        public CenterModel CreateCenter(CreateCenterModel model, Guid insertBy)
        {
            var center = uow.GetService<ICenterRepository>().CreateCenter(model, insertBy);
            return center;
        }
        #endregion

        #region UPDATE
        public string UpdateCenter(UpdateCenterModel model, Guid currentUserId)
        {
            //call CenterService
            var center_service = uow.GetService<ICenterRepository>();

            //check Duplicate  phone
            var check_dup_phone = center_service.Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate address
            var check_dup_address = center_service.Get()
               .Where(c => c.Address.Equals(model.Address));

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";

            var center = center_service.UpdateCenter(model, currentUserId);
            uow.saveChanges();
            return center.CenterId.ToString();
        }

        public CenterStatistic GetStatisticAboutCenter(Guid centerId)
        {
            var centerRepo = uow.GetService<ICenterRepository>();
            var rescueRepo = uow.GetService<IRescueReportRepository>();
            var petRepo = uow.GetService<IPetRepository>();
            var result = new CenterStatistic();
            result.RescueCase = rescueRepo.Get().Where(s => s.CenterId.Equals(centerId)).Count();
            result.PetFindTheOwner = petRepo.Get().Where(s => s.PetStatus == PetStatusConst.FINDINGADOPTER).Count();
            result.PetAdoption = petRepo.Get().Where(s => s.PetStatus == PetStatusConst.ADOPTED).Count();
            return result;
        }
        #endregion
        public List<CenterLocationModel> GetListCenterLocation()
        {
            var centerRepo = uow.GetService<ICenterRepository>();
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var userRoles = userRoleRepo.Get().Where(s => (bool)s.User.IsBelongToCenter && s.IsActive && s.Role.RoleName.Equals(RoleConstant.VOLUNTEER)).ToList();
            var setCenterId = new HashSet<Guid>();
            foreach (var userRole in userRoles)
            {
                setCenterId.Add((Guid)userRole.User.CenterId);
            }
            var result = new List<CenterLocationModel>();
            if (setCenterId.Count != 0)
            {
                foreach (var centerId in setCenterId.ToList())
                {
                    var temp = centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
                    result.Add(new CenterLocationModel
                    {
                        CenterId = temp.CenterId.ToString(),
                        Lat = (double)temp.Lat,
                        Lng = (double)temp.Lng
                    });
                }
            }
            return result;
        }
    }
}
