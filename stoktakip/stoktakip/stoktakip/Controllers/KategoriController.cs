using stoktakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace stoktakip.Controllers
{
    [Authorize(Roles = "A")]
    public class KategoriController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();

        public ActionResult Index()
        {
            return View(db.Kategori.Where(x => x.durum == true).ToList());
        }

        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(Kategori data)
        {
            if (data == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Geçersiz kategori verisi.");
            }

            data.durum = true;
            db.Kategori.Add(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sil(int id)
        {
            var kategori = db.Kategori.FirstOrDefault(x => x.id == id);
            if (kategori == null)
            {
                return HttpNotFound();
            }

            db.Kategori.Remove(kategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Guncelle(int id)
        {
            var kategori = db.Kategori.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        [HttpPost]
       
        public ActionResult Guncelle(Kategori data)
        {
            if (data == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Geçersiz kategori verisi.");
            }

            var kategori = db.Kategori.Find(data.id);
            if (kategori == null)
            {
                return HttpNotFound();
            }

            // Gelen veriyle gelen alanları güncelle
            kategori.ad = data.ad;
            kategori.aciklama = data.aciklama;
            kategori.durum = data.durum;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult KritikStok()
            { return View(); }

    }
}
