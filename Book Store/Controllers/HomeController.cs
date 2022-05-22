using Book_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Book_Store.Controllers
{
    public class HomeController : Controller
    {
        Bookshop_DBEntities db = new Bookshop_DBEntities();


        public ActionResult Index()
        {
            var latespro = db.TBL_Product.OrderByDescending(p => p.Product_ID).Where(p => p.Quantity > 0 && p.Status==1).Take(10);
            ViewBag.latespro = latespro;

            return View();
        }



        public ActionResult Searchproduct(string Name)
        {
            var Products = db.TBL_Product.Where( p=>p.Name.Contains(Name)).ToList();
            var category = db.TBL_Category.ToList();

            List<TBL_Category> children = new List<TBL_Category>();


            string Cat(int Parent_id)
            {
                children = category.Where(p => p.Parent_ID == Parent_id).ToList();
                if (children.Count == 0)
                {
                    return "";
                }
                string S = "";
                foreach (var test in children)
                {
                    S += "<li><a href='/Home/Productlist/" + test.Cate_ID + "'>" + test.Name + " (" + test.TBL_Product.Count + ")</a></li>";
                    if (Cat(test.Cate_ID) != "")
                    {
                        S += "<ul>" + Cat(test.Cate_ID) + "</ul>";

                    }

                }

                return S;

            }
            string mycat = Cat(0);
            ViewBag.Mycat = mycat;
            return View(Products);
        }


        public ActionResult Allproducts(int page = 1, int pagesize = 10)
        {
            List<TBL_Product> Allpro = db.TBL_Product.OrderByDescending(p => p.Product_ID).Where(p => p.Quantity > 0 && p.Status == 1).ToList();
            PagedList<TBL_Product> model = new PagedList<TBL_Product>(Allpro, page, pagesize);
            var category = db.TBL_Category.ToList();

            List<TBL_Category> children = new List<TBL_Category>();


            string Cat(int Parent_id)
            {
                children = category.Where(p => p.Parent_ID == Parent_id).ToList();
                if (children.Count == 0)
                {
                    return "";
                }
                string S = "";
                foreach (var test in children)
                {
                    S += "<li><a href='/Home/Productlist/" + test.Cate_ID + "'>" + test.Name + " (" + test.TBL_Product.Count + ")</a></li>";
                    if (Cat(test.Cate_ID) != "")
                    {
                        S += "<ul>" + Cat(test.Cate_ID) + "</ul>";

                    }

                }

                return S;

            }
            string mycat = Cat(0);
            ViewBag.Mycat = mycat;

            return View(model);

        }




        public ActionResult Productlist(int id,int page = 1, int pagesize = 10)
        {
            List<TBL_Product> Allpro = db.TBL_Product.OrderByDescending(p => p.Product_ID).Where(p => p.Quantity > 0 && p.Status == 1 && p.Category_ID==id).ToList();
            PagedList<TBL_Product> model = new PagedList<TBL_Product>(Allpro, page, pagesize);
            var category = db.TBL_Category.ToList();

            List<TBL_Category> children = new List<TBL_Category>();


            string Cat(int Parent_id)
            {
                children = category.Where(p => p.Parent_ID == Parent_id).ToList();
                if (children.Count == 0)
                {
                    return "";
                }
                string S = "";
                foreach (var test in children)
                {
                    S += "<li><a href='/Home/Productlist/" + test.Cate_ID + "'>" + test.Name + " (" + test.TBL_Product.Count + ")</a></li>";
                    if (Cat(test.Cate_ID) != "")
                    {
                        S += "<ul>" + Cat(test.Cate_ID) + "</ul>";

                    }

                }

                return S;

            }
            string mycat = Cat(0);
            ViewBag.Mycat = mycat;

            return View(model);

        }


































        public ActionResult Aboutus()
        {
            return View();
        }
        public ActionResult Blog()
        {
            ViewBag.Message = "Your contact page.";

            return View("Blog");
        }
        public ActionResult ShowProduct()
        {
            return View("ShowProduct");
        }
    }
}