using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace Images.WEB.Controllers
{
    public class ExifController : Controller
    {
        private Images.Exif.Lib.Interfaces.IExifService exifService;

        public ExifController(Images.Exif.Lib.Interfaces.IExifService iExifService)
        {
            exifService = iExifService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            HttpPostedFileBase file = Request.Files[0];

            if (file != null)
            {
                
                string ext;

                switch (file.ContentType)
                {
                    case "image/jpeg":
                        ext = ".jpg";
                        break;
                    case "image/png":
                        ext = ".png";
                        break;
                    default:
                        ViewBag.Error = true;
                        ViewBag.ErrorMessage = "Неверный формат";
                        return View();
                }

                string fileName
                    = Images.Hashing.Lib.Sha2Hash.GetHash(file.FileName
                    + DateTime.Now.ToLongDateString())
                    + ext;

                string serverPath = Server.MapPath("~/Images/Temp/")
                   + fileName;

                ViewBag.Path = "http://images.somee.com/Images/Temp/" + fileName;

                IDictionary<string, string> dict;

                using (Image img = Image.FromStream(file.InputStream))
                {
                    dict = exifService.GetProperties(img);
                }

                if (dict != null && dict.Count != 0)
                {
                    ViewBag.WithExif = true;
                }

                else
                {
                    ViewBag.WithExif = false;
                }


                file.SaveAs(serverPath);
                file.InputStream.Dispose();

                return View("Index", dict);
            }

            else
            {
                return View();
            }
        }
    }
}