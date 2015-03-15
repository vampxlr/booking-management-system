using System.Web.Mvc;

namespace SimchaMVC.Areas.MembershipTest
{
    public class MembershipTestAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MembershipTest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MembershipTest_default",
                "MembershipTest/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}