using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MegCiptvAccounts.Helper;
using MegCiptvAccounts.Models;

namespace MegCiptvAccounts.Controllers
{
    public class m3uLinkController : Controller
    {
        // GET: m3uLink
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result(string remoteUrl)
        {
            listaResult viewmodel = new listaResult();

            Stream textFromUrl = null;

            try
            {
                HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(remoteUrl);
                requestFile.AllowAutoRedirect = true;
                requestFile.KeepAlive = false;
                HttpWebResponse response = (HttpWebResponse)requestFile.GetResponse();
                textFromUrl = response.GetResponseStream();
                viewmodel = new listManager().ProcessaStream(textFromUrl);
            }
            catch (WebException we)
            {
                ViewBag.erro = "Error loading URL: " + we.Status;
                viewmodel.linhasProcessadas = 0;
                viewmodel.ListaDados = new List<listaDados>();
                return View(viewmodel);
            }


            return View(viewmodel);
        }



    }
}
