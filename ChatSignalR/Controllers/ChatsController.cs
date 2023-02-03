using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChatSignalR.Controllers
{
    public class ChatsController : Controller
    {
        public static Dictionary<int, string> ChatListOptions = new Dictionary<int, string>()
        {
            {1, "Tecnología" },
            {2, "Deporte" },
            {3, "Noticias" }
        };


        public IActionResult ChatList()
        {
            return View(ChatListOptions);
        }

        public IActionResult Chat(int idChat)
        {
            return View("Chat", idChat);
        }
    }
}
