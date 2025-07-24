using ForNewTest.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForNewTest.Contexto
{
    public class AplicationDBContext : IdentityDbContext
    {
        public AplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GeneroModel>(entity =>
            {
                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
                entity.Property(p => p.Nombre)
                .HasMaxLength(100);
            });
            modelBuilder.Entity<ActorModel>().Property(e => e.Nombre).HasMaxLength(150);
            modelBuilder.Entity<ActorModel>().Property(e => e.Foto).IsUnicode();

            modelBuilder.Entity<PeliculaModel>().Property(e => e.Titulo).HasMaxLength(150);
            modelBuilder.Entity<PeliculaModel>().Property(e => e.Poster).IsUnicode();

            modelBuilder.Entity<GeneroPelicula>().HasKey(e => new { e.PeliculaModelId, e.GeneroModelId });
            modelBuilder.Entity<ActorPelicula>().HasKey(e => new { e.PeliculaModelId, e.ActorModelId });

            modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogin");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");
        }

        public DbSet<GeneroModel> Generos { get; set; }
        public DbSet<ActorModel> Actores { get; set; }
        public DbSet<PeliculaModel> Peliculas { get; set; }
        public DbSet<ComentarioModel> Comentarios { get; set; }
        public DbSet<GeneroPelicula> GeneroPeliculas { get; set; }
        public DbSet<ActorPelicula> ActorPeliculas { get; set; }
        public DbSet<PersonaModel> Personas { get; set; }
        public DbSet<ErrorModel> Errores { get; set; }
    }
}
