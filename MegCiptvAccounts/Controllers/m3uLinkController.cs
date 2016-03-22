using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MegCiptvAccounts.Helper;
using MegCiptvAccounts.Models;
using System.Security.Cryptography.X509Certificates;

namespace MegCiptvAccounts.Controllers
{
    public class m3uLinkController : Controller
    {
        // GET: m3uLink
        public ActionResult Index()
        {
            return View();
        }

        private static bool CustomXertificateValidation(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }


        [HttpPost]
        public ActionResult Result(string remoteUrl)
        {

            if (ServicePointManager.ServerCertificateValidationCallback == null)
            {
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(CustomXertificateValidation);
            };



        listaResult viewmodel = new listaResult();

            //HttpWebResponse response = null;
            Stream textFromUrl = null;


            viewmodel.linhasProcessadas = 0;
            viewmodel.ListaDados = new List<listaDados>();

            try
            {
                HttpWebRequest requestFile = (HttpWebRequest)WebRequest.Create(remoteUrl);
                requestFile.MaximumAutomaticRedirections = 10;
                requestFile.AllowAutoRedirect = true;
                requestFile.Credentials = CredentialCache.DefaultCredentials;
                requestFile.UseDefaultCredentials = true;
                requestFile.Referer = remoteUrl;
                requestFile.Timeout = 10000;
                requestFile.KeepAlive = false;
                requestFile.Accept = "*/*";
                requestFile.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                requestFile.CookieContainer = new CookieContainer();

                using (HttpWebResponse response = (HttpWebResponse)requestFile.GetResponse())
                {
                    textFromUrl = response.GetResponseStream();
                    viewmodel = new listManager().ProcessaStream(textFromUrl);
                };
            }
            catch (WebException we)
            {
                HttpWebResponse resp = we.Response as HttpWebResponse;

                //if (resp.Headers["Location"] )

                ViewBag.erro = "Error loading URL: " + we.Status + " - " + (int)resp.StatusCode + " - " + resp.StatusDescription;
                return View(viewmodel);
            }


            return View(viewmodel);
        }



    }
}
