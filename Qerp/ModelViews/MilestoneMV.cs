using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;
using System.Linq;

namespace Qerp.ModelViews
{
    public class MilestoneMV : Milestone
    {
        public int TaskCount 
        { 
            get 
            {
                using QerpContext db = new QerpContext();
                int count = db.Tasks.Where(t => t.MilestoneId == this.Id).Count();
                return count; 
            } 
        }

        public int TasksCompleted
        {
            get
            {
                using QerpContext db = new QerpContext();
                int count = db.Tasks.Where(t => t.MilestoneId == this.Id && t.Completed == 1).Count();
                return count;
            }
        }

        public double PercentageCompleted
        {
            get
            {
                return this.TaskCount > 0 ? (this.TasksCompleted / this.TaskCount * 100) : 0;
            }
        }

        public DateTime? DeadlineFrom;
        public DateTime? DeadlineTo;
        public string? TypeNumber;

        public static async Task<ReturnResult> SelectByApptype(long appTypeId, long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<Milestone> lstMilestone = await db.Milestones
                    .Where(c => 
                        c.CompanyId == companyId &&
                        c.LinkedapptypeId == appTypeId &&
                        c.LinkedtypeId == id
                    )
                    .OrderBy(c => c.Deadline)
                    .ToListAsync();
                List<MilestoneMV>lstMilestoneMV = ObjectManipulation.CastObject<List<MilestoneMV>>(lstMilestone);
                return new ReturnResult(true, "", lstMilestoneMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }


        public static async Task<ReturnResult> Select(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                Milestone? oMilestone = await db.Milestones
                    .Include(c => c.Prospect)
                    .ThenInclude(c => c.Client)
                    .Include(c => c.Prospect)
                    .ThenInclude(c => c.Contact)
                    .Include(c => c.Project)
                    .ThenInclude(c => c.Client)
                    .Include(c => c.Project)
                    .ThenInclude(c => c.Contact)
                    .Where(c =>
                        c.CompanyId == companyId &&
                        c.Id == id
                    )
                    .FirstOrDefaultAsync();
                MilestoneMV lstMilestoneMV = ObjectManipulation.CastObject<MilestoneMV>(oMilestone);
                return new ReturnResult(true, "", lstMilestoneMV);
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
                List<Milestone> lstMilestone = await db.Milestones
                    .Where(c => c.CompanyId == companyId)
                    .OrderBy(c => c.Deadline)
                    .ToListAsync();
                return new ReturnResult(true, "", lstMilestone);
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
                List<Milestone> milestones = await db.Milestones
                    .Include(m => m.Prospect)
                    .Include(m => m.Project)

                    
                    .Where(c =>
                        c.CompanyId == companyId &&
                        (this.Name == null || c.Name.Contains(this.Name)) &&
                        (this.LinkedapptypeId == null || c.LinkedapptypeId == this.LinkedapptypeId) &&
                        (this.LinkedtypeId == null || c.LinkedtypeId == this.LinkedtypeId) &&
                        (this.Description == null || c.Description.Contains(this.Description)) &&
                        (this.Completed == null || c.Completed == this.Completed) &&
                        (this.DeadlineFrom == null || c.Deadline >= this.DeadlineFrom) &&
                        (this.DeadlineTo == null || c.Deadline <= this.DeadlineTo)
                    )
                    .OrderBy(p => p.Deadline)
                    .ToListAsync();

                List<MilestoneMV> lstMilestones = ObjectManipulation.CastObject<List<MilestoneMV>>(milestones);

                if (milestones.Count == 0)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", lstMilestones);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }


    }
}
