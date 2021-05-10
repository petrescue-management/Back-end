using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public static partial class PetProfileExtensions
    {
        public static object GetData(this IQueryable<PetProfile> query, PetProfileFilter filter, int page, int limit, string[] fields)
        {
            query = query.Filter(filter);
            var result = query.SelectedField(fields, page, limit);
            return result;
        }
        private static IQueryable<PetProfile> Filter(this IQueryable<PetProfile> query, PetProfileFilter filter)
        {
            if (filter.CenterId != null && !filter.CenterId.Equals(Guid.Empty))
            {
                query = query.Where(s => s.CenterId.Equals(filter.CenterId));
            }
            if (filter.PetDocumentId != null && !filter.PetDocumentId.Equals(Guid.Empty))
            {
                query = query.Where(s => s.RescueDocumentId.Equals(filter.PetDocumentId));
            }
            if (filter.PetStatus != 0)
            {
                query = query.Where(s => s.PetStatus == filter.PetStatus);
            }
            if (filter.PetBreedName != null)
            {
                query = query.Where(s => s.PetBreed.PetBreedName.Equals(filter.PetBreedName));
            }
            if (filter.PetFurColorName != null)
            {
                query = query.Where(s => s.PetFurColor.PetFurColorName.Equals(filter.PetFurColorName));
            }
            if (filter.PetTypeName != null)
            {
                query = query.Where(s => s.PetBreed.PetType.PetTypeName.Equals(filter.PetTypeName));
            }
            return query.OrderByDescending(s => s.InsertedAt);
        }
        private static object SelectedField(this IQueryable<PetProfile> query, string[] fields, int page, int limit)
        {
            var total = 0;
            if (limit > -1)
            {
                total = query.Count() / limit;
            }
            if (limit > -1 && page >= 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }
            var models = query.ToList();
            var listResult = new List<Dictionary<string, object>>();
            
            foreach (var model in models)
            {
                var obj = new Dictionary<string, object>();
                var petTypeObj = new Dictionary<string, string>()
                {
                    ["petTypeId"] = model.PetBreed.PetType.PetTypeId.ToString(),
                    ["petTypeName"] = model.PetBreed.PetType.PetTypeName
                };
                foreach (string field in fields)
                {
                    switch (field)
                    {
                        case PetFieldConst.INFO:
                            obj["petId"] = model.PetProfileId.ToString();
                            obj["centerId"] = model.CenterId.ToString();
                            obj["petStatus"] = model.PetStatus;
                            obj["petName"] = model.PetName;
                            obj["petType"] = petTypeObj;
                            obj["imageUrl"] = model.PetImgUrl;
                            obj["insertAt"] = model.InsertedAt;
                            break;
                        case PetFieldConst.DETAIL:
                            obj["petId"] = model.PetProfileId.ToString();
                            obj["centerId"] = model.CenterId.ToString();
                            obj["petStatus"] = model.PetStatus;
                            obj["petName"] = model.PetName;
                            obj["petType"] = petTypeObj;
                            obj["petGender"] = model.PetGender;
                            obj["petAge"] = model.PetAge;
                            obj["petBreedName"] = model.PetBreed.PetBreedName;
                            obj["petFurColorName"] = model.PetFurColor.PetFurColorName;
                            obj["imageUrl"] = model.PetImgUrl;
                            obj["insertAt"] = model.InsertedAt;
                            break;
                    }
                    listResult.Add(obj);
                }
            }
            var result = new Dictionary<string, object>()
            {
                ["totalPages"] = total,
                ["result"] = listResult
            };
            return result;
        }
        public static IQueryable<PetProfile> GetAdoptionRegistrationByPet(this IQueryable<PetProfile> query, PetProfileFilter filter)
        {
            query = query.Filter(filter);
            return query;
        }
    }

}
