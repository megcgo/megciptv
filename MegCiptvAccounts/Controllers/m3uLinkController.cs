using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MegCiptvAccounts.Helper;
using MegCiptvAccounts.Models;

namespace MegCiptvAccounts.Controllers
{
    public class m3uLinkController : Controller
    {

        private readonly megciptvEntities _db = new megciptvEntities();
        // GET: m3uLink
        public ActionResult Index(string remoteUrl)
        {
            remoteUrlViewModel viewmodel = new remoteUrlViewModel();

            viewmodel.remoteUrl = string.IsNullOrEmpty(remoteUrl) ? "" : remoteUrl;

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Result(string remoteUrl)
        {

            listaResult viewmodel = new listaResult();

            //HttpWebResponse response = null;

            viewmodel.linhasProcessadas = 0;
            viewmodel.ListaDados = new List<listaDados>();

            try
            {
                HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(remoteUrl);
                requestFile.AllowAutoRedirect = true;
                requestFile.UseDefaultCredentials = true;
                requestFile.Credentials = CredentialCache.DefaultCredentials;
                requestFile.Referer = remoteUrl;
                requestFile.Timeout = 5000;
                requestFile.KeepAlive = true;
                requestFile.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                requestFile.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";
                requestFile.CookieContainer = new CookieContainer();

                using (HttpWebResponse response = (HttpWebResponse)requestFile.GetResponse())
                {
                    Stream textFromUrl = response.GetResponseStream();
                    viewmodel = new listManager().ProcessaStream(textFromUrl);
                };
            }
            catch (WebException we)
            {
                HttpWebResponse resp = we.Response as HttpWebResponse;
                string absUri = resp.ResponseUri.AbsoluteUri;
                string[] getURL = absUri.Split(',');

                if (absUri.Contains("dropboxusercontent"))
                {
                    return Result(getURL[1]);
                }

                ViewBag.erro = "Error loading URL: " + we.Status + " - " + (int)resp.StatusCode + " " + resp.StatusCode;
                return View(viewmodel);
            }

            // Add to DB --------------------------------------------------
            List<lista> fromDB = _db.lista.Where(d => d.Data == DateTime.Today.Date).ToList();

            foreach (var item in viewmodel.ListaDados.Where(w => w.working))
            {
                lista toDB = new lista
                {
                    Server = item.servidor,
                    Login = item.username,
                    Pass = item.password,
                    Data = DateTime.Today
                };

                if (
                    !fromDB.Any(
                        a =>
                            a.Data == toDB.Data
                         && a.Login == toDB.Login
                         && a.Pass == toDB.Pass
                         && a.Server == toDB.Server)
                   )
                    _db.lista.Add(toDB);
            }

            _db.SaveChanges();
            // /Add to DB --------------------------------------------------

            return View(viewmodel);
        }



    }
}
