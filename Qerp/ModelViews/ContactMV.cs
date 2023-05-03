using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{
    public class ContactMV : Contact
    {

        public long? ForcedId;

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                ContactMV oContactMV = ObjectManipulation.CastObject<ContactMV>(
                    await db.Contacts
                        .Include(c => c.City)
                        .ThenInclude(cc => cc.Province)
                        .ThenInclude(p => p.Country)
                        .Include(c => c.Images)
                        .FirstAsync(c => c.Id == id && c.CompanyId == companyId)
                );
                return new ReturnResult(true, "", oContactMV);
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
                List<ContactMV> lstContactMV = ObjectManipulation.CastObject<List<ContactMV>>(
                    await db.Contacts
                        .Include(c => c.City)
                        .ThenInclude(cc => cc.Province)
                        .ThenInclude(p => p.Country)
                        .Where(x => x.CompanyId == companyId)
                        .ToListAsync());
                return new ReturnResult(true, "", lstContactMV);
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
                this.City = null;
                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();
                ReturnResult result = await ContactMV.SelectById(this.Id, this.CompanyId);
                return new ReturnResult(true, "", result.Object);
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
                this.City = null;
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ReturnResult result = await ContactMV.SelectById(this.Id, this.CompanyId);
                return new ReturnResult(true, "", result.Object);
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
                List<Contact> lstContacts = await db.Contacts
                        .Include(c => c.City)
                        .ThenInclude(cc => cc.Province)
                        .Include(c => c.City)
                        .ThenInclude(cc => cc.Country)
                        .Include(c => c.Images.OrderBy(i => i.Sort).Take(1))
                        .Where(c =>
                            c.CompanyId == companyId &&
                            (
                                (
                                    this.ForcedId != null &&
                                    (c.Id == this.ForcedId || (this.Fullname != null && (this.Fullname.Length > 2 && c.Fullname.StartsWith(this.Fullname))))
                                ) ||
                                (
                                    this.ForcedId == null &&
                                    (this.Fullname == null || c.Fullname.Contains(this.Fullname)) &&
                                    (this.Name == null || c.Name.Contains(this.Name)) &&
                                    (this.Surname == null || c.Surname.Contains(this.Surname)) &&
                                    (this.Description == null || c.Description.Contains(this.Description)) &&
                                    (this.Address == null || c.Address.Contains(this.Address)) &&
                                    (this.Email == null || c.Email == this.Email) &&
                                    (this.CityId == null || c.CityId == this.CityId) &&
                                    (this.City.ProvinceId == null || c.City.ProvinceId == this.City.ProvinceId) &&
                                    (this.City.CountryId == null || c.City.CountryId == this.City.CountryId)
                                )
                            )

                        )
                        .ToListAsync();

                //List<ContactMV> contacts = ObjectManipulation.CastObject<List<ContactMV>>(
                //    lstContacts
                //);
                if (lstContacts.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", lstContacts);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public async Task<ReturnResult> SearchUser(long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<User> lstUsers = await db.Users
                        .Include(u => u.Contact)
                        .Where(c =>
                            c.CompanyId == companyId &&
                            (
                                this.ForcedId != null &&
                                (c.Id == this.ForcedId || (this.Fullname != null && (this.Fullname.Length > 2 && c.Contact.Fullname.StartsWith(this.Fullname))))
                            )
                        )
                        .ToListAsync();

                List<Contact> lstContacts = lstUsers.Select(c =>  c.Contact).ToList();


                if (lstContacts.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else
                {
                    return new ReturnResult(true, "", lstContacts);
                }

            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

    }
}
