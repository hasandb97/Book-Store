using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Book_Store.Models;
using PagedList;


namespace Book_Store.Areas.User.Controllers
{
    public class UserController : Controller
    {
        Bookshop_DBEntities db = new Bookshop_DBEntities();

        // GET: User/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserRegister()
        {
            return View("UserRegister");
        }

        [HttpPost]
        public ActionResult UserRegister(string FullName, string PhoneNumber, string Email, string Password)
        {
            var AllUsers = db.TBL_User.Where(p => p.Phone == PhoneNumber || p.Email == Email).ToList();
            if(AllUsers.Count == 0)
            {
                DateTime Date = DateTime.Now;
                TBL_User User = new TBL_User()
                {
                    FullName = FullName,
                    Phone = PhoneNumber,
                    Email = Email,
                    Password = Password,
                    User_Status = 2,
                    RegisterDate = Date,
                    User_Isactive = 1,
                };

                db.TBL_User.Add(User);
                db.SaveChanges();
                ViewBag.RegsiterCompleted = "شما با موفقیت عضو شدید";
                return View("UserLogin");
            }
            else
            {
                ViewBag.RepetitiveUser = "ایمیل یا شماره تلفن وارد شده، قبلا ثبت شده است!";
                return View("UserRegister");
            }
        }



        public ActionResult AdminRegister()
        {
            return View("AdminRegister");
        }

        [HttpPost]
        public ActionResult AdminRegister(string FullName, string PhoneNumber, string Email, string Password)
        {
            var AllUsers = db.TBL_User.Where(p => p.Phone == PhoneNumber || p.Email == Email).ToList();
            if (AllUsers.Count == 0)
            {
                DateTime Date = DateTime.Now;
                TBL_User User = new TBL_User()
                {
                    FullName = FullName,
                    Phone = PhoneNumber,
                    Email = Email,
                    Password = Password,
                    User_Status = 1,
                    RegisterDate = Date,
                    User_Isactive = 0,
                };

                db.TBL_User.Add(User);
                db.SaveChanges();
                ViewBag.RegsiterCompleted = "اطلاعات شما با موفقیت ثبت شد و منتظر تایید مدیر اصلی می باشد.";
                return View("UserLogin");
            }
            else
            {
                ViewBag.RepetitiveUser = "ایمیل یا شماره تلفن وارد شده، قبلا ثبت شده است!";
                return View("AdminRegister");
            }
        }











        public ActionResult Shopinghistory( int id)
        {
            var Myorders = db.TBL_Order.Where(p => p.Userid == id && p.Isfinally == 1).ToList();
            return View(Myorders);

        }








        public ActionResult Addtocart(int id)
        {
            if (Session["UserLogined"]!="True")
            {
                return RedirectToAction("UserLogin");
            }
            int Userid = Convert.ToInt32(Session["UserID"]);
            var Product = db.TBL_Product.FirstOrDefault(p => p.Product_ID == id);

            var Order = db.TBL_Order.FirstOrDefault(p => p.Userid == Userid && p.Userpayment == 0);
            if (Order != null)
            {
                var Orderdetail = db.TBL_Orderdetail.FirstOrDefault(p => p.Orderid == Order.Orderid && p.Productid == Product.Product_ID);
                if (Orderdetail != null)
                {
                    Orderdetail.Count += 1;
                }
                else
                {
                    TBL_Orderdetail neworderdetail = new TBL_Orderdetail()
                    {
                        Orderid = Order.Orderid,
                        Productid = Product.Product_ID,
                        Price = Product.Price,
                        Count = 1,
                    };
                    db.TBL_Orderdetail.Add(neworderdetail);
                }
            }
            else
            {
                TBL_Order neworder = new TBL_Order()
                {
                    Userid = Userid,
                    Isfinally = 0,
                    Userpayment = 0,
                };
                db.TBL_Order.Add(neworder);
                db.SaveChanges();

                TBL_Orderdetail neworderdetail = new TBL_Orderdetail()
                {
                    Orderid = neworder.Orderid,
                    Productid = Product.Product_ID,
                    Price = Product.Price,
                    Count = 1,
                };
                db.TBL_Orderdetail.Add(neworderdetail);
                db.SaveChanges();

            }

            db.SaveChanges();



            return RedirectToAction("Mycart", new { id = Userid });
        }


        public ActionResult Deleteonepro(int id)
        {
            int Userid = Convert.ToInt32(Session["UserID"]);
            var temp = db.TBL_Orderdetail.FirstOrDefault(p => p.Detailid == id && p.TBL_Order.Userid == Userid);
            if( temp.Count <= 1)
            {
                db.TBL_Orderdetail.Remove(temp);
                db.SaveChanges();
            }
            else
            {
                temp.Count -= 1;
                db.SaveChanges();
            }
            return RedirectToAction("Mycart", new { @id = Userid });
        }


        public ActionResult Addonetopro(int id)
        {
            int Userid = Convert.ToInt32(Session["UserID"]);
            var temp = db.TBL_Orderdetail.FirstOrDefault(p => p.Detailid == id && p.TBL_Order.Userid == Userid);
            if( ((temp.TBL_Product.Quantity)- temp.Count )> 0 )
            {
                temp.Count += 1;
            }
            
            db.SaveChanges();
            return RedirectToAction("Mycart", new { @id = Userid });
        }


