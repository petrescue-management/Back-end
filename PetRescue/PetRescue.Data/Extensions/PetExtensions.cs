/*using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Extensions
{
    public static partial class PetExtensions
    {
        public static object GetData(this IQueryable<Pet> query, PetFilter filter, int page, int limit, int total, string[] fields)
        {
            query = query.Filter(filter);
            query = query.Pagination(page, limit);
            var result = query.SelectedField(fields, total);
            return result;
        }
        private static IQueryable<Pet> Filter(this IQueryable<Pet> query, PetFilter filter)
        {
            if (filter.CenterId != null && !filter.CenterId.Equals(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                query = query.Where(s => s.CenterId.Equals(filter.CenterId));
            }
            if (filter.PetId != null && !filter.PetId.Equals(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                query = query.Where(s => s.PetId.Equals(filter.PetId));
            }
            if (filter.PetStatus != 0)
            {
                query = query.Where(s => s.PetStatus == filter.PetStatus);
            }
            if (filter.PetBreedName != null)
            {
                query = query.Where(s => s.PetNavigation.PetBreed.PetBreedName.Equals(filter.PetBreedName));
            }
            if (filter.PetFurColorName != null)
            {
                query = query.Where(s => s.PetNavigation.PetFurColor.PetFurColorName.Equals(filter.PetFurColorName));
            }
            if (filter.PetTypeName != null)
            {
                query = query.Where(s => s.PetNavigation.PetBreed.PetType.PetTypeName.Equals(filter.PetTypeName));
            }
            //if (ValidationExtensions.IsNotNull(filter.IsVaccinated))
            //{
            //    query = query.Where(s => s.PetNavigation.IsVaccinated == filter.IsVaccinated);
            //}
            //if (ValidationExtensions.IsNotNull(filter.IsSterilized))
            //{
            //    query = query.Where(s => s.PetNavigation.IsSterilized == filter.IsSterilized);
            //}
            return query;
        }
        private static IQueryable<Pet> Pagination(this IQueryable<Pet> query, int page, int limit)
        {
            if (limit > -1 && page >= 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }
            return query;
        }
        private static object SelectedField(this IQueryable<Pet> query, string[] fields, int total)
        {
            var models = query.ToList();
            var listResult = new List<Dictionary<string, object>>();
            foreach (var model in models)
            {
                var obj = new Dictionary<string, object>();
                var petTypeObj = new Dictionary<string, string>();
                petTypeObj["petTypeId"] = model.PetNavigation.PetBreed.PetType.PetTypeId.ToString();
                petTypeObj["petTypeName"] = model.PetNavigation.PetBreed.PetType.PetTypeName;
                foreach (string field in fields)
                {
                    switch (field)
                    {
                        case PetFieldConst.INFO:
                            obj["petId"] = model.PetId.ToString();
                            obj["centerId"] = model.CenterId.ToString();
                            obj["petStatus"] = model.PetStatus;
                            obj["petName"] = model.PetNavigation.PetName;
                            obj["petType"] = petTypeObj;
                            obj["imageUrl"] = model.PetNavigation.ImageUrl;
                            break;
                        case PetFieldConst.DETAIL:
                            obj["petId"] = model.PetId.ToString();
                            obj["centerId"] = model.CenterId.ToString();
                            obj["petStatus"] = model.PetStatus;
                            obj["petName"] = model.PetNavigation.PetName;
                            obj["petType"] = petTypeObj;
                            obj["petGender"] = model.PetNavigation.PetGender;
                            obj["petAge"] = model.PetNavigation.PetAge;
                            obj["petBreedName"] = model.PetNavigation.PetBreed.PetBreedName;
                            obj["petFurColorName"] = model.PetNavigation.PetFurColor.PetFurColorName;
                            obj["imageUrl"] = model.PetNavigation.ImageUrl;
                            break;
                    }
                    listResult.Add(obj);
                }
            }
            var result = new Dictionary<string, object>();
            result["totalPages"] = total;
            result["result"] = listResult;
            return result;
        }

        public static IQueryable<Pet> GetAdoptionRegistrationByPet(this IQueryable<Pet> query, PetFilter filter)
        {
            query = query.Filter(filter);
            return query;
        }
    }
}
*/