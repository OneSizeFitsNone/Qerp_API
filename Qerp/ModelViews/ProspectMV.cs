using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class ProspectMV : Prospect
    {
        public long? ForcedId;
        public DateTime? DeadlineFrom;
        public DateTime? DeadlineTo;

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Prospect oProspect = await db.Prospects
                    .Include(p => p.Client)
                    .ThenInclude(p => p.City)
                    .ThenInclude(p => p.Province)
                    .ThenInclude(p => p.Country)
                    .Include(p => p.Client)
                    .ThenInclude(p => p.Images)
                    .Include(p => p.Contact)
                    .ThenInclude(p => p.City)
                    .ThenInclude(p => p.Province)
                    .ThenInclude(p => p.Country)
                    .Include(p => p.Contact)
                    .ThenInclude(p => p.Images)
                    .Include(p => p.ProspectType)
                    .FirstAsync(c => c.Id == id && c.CompanyId == companyId);
                return new ReturnResult(true, "", oProspect);
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
                List<Prospect> lstProspect = await db.Prospects
                    .Where(c => c.CompanyId == companyId)
                    .OrderBy(c => c.Number)
                    .ToListAsync();
                return new ReturnResult(true, "", lstProspect);
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
                ReturnResult result = await CompanyMV.SelectById(this.CompanyId);
                if(!result.Success)
                {
                    return new ReturnResult(false, result.Message, null);
                }

                CompanyMV company = ObjectManipulation.CastObject<CompanyMV>(result.Object);
                company.ProspectNumber += 1;

                this.Number = company.ProspectPrefix + company.ProspectNumber.ToString().PadLeft(6, '0');

                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();

                ApptypecontactMV apptypecontactMV = new ApptypecontactMV();
                apptypecontactMV.CompanyId = this.CompanyId;
                apptypecontactMV.ApptypeId = AppTypeMV.Prospect;
                apptypecontactMV.LinkedId = this.Id;
                apptypecontactMV.ContactId = this.ContactId;
                apptypecontactMV.ClientId = this.ClientId;
                await apptypecontactMV.Insert();
                
                await company.Update();
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

                if(
                    this.ContactId != null && this.ClientId != null &&
                    (await ApptypecontactMV.SelectByApptypeContactClient(this.CompanyId, AppTypeMV.Prospect, this.Id, this.ContactId, this.ClientId) == null)
                )
                {
                    ApptypecontactMV apptypecontactMV = new ApptypecontactMV();
                    apptypecontactMV.CompanyId = this.CompanyId;
                    apptypecontactMV.ApptypeId = AppTypeMV.Prospect;
                    apptypecontactMV.LinkedId = this.Id;
                    apptypecontactMV.ContactId = this.ContactId;
                    apptypecontactMV.ClientId = this.ClientId;
                    await apptypecontactMV.Insert();
                }

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
                List<Prospect> prospects = await db.Prospects
                    .Include(p => p.Client)
                    .Include(p => p.Contact)
                    .Include(p => p.ProspectType)
                    .Include(p => p.Status)
                    .Where(c =>
                        c.CompanyId == companyId &&
                        (
                            (
                                this.ForcedId != null &&
                                (c.Id == this.ForcedId || (this.Number != null && (this.Number.Length > 2 && c.Number.StartsWith(this.Number))))
                            ) ||
                            (
                                this.ForcedId == null &&
                                (this.Number == null || c.Number.Contains(this.Number)) &&
                                (this.ClientId == null || c.ClientId == this.ClientId) &&
                                (this.ContactId == null || c.ContactId == this.ContactId) &&
                                (this.ProspectTypeId == null || c.ProspectTypeId == this.ProspectTypeId) &&
                                (this.Description == null || c.Description.Contains(this.Description)) &&
                                (this.DeadlineFrom == null || c.Deadline >= this.DeadlineFrom) &&
                                (this.DeadlineTo == null || c.Deadline <= this.DeadlineTo) &&
                                (this.StatusId == null || c.StatusId == this.StatusId)
                            )
                        )
                    )
                    .OrderBy(p => p.Number)
                    .ToListAsync();

                if (this.ForcedId != null && this.ForcedId > 0)
                {
                    int i = prospects.FindIndex(c => c.Id == this.ForcedId);
                    var oProspect = prospects[i];
                    prospects.RemoveAt(i);
                    prospects.Insert(0, oProspect);
                }

                if (prospects.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", prospects);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

    }
}
