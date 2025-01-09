using EventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsuario(modelBuilder);
            ConfigureEvento(modelBuilder);
            ConfigureInscripcion(modelBuilder);
        }

        // Configuración de la entidad Usuario
        private void ConfigureUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Usuarios");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(50);

                // Relaciones
                entity.HasMany(u => u.EventosCreados)
                    .WithOne(e => e.UsuarioCreador)
                    .HasForeignKey(e => e.UsuarioCreadorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Usuarios_Eventos");

                entity.HasMany(u => u.Inscripciones)
                    .WithOne(i => i.Usuario)
                    .HasForeignKey(i => i.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Usuarios_Inscripciones");
            });
        }

        // Configuración de la entidad Evento
        private void ConfigureEvento(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Eventos");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FechaHora)
                    .IsRequired();

                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CapacidadMaxima)
                    .IsRequired();

                entity.Property(e => e.AsistentesRegistrados)
                    .IsRequired();

                entity.Property(e => e.UsuarioCreadorNombre)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasOne(e => e.UsuarioCreador)
                    .WithMany(u => u.EventosCreados)
                    .HasForeignKey(e => e.UsuarioCreadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }

        // Configuración de la entidad Inscripcion
        protected void ConfigureInscripcion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.ToTable("Inscripciones");

                entity.Property(i => i.FechaInscripcion)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()"); 

                entity.HasOne(i => i.Usuario)
                      .WithMany(u => u.Inscripciones)
                      .HasForeignKey(i => i.UsuarioId)
                      .HasConstraintName("FK_Inscripciones_Usuarios");

                entity.HasOne(i => i.Evento)
                      .WithMany(e => e.Inscripciones)
                      .HasForeignKey(i => i.EventoId)
                      .HasConstraintName("FK_Inscripciones_Eventos");
            });
        }

    }
}
