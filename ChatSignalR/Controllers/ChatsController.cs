using ChatSignalR.Core.Interfaces;
using ChatSignalR.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ChatSignalR.Controllers
{
    public class ChatsController : Controller
    {
        protected readonly IUsuarioRepository _usuarioRepository;

        public ChatsController(IUsuarioRepository usuarioRepositorio)
        {
            this._usuarioRepository = usuarioRepositorio;
        }

        [AuthorizeUsers(Policy = "ADMINISTRADORES")]
        public IActionResult ChatList()
        {
            return View(_usuarioRepository.ChatListOptions());
        }

        public IActionResult Chat(int idChat)
        {
            var nombreSalaSeleccionada = _usuarioRepository.ChatListOptions().GetValueOrDefault(idChat);
            ViewData["NombreSalaChat"] = nombreSalaSeleccionada;
            return View("Chat", idChat);
        }
    }
}
