using PagedList;
using stoktakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stoktakip.Controllers
{
    public class TumSatislarController : Controller
    {
        Takip_SistemiEntities db = new Takip_SistemiEntities();
        [Authorize(Roles = "A")]
        public ActionResult Index(int sayfa=1)
        {
            return View(db.Satislar.ToList().ToPagedList(sayfa,5));
        }
    }
}