using Microsoft.EntityFrameworkCore;
using Qerp.DBContext;
using Qerp.Interfaces;
using Qerp.Services;
using System.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace Qerp.ModelViews
{
    public class ImageMV : Models.Image
    {

        public static async Task<ReturnResult> SelectByAppTypeLinkedType(long appTypeId, long linkedTypeId)
        {
            try
            {
                using QerpContext db = new QerpContext();
                List<ImageMV> oImageMV = ObjectManipulation.CastObject<List<ImageMV>>(
                    await db.Images
                        .Where(i => i.LinkedapptypeId == appTypeId && i.LinkedtypeId == linkedTypeId)
                        .OrderBy(i => i.Sort)
                        .ToListAsync()
                );
                return new ReturnResult(true, "", oImageMV);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> SelectById(long id)
        {
            try
            {
                using QerpContext db = new QerpContext();
                ImageMV oImageMV = ObjectManipulation.CastObject<ImageMV>(
                    await db.Images
                        .FirstAsync(c => c.Id == id)
                );
                return new ReturnResult(true, "", oImageMV);
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
                ReturnResult result = await ImageMV.SelectById(this.Id);
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
                using QerpContext db = new QerpContext();
                db.Entry(this).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ReturnResult result = await ImageMV.SelectById(this.Id);
                return new ReturnResult(true, "", result.Object);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);

            }
        }

        public static async Task<ReturnResult> UpdateList(List<ImageMV> lstImages)
        {
            try
            {
                foreach(ImageMV imgMV in lstImages)
                {
                    ReturnResult response = await imgMV.Update();
                    if(!response.Success) { 
                        return new ReturnResult(false, response.Message, null);
                    }
                }
                return new ReturnResult(true, "", lstImages);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, ex.Message, null);
            }
        }

        public static async Task<ReturnResult> Upload(IFormFile oFile, long companyId)
        {
            try
            {
                var guid = Guid.NewGuid().ToString("N");
                var folderName = Path.Combine("Images", companyId.ToString());
                folderName = Path.Combine(folderName, guid);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                

                if (oFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(oFile.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var fullPath_Th = Path.Combine(pathToSave, "th_" + fileName);
                    //var dbPath = Path.Combine(folderName, fileName);
                    Directory.CreateDirectory(pathToSave);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        oFile.CopyTo(stream);
                    }

                    using (Image image = Image.Load(fullPath))
                    {
                        int width = 150; //image.Width / 2;
                        int height = 0; //image.Height / 2;
                        image.Mutate(x => x.Resize(width, height));

                        image.Save(fullPath_Th);
                    }

                    dynamic obj = new ExpandoObject();
                    obj.Image = Path.Combine(companyId.ToString(), Path.Combine(guid, fileName));
                    obj.Thumbnail = Path.Combine(companyId.ToString(), Path.Combine(guid, "th_" + fileName));

                    return new ReturnResult(true, "form.fileUploaded", obj);
                }
                else
                {
                    return new ReturnResult(false, "error.invalidFile", null);
                }
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
                List<string> lstLink;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    lstLink = this.Imagelink.Split(@"/").ToList();
                }
                else
                {
                    lstLink = this.Imagelink.Split(@"\").ToList();
                }

                lstLink.RemoveAt(lstLink.Count - 1);

                var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                
                foreach(string sLink in lstLink)
                {
                    pathToDelete = Path.Combine(pathToDelete, sLink);
                }

                if (Directory.Exists(pathToDelete))
                {
                    Directory.Delete(pathToDelete, true);
                }
                else
                {
                    return new ReturnResult(false, "err.pathNotFound", null);
                }

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
