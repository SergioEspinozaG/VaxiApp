using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Instructor
    {
        public Guid InstructorId { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Grados { get; set; }
        public byte[] FotoPerfil { get; set; }

        public ICollection<CursoInstructor> CursoLink { get; set; }
    }
}