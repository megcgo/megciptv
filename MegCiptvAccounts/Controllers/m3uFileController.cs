using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using MegCiptvAccounts.Helper;
using MegCiptvAccounts.Models;

namespace MegCiptvAccounts.Controllers
{
    public class m3uFileController : Controller
    {
        private readonly megciptvEntities _db = new megciptvEntities();

        // GET: m3uFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result()
        {

            listaResult viewmodel = new listaResult();

            viewmodel.linhasProcessadas = 0;
            List<listaDados> dummy = new List<listaDados>();
            viewmodel.ListaDados = dummy;

            var requestFile = Request.Files[0];
            // apanhar só extensão do ficheiro
            var fileext = requestFile.FileName.Substring(requestFile.FileName.LastIndexOf(".") + 1);

            if (!(requestFile.ContentLength > 0 && requestFile.ContentLength < 3145728 && fileext.ToLower().Contains("m3u")))
            {
                ViewBag.erro = "Error loading file. More than 3MB size or not an m3u file type or it has some error..!";
                return View(viewmodel);
            }

            viewmodel = new listManager().ProcessaStream(requestFile.InputStream);

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