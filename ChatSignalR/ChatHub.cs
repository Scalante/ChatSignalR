using Microsoft.AspNetCore.SignalR;

namespace ChatSignalR
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int room, string user, string mensaje)
        {
            //Enviamos un mensaje de forma asincrona a un grupo determinado
            await Clients.Group(room.ToString()).SendAsync("RecibirMensaje", user, mensaje);
        }
        public async Task AddToGroup(string room)
        {
            //Añadimos a la persona a la sala
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
    }
}
