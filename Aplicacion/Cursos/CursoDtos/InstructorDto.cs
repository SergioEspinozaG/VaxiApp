using System;

namespace Aplicacion.Cursos
{
    public class InstructorDto
    {
        public Guid InstructorId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Grados { get; set; }
        public byte[] FotoPerfil { get; set; }

        public DateTime? FechaDeCreacion { get; set; }
    }
}