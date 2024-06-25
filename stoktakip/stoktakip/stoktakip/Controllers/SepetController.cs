using stoktakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stoktakip.Controllers
{
    public class SepetController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();
        public ActionResult Index(decimal?Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x=> x.email == kullaniciadi);
                var model = db.Sepet.Where(x => x.kullaniciid == kullanici.id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.kullaniciid == kullanici.id);
                if (model != null)
                {
                    if(kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün Bulunmamaktadır";
                    }
                    else if (kid!=null)
                    {
                        Tutar = db.Sepet.Where(x => x.kullaniciid == kid.kullaniciid).Sum(x => x.Urun.fiyat * x.adet);
                        ViewBag.Tutar = "Toplam Tutar=" + Tutar + "TL";

                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullanicadi = User.Identity.Name;
                var model = db.Kullanici.FirstOrDefault(x=>x.email == kullanicadi);
                var u = db.Urun.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x => x.kullaniciid == model.id && x.urunid == id);
                if (model!=null)
                {
                    if(sepet!=null)
                    {
                        sepet.adet++;
                        sepet.fiyat = u.fiyat * sepet.adet;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                        kullaniciid = model.id,
                        urunid = u.id,
                        adet = 1,
                        fiyat = u.fiyat,
                        tarih = DateTime.Now
                    };
                    db.Sepet.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            return HttpNotFound();
        }
        public ActionResult SepetCount(int?count)
        {
            if(User.Identity.IsAuthenticated)
            {
                var model = db.Kullanici.FirstOrDefault(x => x.email == User.Identity.Name);
                count = db.Sepet.Where(x=>x.kullaniciid == model.id).Count();
                ViewBag.count = count;
                if(count == 0)
                {
                    ViewBag.count = "";
                }
                return PartialView();

            }
            return HttpNotFound();
        }
        public ActionResult arttir(int id)
        {
            var model = db.Sepet.Find(id);
            model.adet++;
            model.fiyat = model.fiyat * model.fiyat;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult azalt(int id)
        {
            var model = db.Sepet.Find(id);
            if (model.adet==1)
            {
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            model.adet--;
            model.fiyat = model.fiyat *model.adet;
            db.SaveChanges() ;
            return RedirectToAction("Index");



        }
        public ActionResult Sil(int id)
        {
            var urun = db.Sepet.Where(x => x.id == id).FirstOrDefault();
            if (urun == null)
            {
                // Eğer ürün bulunamazsa, bir hata mesajı gösterin veya başka bir sayfaya yönlendirin
                return HttpNotFound(); // Veya istediğiniz bir hata mesajı veya sayfa yönlendirmesi ekleyin
            }

            db.Sepet.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HepsiniSil()
    {
        if (User.Identity.IsAuthenticated)
        {
            var kullaniciadi = User.Identity.Name;
            var kullanici = db.Kullanici.FirstOrDefault(x => x.email == kullaniciadi);
            
            // Kullanıcının sepetindeki tüm ürünleri bul
            var sepetUrunler = db.Sepet.Where(x => x.kullaniciid == kullanici.id).ToList();

            // Eğer kullanıcının sepetinde ürün varsa, hepsini silelim
            if (sepetUrunler != null && sepetUrunler.Any())
            {
                db.Sepet.RemoveRange(sepetUrunler);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        return HttpNotFound();
    }
}

    }
