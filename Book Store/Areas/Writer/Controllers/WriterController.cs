using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Book_Store.Models;
using PagedList;


namespace Book_Store.Areas.Writer.Controllers
{
    public class WriterController : Controller
    {
        Bookshop_DBEntities db = new Bookshop_DBEntities();
        // GET: Writer/Writer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddWriter()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddWriter(string Name , string Nationality , string BirthDay , string DeathTime , HttpPostedFileBase Fileup, string mytextarea)
        {
            TBL_Writer Writer = new TBL_Writer()
            {
                FullName = Name,
                Nationality = Nationality,
                Birthday = Convert.ToInt32(BirthDay),
                Deathdate = Convert.ToInt32(DeathTime),
                Writer_Content = mytextarea,
                Image = Guid.NewGuid() + Path.GetExtension(Fileup.FileName),
            };
            string MyPath = Server.MapPath("~/Content/img/Writers");
            if (!Directory.Exists(MyPath))
            {
                Directory.CreateDirectory(MyPath);
            }
            string filename = MyPath + "/" + Writer.Image;
            Fileup.SaveAs(MyPath + "/" + Writer.Image);
            string thumbnail = "/thumb_" + Writer.Image;
            WebImage img = new WebImage(MyPath + "/" + Writer.Image);
            img.FileName = thumbnail;
            string thumbnail_path = MyPath + thumbnail;
            img.Resize(100, 100);
            img.Save(thumbnail_path);

            Writer.Image_Thumbnail = thumbnail;

            db.TBL_Writer.Add(Writer);
            db.SaveChanges();
            ViewBag.WritreAdded = "نویسنده با موفقیت ثبت شد";

            return View();
        }


        public ActionResult ShowAllWriters(int page = 1, int pageSize = 2)
        {
            List<TBL_Writer> temp = db.TBL_Writer.OrderByDescending(p => p.ID).ToList();
            PagedList<TBL_Writer> model = new PagedList<TBL_Writer>(temp, page, pageSize);
            return View(model);
        }

        public ActionResult ChangeWriterStatus(int id)
        {
            var temp = db.TBL_Writer.First(p => p.ID == id);
            if(temp.Status == 0)
            {
                temp.Status = 1;
            }
            else
            {
                temp.Status = 0;
            }
            db.SaveChanges();
            return RedirectToAction("ShowAllWriters");
        }

        public ActionResult EditWriter(int id)
        {
            var temp = db.TBL_Writer.First(p => p.ID == id);
            return View(temp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditWriter(string TID, string Name, string Nationality, string BirthDay, string DeathTime, HttpPostedFileBase Fileup, string mytextarea)
        {
            int WID = Convert.ToInt32(TID);
            var temp = db.TBL_Writer.First(p => p.ID == WID);
            temp.FullName = Name;
            temp.Nationality = Nationality;
            temp.Birthday = Convert.ToInt32(BirthDay);
            temp.Deathdate = Convert.ToInt32(DeathTime);
            temp.Writer_Content = mytextarea;
            db.SaveChanges();
            return RedirectToAction("ShowAllWriters");
        }


        public ActionResult ShowWriterDetail(int id)
        {
            var temp = db.TBL_Writer.First(p => p.ID == id);
            return View(temp);
        }






    }
}