using Bogus;
using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Models;
using System.Security.Cryptography.Xml;
using Xunit;
using Xunit.Sdk;

namespace Qerp.Conversions
{
    public class DatabaseCleanup
    {
        [Fact]
        public async void FixCitiesProvinceId()
        {
            using QerpContext db = new QerpContext();
            List<Province> lstProvinces = await db.Provinces.ToListAsync();
            List<City> lstCities = await db.Cities.ToListAsync();

            foreach(City citie in lstCities)
            {
                Province? province = lstProvinces.FirstOrDefault(p => p.ImportName == citie.ImportProvince);
                if (province == null) continue;

                citie.ProvinceId = province.Id;

                db.Entry(citie).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }

        }

        [Fact]
        public async void SeedContacts()
        {
            try
            {
                using QerpContext db = new QerpContext();
                using QerpContext db2 = new QerpContext();
                List<City> lstCities = await db.Cities.ToListAsync();

                for (int i = 0; i < 900000; i++)
                {
                    Contact oContact = GetContact().Generate();
                    Random rnd = new Random();
                    City oCity = lstCities[rnd.Next(lstCities.Count() - 1)];
                    oContact.CityId = oCity.Id;
                    oContact.CompanyId = 1000000;

                    db2.Add(oContact);
                    await db2.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var test = ex.Message;
            }

        }

        public static Faker<Contact> GetContact()
        {
            return new Faker<Contact>()
                 .RuleFor(e => e.Name, f => f.Name.FirstName())
                 .RuleFor(e => e.Surname, f => f.Name.LastName())
                 .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name, e.Surname))
                 .RuleFor(e => e.Mobile, f => f.Phone.PhoneNumber())
                 .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                 .RuleFor(e => e.Address, f => f.Address.StreetAddress())
                 .RuleFor(e => e.Description, f => f.Rant.Review("user"));
        }

    }
}
