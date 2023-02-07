using ChatSignalR.Core.Entities;
using ChatSignalR.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatSignalR.Controllers
{
    public class ManageController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ManageController(IUsuarioRepository repositorio)
        {
            this._usuarioRepository = repositorio;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            Usuario usuario = this._usuarioRepository.LogInUsuario(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "No tienes credenciales correctas";
                return View();
            }
            else
            {
                //Debemos crear una identidad(name y role) y una principal dicha identidad 
                //se combinarla con la cookie de autenticación.
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimRole = new Claim(ClaimTypes.Role, usuario.Tipo);
                Claim claimIdUsuario = new Claim("IdUsuario", usuario.Id.ToString());
                Claim claimEmail = new Claim("EmailUsuario", usuario.Email);

                //Todo usuario puede contener una serie de características llamada claims.  Dichas características
                //podemos almacenarlas dentro de user para utilizarlas a lo largo de la ejecución de la aplicación
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(5)
                });

                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
