using Microsoft.EntityFrameworkCore;
using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions options)
        : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey( ci => new{ ci.InstructorId, ci.CursoId});
        } 

        public DbSet<Curso> curso {get; set;}

        public DbSet<Comentario>  comentario {get; set;}

        public DbSet<Instructor> instructor { get; set; }

        public DbSet<CursoInstructor> cursoInstructor { get; set; }

        public DbSet<Precio> precio { get; set; }
    }
}