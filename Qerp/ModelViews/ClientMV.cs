using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class ClientMV : Client
    {
        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                ClientMV oClientMV = ObjectManipulation.CastObject<ClientMV>(await db.Clients.FirstAsync(c => c.Id == id && c.CompanyId == companyId));
                return new ReturnResult(true, "", oClientMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectAll(long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<ClientMV> lstClientMV = ObjectManipulation.CastObject<List<ClientMV>>(await db.Clients.Where(x => x.CompanyId == companyId).ToListAsync());
                return new ReturnResult(true, "", lstClientMV);
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

        public async Task<ReturnResult> Search(long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<ClientMV> clients = ObjectManipulation.CastObject<List<ClientMV>>(
                    await db.Clients.Where(c =>
                        c.CompanyId == companyId &&
                        (this.Name == null || c.Name.Contains(this.Name)) &&
                        (this.Address == null || c.Address.Contains(this.Address)) &&
                        (this.Email == null || c.Email == this.Email) &&
                        (this.Vat == null || c.Vat == this.Vat) &&
                        (this.Iban == null || c.Iban == this.Iban)
                    ).ToListAsync()
                );
                if(clients.Count > 0)
                {
                    return new ReturnResult(true, "", clients);
                }
                else {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }
    }
}
