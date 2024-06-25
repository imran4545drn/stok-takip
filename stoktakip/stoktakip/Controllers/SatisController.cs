using stoktakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace stoktakip.Controllers
{
    public class SatisController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();

        public ActionResult Index(int sayfa = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.email == kullaniciadi);
                var model = db.Satislar.Where(x => x.kullaniciid == kullanici.id).ToList().ToPagedList(sayfa, 5);
                return View(model);
            }
            return HttpNotFound();
        }

        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.id == id);
                    var satis = new Satislar
                    {
                        kullaniciid = model.kullaniciid,
                        urunid = model.urunid,
                        adet = model.adet,
                        resim = model.resim,
                        fiyat = model.fiyat,
                        tarih = model.tarih,
                    };
                    db.Sepet.Remove(model);
                    db.Satislar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın alma işlemi başarılı bir şekilde gerçekleşmiştir.";
                }
            }
            catch (Exception)
            {
                ViewBag.islem = "Satın alma işlemi başarısız.";
            }
            return View("islem");
        }

        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.email == kullaniciadi);
                var model = db.Sepet.Where(x => x.kullaniciid == kullanici.id).ToList();
                if (model.Count > 0)
                {
                    Tutar = model.Sum(x => x.Urun.fiyat * x.adet);
                    ViewBag.Tutar = "Toplam Tutar = " + Tutar + " TL";
                    return View(model);
                }
                ViewBag.Tutar = "Sepetinizde ürün bulunmamaktadır.";
                return View();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult HepsiniSatinAl2()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.email == username);
                var model = db.Sepet.Where(x => x.kullaniciid == kullanici.id).ToList();

                foreach (var item in model)
                {
                    var satis = new Satislar
                    {
                        kullaniciid = item.kullaniciid,
                        urunid = item.urunid,
                        adet = item.adet,
                        fiyat = item.fiyat,
                        resim = item.resim,
                        tarih = item.tarih
                    };
                    db.Satislar.Add(satis);
                }

                db.Sepet.RemoveRange(model);
                db.SaveChanges();

                return RedirectToAction("Index", "Sepet");
            }
            return HttpNotFound();
        }
    }
}
