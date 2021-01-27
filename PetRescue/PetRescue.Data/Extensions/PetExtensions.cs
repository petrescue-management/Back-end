using PetRescue.Data.Models;
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
            var result = query.SelectedField(fields,total);
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
            }if(filter.PetFurColorName != null)
            {
                query = query.Where(s => s.PetNavigation.PetFurColor.PetFurColorName.Equals(filter.PetFurColorName));
            }if(filter.PetTypeName != null)
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
            var listResult = new List<Dictionary<string, string>>();
            foreach (var model in models)
            {
                var obj = new Dictionary<string, string>();
                foreach(string field in fields)
                {
                    switch (field)
                    {
                        case PetFieldConst.INFO:
                            obj["pet_id"] = model.PetId.ToString();
                            obj["center_id"] = model.CenterId.ToString();
                            obj["pet_status"] = model.PetStatus.ToString();
                            obj["pet_name"] = model.PetNavigation.PetName;
                            obj["pet_type_name"] = model.PetNavigation.PetBreed.PetType.PetTypeName;
                            break;
                        case PetFieldConst.DETAIL:
                            obj["pet_id"] = model.PetId.ToString();
                            obj["center_id"] = model.CenterId.ToString();
                            obj["pet_status"] = model.PetStatus.ToString();
                            obj["pet_name"] = model.PetNavigation.PetName;
                            obj["pet_type_name"] = model.PetNavigation.PetBreed.PetType.PetTypeName;
                            obj["pet_gender"] = model.PetNavigation.PetGender.ToString();
                            obj["pet_age"] = model.PetNavigation.PetAge.ToString();
                            obj["weight"] = model.PetNavigation.Weight.ToString();
                            obj["is_vaccinated"] = model.PetNavigation.IsVaccinated.ToString();
                            obj["is_sterilized"] = model.PetNavigation.IsSterilized.ToString();
                            obj["pet_breed_name"] = model.PetNavigation.PetBreed.PetBreedName;
                            obj["pet_fur_color_name"] = model.PetNavigation.PetFurColor.PetFurColorName;
                            break;
                    }
                    listResult.Add(obj);
                }
            }
            var result = new Dictionary<string, object>();
            result["result"] = listResult;
            result["total_page"] = total;
            return result;
        }
    }
}
