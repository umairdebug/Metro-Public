using Metro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Metro.Handlers
{
    public class CustomAuthorized : ActionFilterAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        private List<string> RolesList = new();
        private bool IsAuthorized { get; set; } = false;
        public void OnAuthorization(AuthorizationFilterContext context)
        
        {
            RolesList = (Roles ?? "").Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var db = (AppDbContext)context.HttpContext.RequestServices.GetService(typeof(AppDbContext));
            var logInUser = db.GetLogInUser();
            if (logInUser != null) 
            {
                if(RolesList.Any())
                {
                    foreach (var item in RolesList) 
                    {
                        foreach(var userRole in logInUser.Roles)
                        {
                            if(item ==  userRole)
                            {
                                IsAuthorized = true;
                            }
                            if (IsAuthorized) break;
                        }
                        if (IsAuthorized) break;
                    }
                }
                else
                {
                    IsAuthorized = true;
                }
                
            }
            if (!IsAuthorized) 
            {
                context.Result = new RedirectResult("~/LogIn");
            }
            
        }
    }
}
