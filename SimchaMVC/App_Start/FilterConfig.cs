using System.Web;
using System.Web.Mvc;

namespace SimchaMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }


    }

    public class LoginActionFilter : ActionFilterAttribute, IActionFilter
{
    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)


    {
        // TODO: Add your acction filter's tasks here

        // Log Action Filter Call
       
        this.OnActionExecuting(filterContext);
    }
}

}