        public ActionResult Deletewholepro( int id)
        {
            int Userid = Convert.ToInt32(Session["UserID"]);
            var temp = db.TBL_Orderdetail.FirstOrDefault(p => p.Detailid == id && p.TBL_Order.Userid == Userid);
            db.TBL_Orderdetail.Remove(temp);
            db.SaveChanges();
            return RedirectToAction("Mycart", new { @id = Userid });
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Payment(int id , string State ,string City, string Customeraddress, string Customerphone)
        {
            var myorder = db.TBL_Order.FirstOrDefault(p => p.Userid == id && p.Userpayment == 0);
            myorder.City = City;
            myorder.State = State;
            myorder.Phone = Customerphone;
            myorder.Address = Customeraddress;
            myorder.Userpayment = 1;

            ///sending email
            var receiver = myorder.TBL_User.Email;
            var senderEmail = new MailAddress("hasandb1997@gmail.com", "فروشگاه کتاب نادری");
            var receiverEmail = new MailAddress(receiver, "Receiver");
            var password = "09396954157";
            var sub = "اطلاعیه ثبت درخواست سفارش";
            var body = "سفارش شما ثبت شد و در اسرع وقت توسط ادمین بررسی شده و گزارش آن به شما ایمیل می شود..";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = "سفارش جدید ",
                Body = body
            })
            {
                smtp.Send(mess);
            }

            var orderdetails = db.TBL_Orderdetail.Where( p => p.Orderid == myorder.Orderid).ToList();

            foreach( var temp in orderdetails)
            {
                
                temp.TBL_Product.Quantity=temp.TBL_Product.Quantity - temp.Count;
                db.SaveChanges();
            }
           
            db.SaveChanges();
            return RedirectToAction("Mycart", new { id = id });
        }













        public ActionResult Mycart(int id)
        {
            var myorders = db.TBL_Orderdetail.Where(p => p.TBL_Order.Userid == id && p.TBL_Order.Userpayment == 0).ToList();
            foreach( var order in myorders)
            {
                if(  order.TBL_Product.Quantity == 0)
                {
                    db.TBL_Orderdetail.Remove(order);
                }
                if( order.TBL_Product.Quantity < order.Count)
                {
                     order.Count = order.TBL_Product.Quantity;
                }
                db.SaveChanges();
            }
            var myordersfinall = db.TBL_Orderdetail.Where(p => p.TBL_Order.Userid == id && p.TBL_Order.Userpayment == 0).ToList();
            ViewBag.Userid = Session["UserID"];
            return View(myordersfinall);

        }

        public ActionResult Shoppingstatus( int id)
        {
            var Myorders = db.TBL_Order.OrderByDescending(p =>p.Orderid).Where(p => p.Userid == id && p.Userpayment == 1 && p.Isfinally==0).ToList();
            return View(Myorders);
        }




        public ActionResult Ordertosubmit()
        {

            var orders = db.TBL_Order.OrderByDescending(p => p.Orderid).Where(p => p.Isfinally == 0 && p.Userpayment == 1);
            return View(orders);
        }

        

        public ActionResult Submittedorders()
        {
            var orders = db.TBL_Order.OrderByDescending(p => p.Orderid).Where(p => p.Isfinally == 1 && p.Userpayment == 1);
            return View(orders);
        }

        public ActionResult Submitorder(int id)
        {
            var order = db.TBL_Order.FirstOrDefault(p => p.Orderid == id);
            order.Isfinally = 1;



                    var receiver = order.TBL_User.Email;
                    var senderEmail = new MailAddress("hasandb1997@gmail.com", "فروشگاه کتاب نادری");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "09396954157";
                    var sub = "تایید خرید";
                    var body = "خرید شما توسط ادمین تایید شد و در اسرع وقت برای شما ارسال میگردد.";
                     var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = "تایید خرید",
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                
           

            db.SaveChanges();
            return RedirectToAction("Ordertosubmit");
        }

        public ActionResult Showorderdetailtoadmin( int id)
        {
            var orderdetail = db.TBL_Orderdetail.Where(p => p.Orderid == id).ToList();
            var order = db.TBL_Order.First(p => p.Orderid == id);
            ViewBag.order = order;
            return View(orderdetail);
        }
      


