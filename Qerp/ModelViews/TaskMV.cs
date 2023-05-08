using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Services;
using Qerp.Models;

namespace Qerp.ModelViews
{
    public class TaskMV : Models.Task
    {

        public long? ForcedId;
        public DateTime? DeadlineFrom;
        public DateTime? DeadlineTo;
        public long? SearchAppType;

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Models.Task oTask = await db.Tasks
                    .Include(t => t.Contact)
                    .Include(t => t.Client)
                    .Include(t => t.Prospect)
                    .ThenInclude(p => p.Contact)
                    .Include(t => t.Prospect)
                    .ThenInclude(p => p.Client)
                    .Include(t => t.Project)
                    .ThenInclude(p => p.Contact)
                    .Include(t => t.Project)
                    .ThenInclude(p => p.Client)
                    .Include(t => t.Milestone)
                    .FirstAsync(c => c.Id == id && c.CompanyId == companyId);
                return new ReturnResult(true, "", oTask);
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
                List<TaskMV> lstTaskMV = ObjectManipulation.CastObject<List<TaskMV>>(await db.Tasks.Where(x => x.CompanyId == companyId).ToListAsync());
                return new ReturnResult(true, "", lstTaskMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectByApptype(long companyId, long apptypeId, long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Models.Task> lstTasks = await db.Tasks
                    .Include(t => t.Contact)
                    .Include(t => t.Client)
                    .Include(t => t.Prospect)
                    .Include(t => t.Project)
                    .Include(t => t.Milestone)
                    .Where(t => t.CompanyId == companyId &&
                        (
                            (apptypeId == AppTypeMV.Milestone && t.MilestoneId == id) ||
                            (apptypeId == AppTypeMV.Prospect && t.ProspectId == id) ||
                            (apptypeId == AppTypeMV.Project && t.ProjectId == id) ||
                            (apptypeId == AppTypeMV.Contact && t.ContactId == id) ||
                            (apptypeId == AppTypeMV.Client && t.ClientId == id)
                        )
                    )
                    .OrderBy(t => t.Deadline)
                    .ToListAsync();
                
                return new ReturnResult(true, "", lstTasks);
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
                if(this.MilestoneId != null)
                {
                    if(!await this.HandleMilestoneTask())
                    {
                        return new ReturnResult(false, "err.milestonehandler", null);
                    }
                }

                if(this.ProspectId > 0)
                {
                    Prospect? oProspect = (Prospect)(await ProspectMV.SelectById(this.ProspectId ?? 0, this.CompanyId)).Object;
                    if(oProspect != null)
                    {
                        this.ClientId = oProspect.ClientId;
                    }
                }
                else if(this.ProjectId > 0)
                {
                    Project? oProject = (Project)(await ProjectMV.SelectById(this.ProjectId ?? 0, this.CompanyId)).Object;
                    if(oProject != null)
                    {
                        this.ClientId = oProject.ClientId;
                    }                    
                }


                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();
                return await TaskMV.SelectById(this.Id, this.CompanyId);
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
                if (this.MilestoneId != null)
                {
                    if (!await this.HandleMilestoneTask())
                    {
                        return new ReturnResult(false, "err.milestonehandler", null);
                    }
                }

                if (this.ProspectId > 0)
                {
                    Prospect? oProspect = (Prospect)(await ProspectMV.SelectById(this.ProspectId ?? 0, this.CompanyId)).Object;
                    if (oProspect != null)
                    {
                        this.ClientId = oProspect.ClientId;
                    }
                }
                else if (this.ProjectId > 0)
                {
                    Project? oProject = (Project)(await ProjectMV.SelectById(this.ProjectId ?? 0, this.CompanyId)).Object;
                    if (oProject != null)
                    {
                        this.ClientId = oProject.ClientId;
                    }
                }

                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return await TaskMV.SelectById(this.Id, this.CompanyId);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);

            }
        }

        private async Task<bool> HandleMilestoneTask()
        {
            try {
                using QerpContext db = new QerpContext();
                Milestone? milestone = await db.Milestones.FirstOrDefaultAsync(ms => ms.Id == this.MilestoneId);
                if(milestone == null)
                {
                    return false;
                }
                else
                {
                    if(milestone.LinkedapptypeId == AppTypeMV.Project)
                    {
                        this.ProjectId = milestone.LinkedtypeId;
                    }
                    else if(milestone.LinkedapptypeId == AppTypeMV.Prospect)
                    {
                        this.ProspectId = milestone.LinkedtypeId;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

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
                List<Models.Task> tasks = await db.Tasks
                    .Include(t => t.Prospect)
                    .Include(t => t.Project)
                    .Include(t => t.Milestone)
                    .Include(t => t.Contact)
                    .Where(c =>
                        c.CompanyId == companyId &&
                        (
                            (
                                this.ForcedId != null &&
                                (c.Id == this.ForcedId || (this.Title != null && (this.Title.Length > 2 && c.Title.StartsWith(this.Title))))
                            ) ||
                            (
                                this.ForcedId == null &&
                                (this.Title == null || c.Title.Contains(this.Title)) &&
                                (this.ContactId == null || c.ContactId == this.ContactId) &&
                                (
                                    this.SearchAppType == null ||
                                    (this.SearchAppType == AppTypeMV.Prospect && c.ProspectId != null) ||
                                    (this.SearchAppType == AppTypeMV.Project && c.ProjectId != null) ||
                                    (this.SearchAppType == AppTypeMV.Milestone && c.MilestoneId != null)
                                ) &&
                                (this.ProspectId == null || c.ProspectId == this.ProspectId) &&
                                (this.ProjectId == null || c.ProjectId == this.ProjectId) &&
                                (this.MilestoneId == null || c.MilestoneId == this.MilestoneId) &&
                                (this.Description == null || c.Description.Contains(this.Description)) &&
                                (this.Completed == null || c.Completed == this.Completed) &&
                                (this.ToInvoice == null || c.ToInvoice == this.ToInvoice) &&
                                (this.DeadlineFrom == null || c.Deadline >= this.DeadlineFrom) &&
                                (this.DeadlineTo == null || c.Deadline <= this.DeadlineTo)
                            )
                        )
                    )
                    .OrderBy(p => p.Deadline)
                    .ToListAsync();

                //List<MilestoneMV> lstMilestones = ObjectManipulation.CastObject<List<MilestoneMV>>(milestones);

                if (tasks.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", tasks);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

    }
}
