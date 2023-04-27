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

    }
}
