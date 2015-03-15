using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimchaMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class HashgachotController : Controller
    {
         SimchaDB DB = new SimchaDB();

         [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<hashgacha_images> HG = DB.hashgacha_images.OrderBy(r=>r.order_by).ToList();
           
             return View(HG);
        }

         [HttpPost]
         public ActionResult Index(hashgacha_images HG)
         {
             if (ModelState.IsValid)
             {
                 DB.Entry(HG).State = EntityState.Modified;
                 DB.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(HG);
         }

         [HttpGet]
         public ActionResult NewIndex()
         {
            

             return View();
         }

         public ActionResult Delete(int id)
         {
             hashgacha_images HG = DB.hashgacha_images.Find(id);
             DB.hashgacha_images.Remove(HG);
             DB.SaveChanges();

             if (DB.caterers_hashgachot.Any(r => r.hashgacha_id == id))
             {
                 IEnumerable<caterers_hashgachot> ToRemove = DB.caterers_hashgachot.Where(r => r.hashgacha_id == id).ToList();

                 foreach (var item in ToRemove)
                 {
                     DB.caterers_hashgachot.Remove(item);
                     DB.SaveChanges();

                 }
             }


             return RedirectToAction("Index");
         }



         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult AddNewRecord(hashgacha_images HG)
         {
             var file = Request.Files[0];

             var fileNameForDB = Path.GetFileName(file.FileName);
             var extensionForDB = Path.GetExtension(fileNameForDB);
             
             if (file != null && file.ContentLength > 0)
             {
                
                 var fileName = Path.GetFileName(file.FileName);
                 var extension = Path.GetExtension(fileName);

                 var path = Path.Combine(Server.MapPath("~/Content/thumbs"), fileName);
                 var pathwrite = Path.Combine(Server.MapPath("~/Content/thumbs"), fileName);
                 file.SaveAs(path);
                 

             }

             HG.hashgacha_image = fileNameForDB;
             
             if (ModelState.IsValid)
             {
                 DB.hashgacha_images.Add(HG);
                 DB.SaveChanges();
                 return RedirectToAction("Index");
             }

             return View(HG);
         }

    }
}
