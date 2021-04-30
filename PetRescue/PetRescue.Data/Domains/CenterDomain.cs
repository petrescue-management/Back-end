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

            var countOfVolunteer = uow.GetService<IWorkingHistoryRepository>();

            var documentDomain = uow.GetService<RescueDocumentDomain>();

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
                    CenterImageUrl = c.CenterImgUrl,
                    InsertedAt = c.InsertedAt,
                    UpdatedAt = c.UpdatedAt,
                    CountOfVolunteer = countOfVolunteer.Get().Where(h => h.CenterId.Equals(c.CenterId) && h.IsActive == true).Count(),
                    LastedDocuments = documentDomain.GetLastedRescueDocument(c.CenterId),
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
        public string UpdateCenter(UpdateCenterModel model, Guid updatedBy)
        {
            //call CenterService
            var center_service = uow.GetService<ICenterRepository>();
            var center = center_service.UpdateCenter(model, updatedBy);
            uow.saveChanges();
            return center.CenterId.ToString();
        }
        #endregion

        #region GET COUNT FOR CENTER HOMEPAGE
        public object GetCountForCenterHomePage(Guid centerId)
        {
            var records = uow.GetService<ICenterRepository>().Get().AsQueryable();
            var rescues = uow.GetService<IRescueDocumentRepository>().Get()
                .Where(d => d.CenterId.Equals(centerId)).Count();

            var petProfileService = uow.GetService<IPetProfileRepository>();

            var pets_adopted = petProfileService.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.ADOPTED).Count();

            var pets_finding_owner = petProfileService.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.FINDINGADOPTER).Count();

            var volunteers = uow.GetService<IWorkingHistoryRepository>().Get().Where(u => u.CenterId.Equals(centerId)).Count();

            return new
            {
                rescues = rescues,
                petsAdopted = pets_adopted,
                petsFindingOwner = pets_finding_owner,
                volunteers = volunteers
            };
        }
        #endregion
        public List<CenterLocationModel> GetListCenterLocation()
        {
            var centerRepo = uow.GetService<ICenterRepository>();
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var workingHistoryRepo = uow.GetService<IWorkingHistoryRepository>();
            var works = workingHistoryRepo.Get().Where(s => s.IsActive && s.RoleName.Equals(RoleConstant.VOLUNTEER)).ToList();
            var setCenterId = new HashSet<Guid>();
            foreach (var work in works)
            {
                setCenterId.Add((Guid)work.CenterId);
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
        public List<CenterViewModel> GetListCenter()
        {
            var centerRepo = uow.GetService<ICenterRepository>();
            var result = new List<CenterViewModel>();
            var centers = centerRepo.Get().Where(s=>s.CenterStatus == CenterStatusConst.OPENNING).ToList();
            foreach(var center in centers)
            {
                result.Add(new CenterViewModel 
                {
                    Address = center.Address,
                    CenterId = center.CenterId,
                    CenterName = center.CenterName,
                    CenterStatus = center.CenterStatus,
                    ImageUrl = center.CenterImgUrl,
                    Email = center.CenterNavigation.Email,
                    Phone = center.CenterNavigation.Phone
                });
            }
            return result;
        }
        public int ChangeStateOfCenter(UpdateCenterStatus model, Guid centerId)
        {
            var centerRepo = uow.GetService<ICenterRepository>();
            var center = centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
            center.CenterStatus = model.Status;
            var result = centerRepo.Update(center).Entity;
            if(result != null)
            {
                return 1;
            }
            return 0;
        }
    }
}
