using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
public class ActionsAttribute : ActionFilterAttribute
{
    Stopwatch? watch;
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        watch?.Stop();
        
        Action("OnActionExecuted", filterContext.RouteData);
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Action("OnActionExecuting", filterContext.RouteData);
        watch= Stopwatch.StartNew();
    }
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        Action("OnResultExecuted", filterContext.RouteData);
    }
     public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        Action("OnResultExecuting", filterContext.RouteData);
    }
    private void Action(string methodName, RouteData routeData)
    {
        var controllerName= routeData.Values["controller"];
        var actionName= routeData.Values["action"];
        var message= methodName +" -Controller:"+ controllerName+",Action:"+actionName+",Time of Use:"+watch?.ElapsedMilliseconds.ToString()+"\n";
        Console.WriteLine(message);
    }

}