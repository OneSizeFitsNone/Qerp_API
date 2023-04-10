using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class ProvinceMV : Province
    {

        public static async Task<ReturnResult> SelectAll(long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<ProvinceMV> lstProvincesMV = ObjectManipulation.CastObject<List<ProvinceMV>>(
                    await db.Provinces
                        .Where(p => p.CompanyId == -1 || p.CompanyId == companyId)
                        .OrderBy(p => p.Name)
                        .ToListAsync()
                );

                return new ReturnResult(true, "", lstProvincesMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectByCountry(long companyId, long countryId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<ProvinceMV> lstProvincesMV = ObjectManipulation.CastObject<List<ProvinceMV>>(
                    await db.Provinces
                        .Where(p => 
                            (p.CompanyId == -1 || p.CompanyId == companyId) && 
                            p.CountryId == countryId
                         )
                        .OrderBy(p => p.Name)
                        .ToListAsync()
                );

                return new ReturnResult(true, "", lstProvincesMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

    }

}
