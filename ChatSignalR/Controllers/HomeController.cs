using ChatSignalR.Core.Entities;
using ChatSignalR.Core.Interfaces;
using ChatSignalR.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public HomeController(IUsuarioRepository repositorio)
        {
            this._usuarioRepository = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(string email, string password, string nombre, string apellidos, string tipo)
        {
            bool registrado = this._usuarioRepository.RegistrarUsuario(email, password, nombre, apellidos, tipo);
            if (registrado)
            {
                ViewData["MENSAJE"] = "Usuario registrado con éxito";
            }
            else
            {
                ViewData["MENSAJE"] = "Error al registrar el usuario";
            }
            return View();
        }


        /// <summary>
        /// Método protegido, el cliente que desee obtener la información
        /// deberá iniciar sesión en la aplicación y además, tener el rol de ADMIN
        /// </summary>
        /// <returns></returns>
        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        public IActionResult AdminUsuarios()
        {
            List<Usuario> usuarios = this._usuarioRepository.GetUsuarios();
            return View(usuarios);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}