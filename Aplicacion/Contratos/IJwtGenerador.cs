using Dominio;

namespace Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
         string CreatToken(Usuario usuario);
    }
}