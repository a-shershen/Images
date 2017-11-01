using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace Images.WEB.Controllers
{
    public class ExifController : Controller
    {
        private Images.Exif.Lib.Interfaces.IExifService exifService;

        private static List<counter> counterList = new List<counter>();

        private class counter
        {
            public static int totalCount { get; set; }
            public string fileName { get; set; }
            public string ip { get; set; }
            public DateTime dateTime { get; set; }

            public override string ToString()
            {
                return $"total: {totalCount}<br \\>file: {fileName}<br \\>ip: {ip}<br \\>date time:{dateTime.ToLongDateString()}<br \\><br \\>";
            }
        }

        public ExifController(Images.Exif.Lib.Interfaces.IExifService iExifService)
        {
            exifService = iExifService;
        }

        public string GetCount()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach(var data in counterList)
            {
                sb.Append(data.ToString());
            }

            return sb.ToString();
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
                if(file.FileName=="")
                {
                    ViewBag.Error = true;
                    ViewBag.ErrorMessage = "Неверный формат или пустой файл";
                    return View("Index", null);
                }

                if(file.ContentLength>= 10000000)
                {
                    ViewBag.Error = true;
                    ViewBag.ErrorMessage = "Файл слишком большой (попробуйте меньше 5-7 мб)";
                    return View("Index", null);
                }
                
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
                        return View("Index", null);
                }

                string fileName
                    = Images.Hashing.Lib.Sha2Hash.GetHash(file.FileName
                    + DateTime.Now.ToLongDateString())
                    + ext;

                string serverPath = Server.MapPath("~/Images/Temp/")
                   + fileName;

#if DEBUG
                ViewBag.Path = "/Images/Temp/" + fileName;

#else
                ViewBag.Path = "http://andreiimages-001.myasp.net/Images/Temp/" + fileName;
#endif

                IDictionary< string, string> dict;

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

                counterList.Add(
                    new counter
                    {
                        dateTime = DateTime.Now, fileName = file.FileName, ip = Request.UserHostAddress
                    });

                counter.totalCount++;

                return View("Index", dict);
            }

            else
            {
                return View("Index", null);
            }
        }
    }
}