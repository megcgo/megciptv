using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace MegCiptvAccounts.Controllers
{
    public class m3uFileController : Controller
    {
        // GET: m3uFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result()
        {
            string urlGetm3u = string.Empty;
            


            string pathFilename;
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/";
            var requestFile = Request.Files[0];
            // apanhar só extensão do ficheiro
            var fileext = requestFile.FileName.Substring(requestFile.FileName.LastIndexOf(".") + 1);

            if ((!(requestFile != null && requestFile.ContentLength > 0)) || (requestFile.ContentLength > 1048576) || (fileext.ToLower() != "m3u"))
            {
                ViewBag.erro = "Error loading file. Not a m3u file type!";
                return View();
            }

            using (var parser = new TextFieldParser(requestFile.InputStream))
            {
                parser.CommentTokens = new[] {"#"};
                while (!parser.EndOfData)
                {
                    var currentRow = parser.ReadLine();

                    if (currentRow.StartsWith("http://") && currentRow.Contains("/live/"))
                    {
                        string[] variasPartes = currentRow.Split('/');
                        string servidor = variasPartes[2];
                        string login = variasPartes[4];
                        string pass = variasPartes[5];


                        ViewBag.url = "http://" + servidor + "/get.php?username=" + login + "&password=" + pass +
                                       "&type=m3u_plus&output=mpegts";

                    }




                }
            }

                //pathFilename = Path.Combine(path, DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Ticks.ToString() + "." + fileext);
                //requestFile.SaveAs(pathFilename);
                //if (System.IO.File.Exists(pathFilename)) { System.IO.File.Delete(pathFilename); }

                return View();
        }



    }
}