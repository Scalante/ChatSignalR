using ChatSignalR.Core.Entities;

namespace ChatSignalR.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        public int GetMaxIdUsuario();
        public bool ExisteEmail(string email);
        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos, string tipo);
        public Usuario LogInUsuario(string email, string password);
        public List<Usuario> GetUsuarios();
        public Dictionary<int, string> ChatListOptions();
    }
}