        public ActionResult Deleteorder( int id)
        {
            var order = db.TBL_Orderdetail.Where(p => p.Orderid==id).ToList();
            foreach( var temp in order)
            {
                db.TBL_Orderdetail.Remove(temp);
            }
            var mainorder = db.TBL_Order.FirstOrDefault(p => p.Orderid == id);
            db.TBL_Order.Remove(mainorder);
            db.SaveChanges();
            return RedirectToAction("Submittedorders");
        }
        public ActionResult Adminlogin()
        {
            if (Session["AdminLogined"] == "True")
            {
                return RedirectToAction("AdminPanel", new { id = Session["Adminid"] });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Adminlogin(string Phone, string Password)
        {
            if (Session["AdminLogined"] == "True")
            {
                int Adminid = Convert.ToInt32(Session["Adminid"]);
                RedirectToAction("Adminpanel", new { id=Adminid});
            }
            var User = db.TBL_User.Where(p => p.Phone == Phone && p.Password == Password).ToList();
            if (User.Count == 0)
            {
                ViewBag.LoginError = "اطلاعات وارد شده نادرستند";
                return View("Adminlogin");
            }
            else
            {
                if (User[0].User_Status == 1 && User[0].User_Isactive == 1)
                {
                    Session["AdminLogined"]="True";
                    Session["Adminid"] = User[0].User_ID;
                    return RedirectToAction("AdminPanel", new { id = User[0].User_ID });
                }
                else
                {
                    return View("Adminlogin");
                }
            }
        }




       

        public ActionResult UserLogin()
        {
            if (Session["UserLogined"] == "True")
            {
                return RedirectToAction("UserPanel", new { id = Convert.ToInt32(Session["UserID"]) });
            }
            return View("UserLogin");
        }

        [HttpPost]
        public ActionResult UserLogin(string Phone , string Password)
        {
           
            var User = db.TBL_User.Where(p => p.Phone == Phone && p.Password == Password).ToList();
            if (User.Count == 0)
            {
                ViewBag.LoginError = "اطلاعات وارد شده نادرستند";
                return View("UserLogin");
            }
            else
            {
               
                 if(User[0].User_Status == 2)
                {
                    Session["UserLogined"] = "True";
                    Session["UserID"]=User[0].User_ID;
                    return RedirectToAction("UserPanel", new { id = User[0].User_ID });
                }
                else
                {
                    ViewBag.AccountSuspend = "حساب شما معلق می باشد";
                    return RedirectToAction("UserLogin");
                }

            }

        }


        public ActionResult UserPanel(int id)
        {
           var temp = db.TBL_User.First(p => p.User_ID == id);
           return View(temp);
        }
        public ActionResult AdminPanel(int id)
        {
            var temp = db.TBL_User.Single(p => p.User_ID == id);
            return View(temp);
        }



        public ActionResult AdminLogout()
        {
            Session["AdminLogined"] = "False";
            Session["UserLogined"] = "False";
            return RedirectToAction("UserLogin");

        }

        public ActionResult Showallusers(int page = 1, int pagesize = 10)
        {
            List<TBL_User> users = db.TBL_User.OrderByDescending( p =>p.User_ID).Where(p => p.User_Status == 2).ToList(); 
            PagedList<TBL_User> model = new PagedList<TBL_User>(users, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);
        }

        public ActionResult Showactiveuser(int page = 1, int pagesize = 10)
        {
            List<TBL_User> users = db.TBL_User.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 2 && p.User_Isactive==1).ToList();
            PagedList<TBL_User> model = new PagedList<TBL_User>(users, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);
        }

        public ActionResult Shownotavtiveuser(int page = 1, int pagesize = 10)
        {
            List<TBL_User> users = db.TBL_User.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 2 && p.User_Isactive == 0).ToList();
            PagedList<TBL_User> model = new PagedList<TBL_User>(users, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);
        }

        public ActionResult Showalladmin(int page = 1, int pagesize = 10)
        {
            List<TBL_User> users = db.TBL_User.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 1 && p.FullName != "admin").ToList();
            PagedList<TBL_User> model = new PagedList<TBL_User>(users, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);
        }
        


        public ActionResult Activeuser( int id )
        {
            var user = db.TBL_User.First(p => p.User_ID == id);
            if( user.User_Isactive == 0 )
            {
                user.User_Isactive = 1;
            }
            else
            {
                user.User_Isactive = 0;
            }
            db.SaveChanges();
            if( user.User_Status == 1)
            {
                return RedirectToAction("Showalladmin", "User");
            }
            else
            {
                return RedirectToAction("Showallusers", "User");
            }
        }

        public ActionResult Deleteuser(int id)
        {
            var user = db.TBL_User.First( p => p.User_ID==id);
            db.TBL_User.Remove(user);
            db.SaveChanges();
            if (user.User_Status == 1)
            {
                return RedirectToAction("Showalladmin", "User");
            }
            else
            {
                return RedirectToAction("Showallusers", "User");
            }
        }

        public ActionResult Showactiveadmin(int page = 1, int pagesize = 10)
        {
            List<TBL_User> admins = db.TBL_User.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 1 && p.User_Isactive==1 && p.FullName!="admin").ToList();
            PagedList<TBL_User> model = new PagedList<TBL_User>(admins, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);
            
        }

        public ActionResult Shownotactivedadmin(int page = 1, int pagesize = 10)
        {
            List<TBL_User> admins = db.TBL_User.OrderByDescending(p => p.User_ID).Where(p => p.User_Status == 1 && p.User_Isactive ==0 && p.FullName != "admin").ToList();
            PagedList<TBL_User> model = new PagedList<TBL_User>(admins, page, pagesize);
            var AllUsers = db.TBL_User.ToList();
            return View(model);

        }
        



    }
}