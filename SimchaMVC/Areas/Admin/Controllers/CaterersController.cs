using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.Data;

namespace SimchaMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class CaterersController : Controller
    {

        SimchaDB DB = new SimchaDB();
        [HttpGet]
        public ActionResult Index()
        {

            var caterers = from s in DB.caterers
                        select s;
         
            ViewBag.Caterers = caterers;
            return View(caterers);


        }


        public ActionResult AddNewRecord()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRecord(caterer caterer)
        {
            if (ModelState.IsValid)
            {
                DB.caterers.Add(caterer);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caterer);
        }



        public ActionResult Edit(int id = 0)
        {
            caterer caterer = DB.caterers.Find(id);
            if (caterer == null)
            {
                return HttpNotFound();
            }
            return View(caterer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(caterer caterer)
        {
            if (ModelState.IsValid)
            {
                DB.Entry(caterer).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caterer);
        }





        //public ActionResult Delete(int id)
        //{
        //    caterer caterer = DB.caterers.Find(id);
        //    DB.caterers.Remove(caterer);
        //    DB.SaveChanges();

        //    if (DB.hall_caterers.Any(r => r.caterer_id == id))
        //    {
        //        IEnumerable<hall_caterers> caterersToRemove = DB.hall_caterers.Where(r => r.caterer_id == id).ToList();

        //        foreach (var item in caterersToRemove)
        //        {
        //            DB.hall_caterers.Remove(item);
        //            DB.SaveChanges();

        //        }
        //    }

        //    return RedirectToAction("Index");
        //}



         [HttpGet]
        public ActionResult CatererHashgachot(int id = 0)
        {
            IEnumerable<hashgacha_images> hashgacha_images = DB.hashgacha_images.ToList();
            IEnumerable<caterers_hashgachot> Allcaterers_hashgachot = DB.caterers_hashgachot.Where(h => h.caterer_id == id);

            IEnumerable<hashgacha_images> hashgachotCaterers = from xx in hashgacha_images
                                                join halC in Allcaterers_hashgachot on xx.id equals halC.hashgacha_id
                                                select xx;
            IEnumerable<hashgacha_images> NothashgachotCaterers = hashgacha_images.Except(hashgachotCaterers);
            ViewBag.NothashgachotCaterers = NothashgachotCaterers;
            return View(hashgachotCaterers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult CatererHashgachot(string caterer_id, string hashgacha_id)
         {
             caterers_hashgachot newcaterers_hashgachot = new caterers_hashgachot();
             newcaterers_hashgachot.caterer_id = Int32.Parse(caterer_id);
             newcaterers_hashgachot.hashgacha_id = Int32.Parse(hashgacha_id);

             if (ModelState.IsValid)
             {
                 DB.caterers_hashgachot.Add(newcaterers_hashgachot);
                 DB.SaveChanges();

             }



             int id = Int32.Parse(caterer_id);
             IEnumerable<hashgacha_images> hashgacha_images = DB.hashgacha_images.ToList();
             IEnumerable<caterers_hashgachot> Allcaterers_hashgachot = DB.caterers_hashgachot.Where(h => h.caterer_id == id);

             IEnumerable<hashgacha_images> hashgachotCaterers = from xx in hashgacha_images
                                                                join halC in Allcaterers_hashgachot on xx.id equals halC.hashgacha_id
                                                                select xx;
             IEnumerable<hashgacha_images> NothashgachotCaterers = hashgacha_images.Except(hashgachotCaterers);
             ViewBag.NothashgachotCaterers = NothashgachotCaterers;
             return View(hashgachotCaterers);
        }
        [HttpGet]
        public ActionResult RemoveHasgacha(int caterer_id, int hashgacha_id)
        {
            caterers_hashgachot oldcaterers_hashgachot;

            if (DB.caterers_hashgachot.Any(r => r.hashgacha_id == hashgacha_id && r.caterer_id == caterer_id))
            {
                 oldcaterers_hashgachot =
                    DB.caterers_hashgachot.FirstOrDefault(
                        r => r.hashgacha_id == hashgacha_id && r.caterer_id == caterer_id);


            }
            else
            {
                return RedirectToAction("CatererHashgachot", new { id = caterer_id });

            }

            if (ModelState.IsValid)
             {
                 DB.caterers_hashgachot.Remove(oldcaterers_hashgachot);
                 DB.SaveChanges();

             }
             return RedirectToAction("CatererHashgachot", new { id = caterer_id });

         }

        // public ActionResult deleteCaterer(int id, string caterer_id)
        //{
        //    int hashgachotId = id;
        //    int catererid = Convert.ToInt32(caterer_id);
        //    caterers_hashgachot caterers_hashgachot = DB.caterers_hashgachot.FirstOrDefault(r => r.hashgacha_id == hashgachotId && r.caterer_id == catererid);

        //    DB.caterers_hashgachot.Remove(caterers_hashgachot);
        //    DB.SaveChanges();
        //    return RedirectToAction("CatererHashgachot", new { id = caterer_id });
        //}


        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }
        



    }
}
