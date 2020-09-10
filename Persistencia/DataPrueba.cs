using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertData(CursosOnlineContext context, UserManager<Usuario> usuarioManager){
            if(!usuarioManager.Users.Any()){
                var usuario = new Usuario{
                    NombreCompleto = "Sergio Espinoza",
                    UserName = "chechx17",
                    Email = "sergio@hotmail.com",
                };

                await usuarioManager.CreateAsync(usuario, "Password123$");
                
            }

        }
    }
}