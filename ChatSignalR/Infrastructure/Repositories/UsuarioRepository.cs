using ChatSignalR.Core.Entities;
using ChatSignalR.Core.Interfaces;
using ChatSignalR.Helpers;
using ChatSignalR.Infrastructure.Context;

namespace ChatSignalR.Infrastructure.Repositories
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UsuarioRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public int GetMaxIdUsuario()
        {
            if (this._applicationDbContext.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this._applicationDbContext.Usuarios.Max(z => z.Id) + 1;
            }
        }

        public bool ExisteEmail(string email)
        {
            var consulta = from datos in this._applicationDbContext.Usuarios
                           where datos.Email == email
                           select datos;
            if (consulta.Count() > 0)
            {
                //El email existe en la base de datos
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos, string tipo)
        {
            bool ExisteEmail = this.ExisteEmail(email);
            if (ExisteEmail)
            {
                return false;
            }
            else
            {
                int idusuario = this.GetMaxIdUsuario();
                Usuario usuario = new Usuario();
                usuario.Id = idusuario;
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Apellidos = apellidos;
                usuario.Tipo = tipo;
                //Se Genera un salt aleatorio para cada usuario
                usuario.Salt = HelperCryptography.GenerateSalt();
                //Se genera su password con el salt
                usuario.Password = HelperCryptography.EncriptarPassword(password, usuario.Salt);
                this._applicationDbContext.Usuarios.Add(usuario);
                this._applicationDbContext.SaveChanges();
                return true;
            }

        }

        public Usuario LogInUsuario(string email, string password)
        {
            Usuario usuario = this._applicationDbContext.Usuarios.SingleOrDefault(x => x.Email == email);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                /*Debemos comparar con la base de datos el password haciendo de
                nuevo el cifrado con cada salt de usuario*/
                byte[] passUsuario = usuario.Password;
                string salt = usuario.Salt;
                //Ciframos de nuevo para comparar
                byte[] temporal = HelperCryptography.EncriptarPassword(password, salt);
                //Comparamos los arrays para comprobar si el cifrado es el mismo
                bool respuesta = HelperCryptography.compareArrays(passUsuario, temporal);
                if (respuesta == true)
                {
                    return usuario;
                }
                else
                {
                    //Contraseña incorrecta
                    return null;
                }
            }
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this._applicationDbContext.Usuarios
                           select datos;
            return consulta.ToList();
        }

        public Dictionary<int, string> ChatListOptions()
        {
            return this._applicationDbContext.SalasChat.ToDictionary(x => x.Id, y => y.Nombre);
        }
    }
}
