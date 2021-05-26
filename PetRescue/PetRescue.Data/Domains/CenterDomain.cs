using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
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
        private readonly IFinderFormRepository _finderFormRepo;
        public CenterDomain(IUnitOfWork uow, 
            ICenterRepository centerRepo, 
            IRescueDocumentRepository rescueDocumentRepo, 
            IPetProfileRepository petProfileRepo,
            IFinderFormRepository finderFormRepo) : base(uow)
        {
            this._centerRepo = centerRepo;            
            this._rescueDocumentRepo = rescueDocumentRepo;
            this._petProfileRepo = petProfileRepo;
            this._finderFormRepo = finderFormRepo;
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
                .Where(p => p.PetStatus == PetStatusConst.FINDINGOWNER).Count();
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
            var centers = _centerRepo.Get().Where(s => s.CenterStatus == CenterStatusConst.OPENNING);
            var result = new List<CenterLocationModel>();
                foreach (var center in centers)
                {
                    result.Add(new CenterLocationModel
                    {
                        CenterId = center.CenterId,
                        Lat = (double)center.Lat,
                        Lng = (double)center.Lng
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
        public object GetListCenterDistance(Guid finderFormId)
        {
            var finderForm = _finderFormRepo.Get().FirstOrDefault(s => s.FinderFormId.Equals(finderFormId));
            var centers = GetListCenterLocation();
            var googleMapExtension = new GoogleMapExtensions();
            var location = finderForm.Lat + ", " + finderForm.Lng;
            var records = googleMapExtension.FindListShortestCenter(location, centers);
            var result = new List<CenterLocationViewModel>();
            if(records.Count != 0)
            {
                if(records.Count > 2)
                {
                    var center = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(records[0].CenterId));
                    result.Add(new CenterLocationViewModel
                    {
                        CenterId = records[0].CenterId,
                        CenterAddrees = center.Address,
                        CenterName = center.CenterName,
                        Phone = center.Phone,
                        Distance = Math.Round(records[0].Value / 1000, 2),
                        CenterImgUrl = center.CenterImgUrl
                    });
                    center = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(records[1].CenterId));
                    result.Add(new CenterLocationViewModel
                    {
                        CenterId = records[1].CenterId,
                        CenterAddrees = center.Address,
                        CenterName = center.CenterName,
                        Phone = center.Phone,
                        Distance = Math.Round(records[1].Value / 1000, 2),
                        CenterImgUrl = center.CenterImgUrl
                    });
                }
                else
                {
                    var center = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(records[0].CenterId));
                    result.Add(new CenterLocationViewModel
                    {
                        CenterId = records[0].CenterId,
                        CenterAddrees = center.Address,
                        CenterName = center.CenterName,
                        Phone = center.Phone,
                        Distance = Math.Round(records[0].Value / 1000, 2),
                        CenterImgUrl = center.CenterImgUrl
                    });
                }
            }
            return result;
        }
    }
}
