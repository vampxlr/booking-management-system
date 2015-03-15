using SimchaMVC.Areas.Admin.Controllers.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace SimchaMVC.Areas.MembershipTest.Controllers
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

       

        [HttpGet]
      
        public ActionResult Login()
        {
            if (WebSecurity.IsAuthenticated) { Session["UserName"] = WebSecurity.CurrentUserName; Response.Redirect("~/MembershipTest/home/index"); }
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {


            if (form["UserName"] == null || form["UserName"] == "")
            {
                ViewBag.ErrorMsg = "Enter User Name";
                return View();
            }
            else if (form["UserName"] != null || form["UserName"] != "")
            {
                string TxtNewAlbumvalue = form["UserName"];
                if (TxtNewAlbumvalue == string.Empty)
                {

                    ViewBag.ErrorMsg = "Enter User Name";
                    return View();
                }
            }

            string uname = form["UserName"];

            if (form["Password"] == null)
            {
                ViewBag.ErrorMsg = "Enter password";
                return View();
            }
            else if (form["Password"] != null)
            {
                string TxtNewAlbumvalue1 = form["Password"];
                if (TxtNewAlbumvalue1 == string.Empty)
                {

                    ViewBag.ErrorMsg = "Enter password";
                    return View();
                }
            }







            bool success = WebSecurity.Login(form["UserName"], form["Password"], false);
            if (success)
            {

            


                       

                if (form["rememberMe"] == "on")
                {
                    
                    // Clear any other tickets that are already in the response
                    Response.Cookies.Clear();

                    // Set the new expiry date - to thirty days from now
                    DateTime expiryDate = DateTime.Now.AddDays(14);

                    // Create a new forms auth ticket
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, form["UserName"], DateTime.Now, expiryDate, true, String.Empty);
                    
                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create a new authentication cookie - and set its expiration date
                    HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    authenticationCookie.Expires = ticket.Expiration;
                    Session.Timeout = 1440;
                    // Add the cookie to the response.
                    Response.Cookies.Add(authenticationCookie);

                }
                


                string returnUrl = Request.QueryString["ReturnUrl"];
                if (returnUrl == null)
                {
                    Response.Redirect("~/MembershipTest/home/index");
                }
                else
                {
                    Response.Redirect(returnUrl);
                }
            }
            return View();
        }



        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            Response.Redirect("~/MembershipTest/home/login");
            return View();
        }


    }
}
