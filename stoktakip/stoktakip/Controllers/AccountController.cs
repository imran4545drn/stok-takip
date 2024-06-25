using stoktakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace stoktakip.Controllers
{
    public class AccountController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult Login(Kullanici p)
        {
            var bilgiler = db.Kullanici.FirstOrDefault(x => x.email == p.email && x.sifre == p.sifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.email, false);
                Session["email"] = bilgiler.email.ToString();
                Session["ad"] = bilgiler.ad.ToString();
                Session["soyad"] = bilgiler.soyad.ToString();
                return RedirectToAction("Index", "Home"); // Yönlendirme "Home" ve "Index" action'ına
            }
            else
            {
                ViewBag.hata = "Kullanıcı adı veya şifre hatalı";
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Kullanici data)
        {
            db.Kullanici.Add(data);
            data.rol = "U";
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Guncelle()
        {
            var kullanicilar = (string)Session["mail"];
            var degerler = db.Kullanici.FirstOrDefault(x => x.email == kullanicilar);
            if (degerler == null)
            {
                return HttpNotFound();
            }
            return View(degerler);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(Kullanici data)
        {
            if (ModelState.IsValid)
            {
                var kullanicilar = (string)Session["mail"];
                var user = db.Kullanici.FirstOrDefault(x => x.email == kullanicilar);
                if (user != null)
                {
                    user.ad = data.ad;
                    user.soyad = data.soyad;
                    user.email = data.email;
                    user.kullaniciad = data.kullaniciad;
                    user.sifre = data.sifre;
                    user.sifretekrar = data.sifretekrar;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(data);
        }
        public ActionResult imran()
        {
            var kullanicilar = (string)Session["mail"];
            var degerler = db.Kullanici.FirstOrDefault(x => x.email == kullanicilar);
            if (degerler == null)
            {
                return HttpNotFound();
            }
            return View(degerler);
          
        }
        [HttpPost]
        public ActionResult imran(Kullanici data)
        {
            if (ModelState.IsValid)
            {
                var kullanicilar = (string)Session["mail"];
                var user = db.Kullanici.FirstOrDefault(x => x.email == kullanicilar);
                if (user != null)
                {
                    user.ad = data.ad;
                    user.soyad = data.soyad;
                    user.email = data.email;
                    user.kullaniciad = data.kullaniciad;
                    user.sifre = data.sifre;
                    user.sifretekrar = data.sifretekrar;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(data);
        }
    }
    
}