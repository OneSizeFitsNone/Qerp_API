using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class CountryMV : Country
    {
        public static async Task<ReturnResult> SelectAll()
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<CountryMV> lstCountriesMV = ObjectManipulation.CastObject<List<CountryMV>>(
                    await db.Countries.OrderBy(c => c.Nicename).ToListAsync()
                );

                return new ReturnResult(true, "", lstCountriesMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }
    }
}
