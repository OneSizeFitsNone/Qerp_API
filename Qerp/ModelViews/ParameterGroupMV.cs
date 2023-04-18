using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;

namespace Qerp.ModelViews
{
    public class ParametergroupMV: Parametergroup
    {

        public static async Task<bool> IsEditable(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Parametergroup parametergroup = await db.Parametergroups.FirstOrDefaultAsync(x => x.Id == id);
                if (parametergroup != null)
                {
                    return parametergroup.CanEdit;
                }
                else { return false; }
            }
            catch {
                return false;
            }
            
        }

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
