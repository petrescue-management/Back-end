﻿using PetRescue.Data.ConstantHelper;
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
        private readonly IWorkingHistoryRepository _workingHistoryRepo;
        private readonly RescueDocumentDomain _rescueDocumentDomain;
        private readonly IRescueDocumentRepository _rescueDocumentRepo;
        private readonly IPetProfileRepository _petProfileRepo;
        public CenterDomain(IUnitOfWork uow, ICenterRepository centerRepo, IWorkingHistoryRepository workingHistoryRepo, RescueDocumentDomain rescueDocumentDomain, IRescueDocumentRepository rescueDocumentRepo, IPetProfileRepository petProfileRepo) : base(uow)
        {
            this._centerRepo = centerRepo;
            this._workingHistoryRepo = workingHistoryRepo;
            this._rescueDocumentDomain = rescueDocumentDomain;
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
                    CountOfVolunteer = _workingHistoryRepo.Get().Where(h => h.CenterId.Equals(c.CenterId) 
                    && h.IsActive == true && h.RoleName.Equals(RoleConstant.VOLUNTEER)).Count(),
                    LastedDocuments = _rescueDocumentDomain.GetLastedRescueDocument(c.CenterId),
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
            _uow.saveChanges();
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
            _uow.saveChanges();
            return center.CenterId.ToString();
        }
        #endregion

        #region GET COUNT FOR CENTER HOMEPAGE
        public object GetCountForCenterHomePage(Guid centerId)
        {
            var records = _centerRepo.Get().AsQueryable();
            var rescues = _rescueDocumentRepo.Get()
                .Where(d => d.CenterId.Equals(centerId)).Count();

            var pets_adopted = _petProfileRepo.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.ADOPTED).Count();

            var pets_finding_owner = _petProfileRepo.Get()
                .Where(p => p.CenterId.Equals(centerId))
                .Where(p => p.PetStatus == PetStatusConst.FINDINGADOPTER).Count();

            var volunteers = _workingHistoryRepo.Get()
                .Where(u => u.CenterId.Equals(centerId) && u.IsActive == true && u.RoleName.Equals(RoleConstant.VOLUNTEER)).Count();
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
            var works = _workingHistoryRepo.Get().Where(s => s.IsActive && s.RoleName.Equals(RoleConstant.VOLUNTEER)).ToList();
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
                    var temp = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
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
                _uow.saveChanges();
                return true;
            }
            return false;
        }
    }
}
