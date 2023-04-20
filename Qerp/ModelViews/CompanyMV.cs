using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class CompanyMV : Company
    {
        public static async Task<ReturnResult> SelectById(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Company oCompanyMV = await db.Companies
                    .Include(c => c.City)
                    .ThenInclude(cc => cc.Province)
                    .ThenInclude(p => p.Country)
                    .Include(c => c.InvoiceCity)
                    .ThenInclude(ic => ic.Province)
                    .ThenInclude(p => p.Country)
                    .FirstAsync(c => c.Id == id);
                return new ReturnResult(true, "", oCompanyMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectAll()
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Company> tmp = await db.Companies.ToListAsync();
                List<CompanyMV> lstCompanyMV = ObjectManipulation.CastObject<List<CompanyMV>>(tmp);
                return new ReturnResult(true, "", lstCompanyMV);
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
                return new ReturnResult(true, "", this);
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
                return new ReturnResult(true, "", this);
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
