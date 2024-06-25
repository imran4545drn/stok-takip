using stoktakip.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace stoktakip.Controllers
{
    public class UrunController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();
       [Authorize]
        public ActionResult Index(string ara)
        {
            var list = db.Urun.ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                list = list.Where(x=>x.ad.Contains(ara)|| x.aciklama.Contains(ara)).ToList();
            }
            return View(list);
        }
        [Authorize(Roles ="A")]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.ad,
                                               Value = x.id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpPost]
       public ActionResult Ekle(Urun Data, HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/Image" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            Data.resim = File.FileName.ToString();
            db.Urun.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var urun = db.Urun.Where(x => x.id == id).FirstOrDefault();
            if (urun == null)
            {
                
                return HttpNotFound(); 
            }

            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var guncelle = db.Urun.Where(x => x.id == id).FirstOrDefault();

            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.ad,
                                               Value = x.id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;

            return View(guncelle);
        }
        [Authorize(Roles = "A")]

        [HttpPost]
        public ActionResult Guncelle(Urun model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var urun = db.Urun.Find(model.id);
                if (urun != null)
                {
                    urun.resim = file.FileName.ToString();
                    urun.ad = model.ad;
                    urun.aciklama = model.aciklama;
                    urun.fiyat = model.fiyat;
                    urun.stok = model.stok;
                    urun.populer = model.populer;
                    urun.kategoriid = model.kategoriid;

                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Image"), file.FileName);
                        file.SaveAs(path);
                        urun.resim = file.FileName;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.ad,
                                               Value = x.id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;

            return View(model);
        }
        public ActionResult KritikStok()
        {
            var kritik = db.Urun.Where(x => x.stok <= 50).ToList();
            return View(kritik);
        }
        public PartialViewResult StokCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var count = db.Urun.Where(x => x.stok < 50).Count();
                ViewBag.count = count;

                var azalan = db.Urun.Where(x => x.stok == 50).Count();
                ViewBag.azalan = azalan;
            }
            return PartialView();
        }
        public ActionResult StokGrafik()
        {
            ArrayList deger1 = new ArrayList();
            ArrayList deger2 = new ArrayList();
            var veriler = db.Urun.ToList();
            veriler.ToList().ForEach(x => deger1.Add(x.ad));
            veriler.ToList().ForEach(x => deger2.Add(x.stok));
            var grafik = new Chart(width: 500, height: 500).AddTitle("ürün-stok-grafiği").AddSeries(chartType: "Column", name: "ad", xValue: deger1, yValues: deger2);
            return File(grafik.ToWebImage().GetBytes(), "image/jpeg");


        }

    }
}