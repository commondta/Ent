namespace HRMS_Web.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionId = context.HttpContext.Session.GetString("ID");

            if (string.IsNullOrEmpty(sessionId))
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Session is not available or user is not authenticated." });
                return;
            }

            base.OnActionExecuting(context);
        }
    }

}
