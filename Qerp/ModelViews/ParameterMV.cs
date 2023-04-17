using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Qerp.ModelViews
{
    public class ParameterMV: Parameter
    {
        public ParameterMV() { }

        public static async Task<ReturnResult> GetById(long companyId, long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Parameter? parameter = await db.Parameters.FirstOrDefaultAsync(p => p.Id == id && (p.CompanyId == -1 || p.CompanyId == companyId));
                return new ReturnResult(true, "", parameter);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> GetByGroupId(long companyId, long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Parameter> parameters = await db.Parameters
                    .Where(p =>
                        (p.CompanyId == -1 || p.CompanyId == companyId) &&
                        p.GroupId == id                        
                    )
                    .OrderBy(p => p.Name)
                    .ToListAsync();
                return new ReturnResult(true, "", parameters);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public async Task<ReturnResult> Insert()
        {
            try
            {
                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();
                ReturnResult result = await ParameterMV.GetById(this.CompanyId, this.Id);
                return new ReturnResult(true, "", result.Object);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);

            }
        }

        public async Task<ReturnResult> Update()
        {
            try
            {
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ReturnResult result = await ParameterMV.GetById(this.CompanyId, this.Id);
                return new ReturnResult(true, "", result.Object);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);

            }
        }

        public async Task<ReturnResult> Delete()
        {
            try
            {
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return new ReturnResult(true, "", this);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }


    }
}
