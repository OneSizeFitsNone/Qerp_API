using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{

    public class CityMV : City
    {
        public string CodeAndName
        {
            get
            {
                return this.PostalCode + " " + this.Name;
            }
        }

        public static async Task<ReturnResult> SelectAll(long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<CityMV> lstCityMV = ObjectManipulation.CastObject<List<CityMV>>(
                    await db.Cities
                        .Where(c => c.CompanyId == -1 || c.CompanyId == companyId)
                        .OrderBy(c => c.Name)
                        .ToListAsync()
                );

                return new ReturnResult(true, "", lstCityMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectByProvince(long companyId, long provinceId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<CityMV> lstCityMV = ObjectManipulation.CastObject<List<CityMV>>(
                    await db.Cities
                        .Where(c =>
                            (c.CompanyId == -1 || c.CompanyId == companyId) &&
                            c.ProvinceId == provinceId
                         )
                        .OrderBy(p => p.Name)
                        .ToListAsync()
                );

                return new ReturnResult(true, "", lstCityMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }
    }
}
