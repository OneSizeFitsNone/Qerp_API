using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.Services;

namespace Qerp.ModelViews
{

    public class ClientMV : Client
    {

        public long? ForcedId;

        public static async Task<ReturnResult> SelectById(long id, long companyId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                ClientMV oClientMV = ObjectManipulation.CastObject<ClientMV>(
                    await db.Clients
                        .Include (c => c.City)
                        .ThenInclude(cc => cc.Province)
                        .ThenInclude(p => p.Country)
                        .Include(c => c.InvoiceCity)
                        .ThenInclude(ic => ic.Province)
                        .ThenInclude(p => p.Country)
                        .Include(c => c.Images)
                        .FirstAsync(c => c.Id == id && c.CompanyId == companyId)
                );
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
                List<ClientMV> lstClientMV = ObjectManipulation.CastObject<List<ClientMV>>(
                    await db.Clients
                    .Include(c => c.City).ThenInclude(cc => cc.Province).ThenInclude(p => p.Country)
                    .Include (c => c.InvoiceCity).ThenInclude (ic => ic.Province).ThenInclude (p => p.Country)
                    .Where(x => x.CompanyId == companyId).ToListAsync()
                );
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
                this.City = null;
                this.InvoiceCity = null;
                if(this.InvoiceSameAddress == 1) { this.InvoiceCityId = null; this.InvoiceAddress = null; }
                using QerpContext db = new QerpContext();
                db.Add(this);
                await db.SaveChangesAsync();
                ReturnResult result = await ClientMV.SelectById(this.Id, this.CompanyId);
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
                this.InvoiceCity = null;
                if (this.InvoiceSameAddress == 1) { this.InvoiceCityId = null; this.InvoiceAddress = null; }
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ReturnResult result = await ClientMV.SelectById(this.Id, this.CompanyId);
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
                List<Client> clients = await db.Clients
                    .Include(c => c.City)
                    .ThenInclude(cc => cc.Province)
                    .Include(c => c.City)
                    .ThenInclude(cc => cc.Country)
                    .Include(c => c.InvoiceCity)
                    .ThenInclude(cc => cc.Province)
                    .Include(c => c.InvoiceCity)
                    .ThenInclude(cc => cc.Country)
                    .Include(c => c.Images.OrderBy(i => i.Sort).Take(1))
                    .Where(c =>
                        c.CompanyId == companyId &&
                        (
                            (
                                this.ForcedId != null && 
                                (c.Id == this.ForcedId || (this.Name != null && (this.Name.Length > 2 && c.Name.StartsWith(this.Name))))
                            ) ||
                            (
                                this.ForcedId == null && 
                                (this.Name == null || c.Name.Contains(this.Name)) &&
                                (this.Email == null || c.Email == this.Email) &&
                                (this.Vat == null || c.Vat == this.Vat) &&
                                (this.Iban == null || c.Iban == this.Iban) &&
                                (this.Description == null || c.Description.Contains(this.Description)) &&
                                (this.Address == null || c.Address.Contains(this.Address)) &&
                                (this.CityId == null || c.CityId == this.CityId) &&
                                (this.City.ProvinceId == null || c.City.ProvinceId == this.City.ProvinceId) &&
                                (this.City.CountryId == null || c.City.CountryId == this.City.CountryId) &&
                                (this.InvoiceAddress == null || c.InvoiceAddress.Contains(this.InvoiceAddress)) &&
                                (this.InvoiceCityId == null || c.InvoiceCityId == this.InvoiceCityId) &&
                                (this.InvoiceCity.ProvinceId == null || c.InvoiceCity.ProvinceId == this.InvoiceCity.ProvinceId) &&
                                (this.InvoiceCity.CountryId == null || c.InvoiceCity.CountryId == this.InvoiceCity.CountryId)
                            )
                        )
                    )
                    .ToListAsync();

                if (this.ForcedId != null && this.ForcedId > 0)
                {
                    int i = clients.FindIndex(c => c.Id == this.ForcedId);
                    var oClient = clients[i];
                    clients.RemoveAt(i);
                    clients.Insert(0, oClient);
                }

                if (clients.Count == 0 && this.ForcedId == null)
                {
                    return new ReturnResult(false, "warn.noresultsfound", null);
                }
                else {
                    return new ReturnResult(true, "", clients);
                }
                
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }
    }
}
