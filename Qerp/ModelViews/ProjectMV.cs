using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;
using System.Linq;

namespace Qerp.ModelViews
{
    public class ProjectMV : Project
    {
        public long? ForcedId;
        public DateTime? DeadlineFrom;
        public DateTime? DeadlineTo;

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Project oProject = await db.Projects
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
                    .Include(p => p.ProjectType)
                    .Include(p => p.Prospect)
                    .FirstAsync(c => c.Id == id && c.CompanyId == companyId);
                return new ReturnResult(true, "", oProject);
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
                List<Project> lstProject = await db.Projects
                    .Where(c => c.CompanyId == companyId)
                    .OrderBy(c => c.Number)
                    .ToListAsync();
                return new ReturnResult(true, "", lstProject);
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
                if (!result.Success)
                {
                    return new ReturnResult(false, result.Message, null);
                }

                CompanyMV company = ObjectManipulation.CastObject<CompanyMV>(result.Object);
                company.ProjectNumber += 1;

                this.Number = company.ProjectPrefix + company.ProjectNumber.ToString().PadLeft(6, '0');

                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();

                ApptypecontactMV apptypecontactMV = new ApptypecontactMV();
                apptypecontactMV.CompanyId = this.CompanyId;
                apptypecontactMV.ApptypeId = AppTypeMV.Project;
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
                List<Project> projects = await db.Projects
                    .Include(p => p.Client)
                    .Include(p => p.Contact)
                    .Include(p => p.ProjectType)
                    .Include(p => p.Prospect)
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
                                (this.ProjectTypeId == null || c.ProjectTypeId == this.ProjectTypeId) &&
                                (this.ProspectId == null || c.ProspectId == this.ProspectId) &&
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
                    int i = projects.FindIndex(c => c.Id == this.ForcedId);
                    var oProject = projects[i];
                    projects.RemoveAt(i);
                    projects.Insert(0, oProject);
                }

                if (projects.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", projects);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }
    }
}
