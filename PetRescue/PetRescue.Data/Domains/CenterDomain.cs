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
        private readonly ICenterRepository _centerRepo;
        private readonly IRescueDocumentRepository _rescueDocumentRepo;
        private readonly IPetProfileRepository _petProfileRepo;
        public CenterDomain(IUnitOfWork uow, 
            ICenterRepository centerRepo, 
            IRescueDocumentRepository rescueDocumentRepo, 
            IPetProfileRepository petProfileRepo) : base(uow)
        {
            this._centerRepo = centerRepo;            
            this._rescueDocumentRepo = rescueDocumentRepo;
            this._petProfileRepo = petProfileRepo;
        }

        #region SEARCH
        public SearchReturnModel SearchCenter(SearchModel model)
        {
            var records = _centerRepo.Get().AsQueryable();

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
                    LastedDocuments = _uow.GetService<RescueDocumentDomain>().GetLastedRescueDocument(c.CenterId),
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
            var center = _centerRepo.GetCenterById(id);
            return center;
        }
        #endregion

        #region DELETE
        public CenterModel DeleteCenter(Guid id, Guid updateBy)
        {
            var center = _centerRepo.DeleteCenter(id, updateBy);
            _uow.SaveChanges();
            return center;
        }
        #endregion

        #region CREATE
        public CenterModel CreateCenter(CreateCenterModel model, Guid insertBy)
        {
            var center = _centerRepo.CreateCenter(model, insertBy);
            return center;
        }
        #endregion

        #region UPDATE
        public string UpdateCenter(UpdateCenterModel model, Guid updatedBy)
        {
            //call CenterService
            var center = _centerRepo.UpdateCenter(model, updatedBy);
            _uow.SaveChanges();
            return center.CenterId.ToString();
        }
        #endregion

        #region GET COUNT FOR CENTER HOMEPAGE
        public object GetCountForCenterHomePage(Guid centerId)
        {
            var records = _centerRepo.Get().AsQueryable();
            int rescues = _rescueDocumentRepo.Get()
                .Where(d => d.CenterId.Equals(centerId)).Count();

            int pets_adopted = _petProfileRepo.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.ADOPTED).Count();

            int pets_finding_owner = _petProfileRepo.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.FINDINGADOPTER).Count();
            return new
            {
                rescues = rescues,
                petsAdopted = pets_adopted,
                petsFindingOwner = pets_finding_owner,
            };
        }
        #endregion
        public List<CenterLocationModel> GetListCenterLocation()
        {
            var setCenterId = new HashSet<Guid>();
            var result = new List<CenterLocationModel>();
                foreach (var centerId in setCenterId.ToList())
                {
                    var temp = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId) && s.CenterStatus == CenterStatusConst.OPENNING);
                    result.Add(new CenterLocationModel
                    {
                        CenterId = temp.CenterId.ToString(),
                        Lat = (double)temp.Lat,
                        Lng = (double)temp.Lng
                    });
                }
            return result;
        }
        public List<CenterViewModel> GetListCenter()
        {
            var result = new List<CenterViewModel>();
            var centers = _centerRepo.Get().Where(s=>s.CenterStatus == CenterStatusConst.OPENNING).ToList();
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
        public bool ChangeStateOfCenter(UpdateCenterStatus model, Guid centerId)
        {
            var center = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
            center.CenterStatus = model.Status;
            var result = _centerRepo.Update(center).Entity;
            if(result != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
