using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;

namespace Qerp.ModelViews
{
    public class ParametergroupMV: Parametergroup
    {
        public static async Task<ReturnResult> GetAll()
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Parametergroup> parametergroups = await db.Parametergroups
                    .OrderBy(pg => pg.Sort)
                    .ThenBy(pg => pg.Name)
                    .ToListAsync();
                return new ReturnResult(true, "", parametergroups);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }

        }
    }
}
