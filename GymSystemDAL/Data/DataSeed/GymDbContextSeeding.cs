using GymSystemDAL.Data.Context;
using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymSystemDAL.Data.DataSeed
{
    public class GymDbContextSeeding
    {
        public static bool SeedData(GymSystemDBContext dbContext)
        {
			try
			{
				var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if(HasCategories && HasPlans) return true;
                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFiles<Plan>("plans.json");
                    if(plans.Any())
                         dbContext.Plans.AddRange(plans);
                   
                }
                if (!HasCategories)
                {
                    var categories = LoadDataFromJsonFiles<Category>("categories.json");
                    if (categories.Any())
                        dbContext.Categories.AddRange(categories);

                }

                return dbContext.SaveChanges() > 0;
            }
			catch (Exception)
			{
				return false;
			}
        }

        private static List<T> LoadDataFromJsonFiles<T>(string FileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();
            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true

            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
        
    }
}
