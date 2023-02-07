using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatSignalR.Helpers
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Los usuarios son almacenados dentro de httpcontext y dentro de user, un usuario está
            //compuesto por una identidad, podemos saber el nombre del usuario o si esta autenticado
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                //Se redirecciona la petición a la vista del login si el usuario no está atenticado.
                RouteValueDictionary rutalogin = new RouteValueDictionary(new
                {
                    controller = "Manage",
                    action = "Login"
                });
                RedirectToRouteResult result = new RedirectToRouteResult(rutalogin);
                context.Result = result;
            }
        }
    }
}
