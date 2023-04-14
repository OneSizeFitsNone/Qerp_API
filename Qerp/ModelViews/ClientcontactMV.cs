using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class ClientcontactMV : Clientcontact
    {
        public static async Task<ReturnResult> SelectById(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                ClientcontactMV oClientcontactMV = ObjectManipulation.CastObject<ClientcontactMV>(
                    await db.Clientcontacts
                        .Include(c => c.Contact)
                        .ThenInclude(cc => cc.City)
                        .ThenInclude(ccc => ccc.Province)
                        .ThenInclude(cccp => cccp.Country)
                        .Include(c => c.Client)
                        .ThenInclude(cc => cc.City)
                        .ThenInclude(ccc => ccc.Province)
                        .ThenInclude(cccp => cccp.Country)
                        .Include(c => c.Contactrole)
                        .FirstAsync(c => c.Id == id)
                    );
                return new ReturnResult(true, "", oClientcontactMV);
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
                List<ClientcontactMV> lstClientcontactMV = ObjectManipulation.CastObject<List<ClientcontactMV>>(await db.Clientcontacts.ToListAsync());
                return new ReturnResult(true, "", lstClientcontactMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }


        public static async Task<ReturnResult> SelectByContactId(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Clientcontact> lstClientcontacts = await db.Clientcontacts
                    .Include(c => c.Client)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contactrole)
                    .Where(cc => cc.ContactId == id)
                    .OrderBy(cc => cc.Client.Name)
                    .ToListAsync();
                return new ReturnResult(true, "", lstClientcontacts);
            }
            catch (Exception ex) 
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectByClientId(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Clientcontact> lstClientcontacts = await db.Clientcontacts
                    .Include(c => c.Contact)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contactrole)
                    .Where(cc => cc.ClientId == id)
                    .OrderBy(cc => cc.Client.Name)
                    .ToListAsync();
                return new ReturnResult(true, "", lstClientcontacts);
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

                return await ClientcontactMV.SelectById(this.Id);
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
                return await ClientcontactMV.SelectById(this.Id);
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
