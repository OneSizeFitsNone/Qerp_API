using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;
using System.Runtime.CompilerServices;

namespace Qerp.ModelViews
{
    public class ApptypecontactMV : Apptypecontact
    {
        public static async Task<ReturnResult> SelectById(long companyId, long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Apptypecontact? oApptypecontact = 
                    await db.Apptypecontacts
                        .Include(c => c.Client)
                        .ThenInclude(cc => cc.City)
                        .ThenInclude(ccc => ccc.Province)
                        .ThenInclude(cccp => cccp.Country)
                        .Include(c => c.Contact)
                        .ThenInclude(cc => cc.City)
                        .ThenInclude(ccc => ccc.Province)
                        .ThenInclude(cccp => cccp.Country)
                        .Include(c => c.Contactrole)

                        .FirstOrDefaultAsync(c => c.Id == id);
                return new ReturnResult(true, "", oApptypecontact);
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
                List<Apptypecontact> lstApptypecontact = await db.Apptypecontacts.ToListAsync();
                return new ReturnResult(true, "", lstApptypecontact);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<Apptypecontact> SelectByApptypeContactClient(long companyId, long appTypeId, long linkedId, long? contactId, long? clientId)
        {
            try
            {
                using QerpContext db = new QerpContext();

                return await db.Apptypecontacts
                    .Where(at => 
                        at.CompanyId == companyId &&
                        at.ApptypeId == appTypeId &&
                        at.LinkedId == linkedId &&
                        at.ContactId == contactId &&
                        at.ClientId == clientId
                    )
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
            
        }

        public static async Task<ReturnResult> SelectBySource(long companyId, long apptypeId, long linkedId, long requestedType)
        {
            try
            {

                using QerpContext db = new QerpContext();
                
                List<Apptypecontact> lstApptypecontacts = await db.Apptypecontacts
                    .Include(c => c.Client)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contact)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contactrole)
                    .Include(c => c.Project)
                    .Include(c => c.Task)
                    .Include(c => c.Prospect)
                    .Where(cc => 
                        cc.CompanyId == companyId &&
                        cc.ApptypeId == requestedType &&
                        (
                            (apptypeId == AppTypeMV.Contact && cc.ContactId == linkedId) ||
                            (apptypeId == AppTypeMV.Client && cc.ClientId == linkedId)
                        )
                     )
                    .OrderBy(cc => cc.Project.Number)
                    .ThenBy(cc => cc.Prospect.Number)
                    .ThenBy(cc => cc.Client.Name)
                    .ThenBy(cc => cc.Contact.Fullname)
                    .ToListAsync();

                return new ReturnResult(true, "", lstApptypecontacts);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectByApptypeLinkedId(long companyId, long apptypeId, long linkedId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Apptypecontact> lstApptypecontacts = await db.Apptypecontacts
                    .Include(c => c.Client)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contact)
                    .ThenInclude(cc => cc.City)
                    .ThenInclude(ccc => ccc.Province)
                    .ThenInclude(cccp => cccp.Country)
                    .Include(c => c.Contactrole)
                    //.Include(c => c.Project)
                    //.Include(c => c.Task)
                    //.Include(c => c.Prospect)
                    .Where(cc => cc.ApptypeId == apptypeId && cc.LinkedId == linkedId && cc.CompanyId == companyId)
                    .OrderBy(cc => cc.Client.Name)
                    .ThenBy(cc => cc.Contact.Fullname)
                    .ToListAsync();
                return new ReturnResult(true, "", lstApptypecontacts);
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

                return await ApptypecontactMV.SelectById(this.CompanyId, this.Id);
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
                return await ApptypecontactMV.SelectById(this.CompanyId, this.Id);
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
