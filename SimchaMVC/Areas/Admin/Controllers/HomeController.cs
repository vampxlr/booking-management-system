
using System;

using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;
using WebMatrix.WebData;

namespace SimchaMVC.Areas.Admin.Controllers
{
                         
    public class HomeController : Controller
    {
   
       
      
        
        //
        // GET: /Admin/Dashboard/
     
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
           
            return View();
        }



        //
        // GET: /Admin/Dashboard/

  


        [HttpGet]

        public ActionResult Login()
        {
            if (!WebSecurity.IsAuthenticated) return View();
            Session["UserName"] = WebSecurity.CurrentUserName; Response.Redirect("~/Admin/home/index");

            return View();
        }

        [HttpPost]
       
        public ActionResult Login(FormCollection form)
        {


            if (form["UserName"] == null || form["UserName"] == "")
            {
                ViewBag.ErrorMsg = "Enter User Name";
                return View();
            }
            else if (form["UserName"] != null || form["UserName"] != "")
            {
                var TxtNewAlbumvalue = form["UserName"];
                if (TxtNewAlbumvalue == string.Empty)
                {

                    ViewBag.ErrorMsg = "Enter User Name";
                    return View();
                }
            }

            

            if (form["Password"] == null)
            {
                ViewBag.ErrorMsg = "Enter password";
                return View();
            }
             if (form["Password"] != null)
            {
                var TxtNewAlbumvalue1 = form["Password"];
                if (TxtNewAlbumvalue1 == string.Empty)
                {

                    ViewBag.ErrorMsg = "Enter password";
                    return View();
                }
            }







            var success = WebSecurity.Login(form["UserName"], form["Password"]);
            if (success)
            {


                if (form["rememberMe"] == "on" || true)
                {

                    // Clear any other tickets that are already in the response
                    Response.Cookies.Clear();

                    // Set the new expiry date - to thirty days from now
                    var expiryDate = DateTime.Now.AddDays(14);

                    // Create a new forms auth ticket
                    var ticket = new FormsAuthenticationTicket(2, form["UserName"], DateTime.Now, expiryDate, true, String.Empty);

                    // Encrypt the ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create a new authentication cookie - and set its expiration date
                    var authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        Expires = ticket.Expiration
                    };

                    // Add the cookie to the response.
                    Response.Cookies.Add(authenticationCookie);

                }



                var returnUrl = Request.QueryString["ReturnUrl"];
                Response.Redirect(returnUrl ?? "~/Admin/home/index");
            }
            else {
                ViewBag.ErrorMsg = "Wrong Credentials";
                return View();
            }
            return View();
        }



        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            Response.Redirect("~/Admin/home/login");
            return View();
        }


    }
}
