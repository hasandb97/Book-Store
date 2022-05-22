using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Book_Store.Models;
using PagedList;


namespace Book_Store.Areas.Book.Controllers
{
    public class BookController : Controller
    {
        Bookshop_DBEntities db = new Bookshop_DBEntities();

        public string ProDetails(int id=13)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            string res = temp.Name + temp.Thumbnail + temp.TBL_Writer.FullName; 
            return res;
        }

        public List<int> Numbers(int id = 13)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            List<int> numbers = new List<int>();
            numbers.Add(Convert.ToInt32(temp.Price));
            numbers.Add(Convert.ToInt32(temp.Shabak));
            numbers.Add(Convert.ToInt32(temp.Weight));
            return numbers;

        }
        
        public List<int> CartID()
        {
            List<int> Result = new List<int>();
            if(Request.Cookies["CartID"] != null)
            {
                var temp = Request.Cookies["CartID"].Value.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                Result = temp;
            }
            return Result;
        }

        public List<int> CartValue()
        {
            List<int> Result = new List<int>();
            if (Request.Cookies["CartValue"] != null)
            {
                var test = Request.Cookies["CartValue"].Value.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                Result = test;
            }
            return Result;
        }


        // GET: Book/Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewComment( int id , string Txt )
        {
            int Userid = Convert.ToInt32(Session["UserID"]);
            TBL_Comments newcomment = new TBL_Comments()
            {
                Comment_Content = Txt,
                Product_ID = id,
                User_ID = Userid,
                Comment_Status = 0,
            };
           
           
            db.TBL_Comments.Add(newcomment);
            db.SaveChanges();
            return RedirectToAction("ShowProduct", "Book", new { id = id });
        }

        public ActionResult ShowProduct(int id)
        {
            var Book = db.TBL_Product.First( p=> p.Product_ID == id);
            var Comments = db.TBL_Comments.Where(p => p.Product_ID == id && p.Comment_Status==1 ).ToList();
            ViewBag.Comments = Comments;
            
            return View(Book);
        }


        public ActionResult Showallcomments(int page = 1, int pagesize = 10)
        {
            List<TBL_Comments> comments = db.TBL_Comments.OrderByDescending(p => p.Comment_ID).ToList();
            PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(comments, page, pagesize);
            var Allproduct = db.TBL_Comments.ToList();
            return View(model);
        }


        public ActionResult Publishcomment(int id)
        {
            var comment = db.TBL_Comments.First( p => p.Comment_ID == id);
            if( comment.Comment_Status ==1 )
            {
                comment.Comment_Status = 0;
            }
            else
            {
                comment.Comment_Status = 1;
            }
            db.SaveChanges();
            return RedirectToAction("Showallcomments","Book");
        }


        public ActionResult Showallactivecomments(int page = 1, int pagesize = 10)
        {

            List<TBL_Comments> comments = db.TBL_Comments.OrderByDescending(p => p.Product_ID).Where(p => p.Comment_Status == 1).ToList();
            PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(comments, page, pagesize);
            var Allproduct = db.TBL_Comments.ToList();
            return View(model);

        }

        public ActionResult Showalltoactivecomments(int page = 1, int pagesize = 10)
        {

            List<TBL_Comments> comments = db.TBL_Comments.OrderByDescending(p => p.Product_ID).Where(p => p.Comment_Status == 1).ToList();
            PagedList<TBL_Comments> model = new PagedList<TBL_Comments>(comments, page, pagesize);
            var Allproduct = db.TBL_Comments.ToList();
            return View(model);

        }


        public ActionResult Deletecomment( int id)
        {
            var comment = db.TBL_Comments.First( p => p.Comment_ID==id);
            db.TBL_Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Showallcomments", "Book");
        }

        /// Show All Products list

        public ActionResult Showallproduct(int page = 1, int pagesize = 10)
        {
            List<TBL_Product> Products = db.TBL_Product.OrderByDescending(p => p.Product_ID).ToList();
            PagedList<TBL_Product> model = new PagedList<TBL_Product>(Products, page, pagesize);
            var Allproduct = db.TBL_Product.ToList();
            return View(model);
        }

        
        public ActionResult ShowPublishedProduct(int page = 1, int pagesize = 10)
        {
            List<TBL_Product> Products = db.TBL_Product.Where(p => p.Status == 1).OrderByDescending(p => p.Product_ID).ToList();
            PagedList<TBL_Product> model = new PagedList<TBL_Product>(Products, page, pagesize);
            var PublishedProduct = db.TBL_Product.ToList();
            return View(model);
        }

        public ActionResult ShowUnpublishedProduct(int page = 1, int pagesize = 10)
        {
            List<TBL_Product> Products = db.TBL_Product.Where(p => p.Status == 0).OrderByDescending(p => p.Product_ID).ToList();
            PagedList<TBL_Product> model = new PagedList<TBL_Product>(Products, page, pagesize);
            var PublishedProduct = db.TBL_Product.ToList();
            return View(model);
        }

        public ActionResult Unpublishproduct2( int id)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            temp.Status = 0;
            db.SaveChanges();
            return RedirectToAction("ShowPublishedProduct", "Book");
        }


        public ActionResult Publishproduct(int id)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            temp.Status = 1;
            db.SaveChanges();
            return RedirectToAction("Showallproduct","Book");
        }

        public ActionResult Unpublishproduct(int id)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            temp.Status = 0;
            db.SaveChanges();
            return RedirectToAction("Showallproduct", "Book");
        }

        public ActionResult Deleteproduct(int id)
        {
            var comments = db.TBL_Comments.Where(p => p.Product_ID == id);
            foreach( var test in comments)
            {
                db.TBL_Comments.Remove(test);
            }
            var temp = db.TBL_Product.First( p => p.Product_ID == id );
            db.TBL_Product.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("Showallproduct", "Book");

        }

















        public ActionResult NewBook()
        {
           
            var Writer = db.TBL_Writer.ToList();
            var Cat = db.TBL_Category.ToList();
            ViewBag.Cat = Cat;
            var Entesharat = db.TBL_Entesharat.ToList();
            ViewBag.Entesharat = Entesharat;
            var Translator = db.TBL_Translator.ToList();
            ViewBag.Translator = Translator;
            
            return View(Writer);
        }

        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult NewBook(string BookName, string Shabak, string Writer, string Translator, string Entesharat, HttpPostedFileBase Fileup, HttpPostedFileBase Fileup1, HttpPostedFileBase Fileup2, string Category, string Price, string Subject, string ChapYear, string Pages, string ShomareChap, string Lenght, string Width, string Height, string Weight, string mytextarea)
        public ActionResult NewBook(string BookName, string Shabak, string Writer,string Translator,string Entesharat , HttpPostedFileBase Fileup, string Category,string Price,string amount,string Discount , string Subject,string ChapYear,string Pages,string ShomareChap,string Lenght,string Width, string Height,string Weight,string mytextarea)
        {
            Random rnd = new Random();
            int num = rnd.Next(100000);

            TBL_Product MyBook = new TBL_Product()
            {
                Name = BookName,
                Shabak = Convert.ToInt32(Shabak),
                Writer_ID = Convert.ToInt32(Writer),
                Motarjem = Convert.ToInt32(Translator),
                Entersharat = Convert.ToInt32(Entesharat),
                Category_ID = Convert.ToInt32(Category),
                Price = Convert.ToInt32(Price),
                Subject = Subject,
                ChapYear = Convert.ToInt32(ChapYear),
                Pages = Convert.ToInt32(Pages),
                ShomareChap = Convert.ToInt32(ShomareChap),
                Lenght = Convert.ToInt32(Lenght),
                Width = Convert.ToInt32(Width),
                Height = Convert.ToInt32(Height),
                Weight = Convert.ToInt32(Weight),
                Summery = mytextarea,
                Quantity = Convert.ToInt32(amount),
                Meta_Title = BookName,
                Code = num,
                Discount_ID = Convert.ToInt32(Discount),
                Status = 0,
                Image = Guid.NewGuid() + Path.GetExtension(Fileup.FileName),
            };

          



            string MyPath = Server.MapPath("~/Content/img/Products");
            if (!Directory.Exists(MyPath))
            {
                Directory.CreateDirectory(MyPath);
            }
            string filename = MyPath + "/" + MyBook.Image;
            Fileup.SaveAs(MyPath + "/" + MyBook.Image);

            string thumbnail = "/thumb_" + MyBook.Image;
            WebImage img = new WebImage(MyPath + "/" + MyBook.Image);
            img.FileName = thumbnail;
            string thumbnail_path = MyPath + thumbnail;
            img.Resize(100, 100);
            img.Save(thumbnail_path);
            MyBook.Thumbnail = thumbnail;
            db.TBL_Product.Add(MyBook);
            db.SaveChanges();
            return RedirectToAction("ProductAdded");
        }

        public ActionResult ProductAdded()
        {
            return View();
        }


        public ActionResult Newwriter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Newwriter(string Writername , string Nationalityradio , string Nationality , string Aboutwriter)
        {
            int IsIranian;
            if( Nationalityradio == "Yes")
            {
                IsIranian = 1;
            }
            else
            {
                IsIranian = 0;
            }
            TBL_Writer Writer = new TBL_Writer()
            {
                FullName = Writername,
                Nationality = Nationality,
                Iranian = IsIranian,
                Writer_Content = Aboutwriter,
            };
            db.TBL_Writer.Add(Writer);
            db.SaveChanges();
            return RedirectToAction("Showallwriters");
        }



      

        public ActionResult Showallwriters(int page = 1, int pagesize = 10)
        {
            List<TBL_Writer> users = db.TBL_Writer.OrderByDescending(p => p.ID).ToList();
            PagedList<TBL_Writer> model = new PagedList<TBL_Writer>(users, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);

        }
        

        public ActionResult Deletewriter(int id)
        {
            var Writer = db.TBL_Writer.FirstOrDefault(p => p.ID == id);
            db.TBL_Writer.Remove(Writer);
            db.SaveChanges();
            return RedirectToAction("Showallwriters");
        }

        public ActionResult Editwriter( int id)
        {
            var Writer = db.TBL_Writer.FirstOrDefault(p => p.ID == id);
            return View(Writer);
        }
        [HttpPost]
        public ActionResult Editwriter(string id ,string Writername, string Nationalityradio, string Nationality, string Aboutwriter)
        {
            int Wid = Convert.ToInt32(id);
            var Writer = db.TBL_Writer.FirstOrDefault(p => p.ID == Wid);
            Writer.Nationality = Nationality;
            Writer.FullName = Writername;
            Writer.Writer_Content = Aboutwriter;
            if (Nationalityradio == "Yes")
            {
                Writer.Iranian = 1;
            }
            else
            {
                Writer.Iranian = 0;
            }
            db.SaveChanges();

            return RedirectToAction("Showallwriters");
        }


        public ActionResult Editprodct(int id)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            var Category = db.TBL_Category.ToList();
            var Writer = db.TBL_Writer.ToList();
            var Translator = db.TBL_Translator.ToList();
            var Entesharat = db.TBL_Entesharat.ToList();

            ViewBag.Category = Category;
            ViewBag.Writer = Writer;
            ViewBag.Translator = Translator;
            ViewBag.Entesharat = Entesharat;
            return View(temp);
        }

        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult NewBook(string BookName, string Shabak, string Writer, string Translator, string Entesharat, HttpPostedFileBase Fileup, HttpPostedFileBase Fileup1, HttpPostedFileBase Fileup2, string Category, string Price, string Subject, string ChapYear, string Pages, string ShomareChap, string Lenght, string Width, string Height, string Weight, string mytextarea)
        public ActionResult Editprodct(string BookName, String ID, string Shabak, string Writer, string Translator,string Entesharat, HttpPostedFileBase Fileup, string Category, string Price, string amount,string Discount, string Subject, string ChapYear, string Pages, string ShomareChap, string Lenght, string Width, string Height, string Weight, string mytextarea)
        {
            int id = Convert.ToInt32(ID);
            var temp = db.TBL_Product.First(p => p.Product_ID == id);
            temp.Name = BookName;
            temp.Shabak = Convert.ToInt32(Shabak);
            temp.Writer_ID = Convert.ToInt32(Writer);
            temp.Motarjem = Convert.ToInt32(Translator);
            temp.Entersharat = Convert.ToInt32(Entesharat);
            temp.Category_ID = Convert.ToInt32(Category);
            temp.Price = Convert.ToInt32(Price);
            temp.Subject = Subject;
            temp.ChapYear = Convert.ToInt32(ChapYear);
            temp.Pages = Convert.ToInt32(Pages);
            temp.ShomareChap = Convert.ToInt32(ShomareChap);
            temp.Lenght = Convert.ToInt32(Lenght);
            temp.Width = Convert.ToInt32(Width);
            temp.Height = Convert.ToInt32(Height);
            temp.Weight = Convert.ToInt32(Weight);
            temp.Summery = mytextarea;
            temp.Quantity = Convert.ToInt32(amount);
            temp.Discount_ID = Convert.ToInt32(Discount);
            temp.Meta_Title = BookName;
            if( Fileup != null)
            {
                temp.Image = Guid.NewGuid() + Path.GetExtension(Fileup.FileName);
                string MyPath = Server.MapPath("~/Content/img/Products");
                if (!Directory.Exists(MyPath))
                {
                    Directory.CreateDirectory(MyPath);
                }
                string filename = MyPath + "/" + temp.Image;
                Fileup.SaveAs(MyPath + "/" + temp.Image);

                string thumbnail = "/thumb_" + temp.Image;
                WebImage img = new WebImage(MyPath + "/" + temp.Image);
                img.FileName = thumbnail;
                string thumbnail_path = MyPath + thumbnail;
                img.Resize(100, 100);
                img.Save(thumbnail_path);
                temp.Thumbnail = thumbnail;
                db.SaveChanges();
                return RedirectToAction("Showallproduct","Book");

            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Showallproduct", "Book");
            }



        }

        
           
        

        public ActionResult AddToCart(int ID)
        {
            var temp = db.TBL_Product.First(p => p.Product_ID == ID);
            List<int> TheID = new List<int>();
            List<int> TheAmount = new List<int>();

            TheID = CartID();  /// Select all id of Cart
            TheAmount = CartValue();   /// Select all values of Cart product

            /*


            num.Add(10);
            num.Add(20);
            num.Add(30);

            List<int> Mylist = new List<int>();
            Mylist = MyCart();

            var yourListString = String.Join(",", num);

            HttpCookie yourListCookie = new HttpCookie("YourList", yourListString);
            yourListCookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(yourListCookie);


            Response.Cookies["MYLIS"]["name"] = yourListString;



            Response.Cookies["Product"]["ID"] = Convert.ToString(temp.Code);
            Response.Cookies["Product"]["Name"] = temp.Name;
            Response.Cookies["Product"].Expires = DateTime.Now.AddHours(1);

            */
            return RedirectToAction("ShowProduct", new { id = ID});
        }



    }
}