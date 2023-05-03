using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;
using System.ComponentModel.Design;

namespace Qerp.ModelViews
{
    public class UserMV : User
    {

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                UserMV oUserMV = ObjectManipulation.CastObject<UserMV>(await db.Users.Where(x => x.CompanyId == companyId).FirstAsync(c => c.Id == id));
                return new ReturnResult(true, "", oUserMV);
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
                List<UserMV> lstUserMV = ObjectManipulation.CastObject<List<UserMV>>(await db.Users.Where(x => x.CompanyId == companyId).ToListAsync());
                return new ReturnResult(true, "", lstUserMV);
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
                
                if(this.Password == null)
                {
                    using QerpContext tmpDb = new QerpContext();
                    User? tmpUser = await tmpDb.Users.FirstOrDefaultAsync(u => u.Id == this.Id);
                    this.Password = tmpUser?.Password;
                }
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.Password = null;
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

        public static async Task<ReturnResult> Login(string username, string password)
        {
            try
            {
                using QerpContext db = new QerpContext();
                UserMV oUserMV = ObjectManipulation.CastObject<UserMV>(
                    await db.Users
                        .Include(u => u.Contact)
                        .ThenInclude(c => c.Images)
                        .Include(u => u.Saveditems)
                        .FirstOrDefaultAsync(u => u.Username == username && u.Password == password)
                );
                if(oUserMV != null)
                {
                    oUserMV.Password = null;
                    return new ReturnResult(true, "", oUserMV);
                }
                else
                {
                    return new ReturnResult(false, "warn.user.invalidcredentials", null);
                }
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }

        }

        public static long getCompanyByToken(string sToken)
        {
            using QerpContext db = new QerpContext();
            return 0;
        }

        
    }
}
