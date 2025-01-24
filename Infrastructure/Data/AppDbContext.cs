using EventsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Empresa> Empresas { get; set; } // DbSet para Empresa

        // Configuración de las entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsuario(modelBuilder);
            ConfigureEvento(modelBuilder);
            ConfigureInscripcion(modelBuilder);
            ConfigureProveedor(modelBuilder);
            ConfigureEmpresa(modelBuilder); // Llamada para configurar la entidad Empresa
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

                entity.Property(e => e.CelularPersonal)
                    .IsRequired();

                entity.Property(e => e.CelularCorporativo)
                    .IsRequired();

                entity.Property(e => e.TipoDocumento)
                    .IsRequired();

                entity.Property(e => e.DocumentoIdentidad)
                    .IsRequired();

                entity.Property(e => e.CorreoCorporativo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CorreoPersonal)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(50);

                // Configuración de fechas
                entity.Property(e => e.FechaContratoInicio);

                entity.Property(e => e.FechaContratoFin);
                
                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(255); 

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
                    .HasMaxLength(500);

                entity.Property(e => e.FechaHora);

                entity.Property(e => e.Ubicacion)
                    .HasMaxLength(200);

                entity.Property(e => e.CapacidadMaxima)
                    .IsRequired();

                entity.Property(e => e.AsistentesRegistrados)
                    .HasDefaultValue(0);

                entity.Property(e => e.Prioridad)
                    .IsRequired();

                // Configuración de FechasEventos como tipo propio
                entity.OwnsOne(e => e.Fechas, fechas =>
                {
                    fechas.Property(f => f.FechaInicio).IsRequired();
                    fechas.Property(f => f.FechaFin).IsRequired();
                    fechas.Property(f => f.FechaAsignacion).IsRequired();
                    fechas.Property(f => f.FechaCotizacion).IsRequired();
                    fechas.Property(f => f.FechaAprovacion).IsRequired();
                });

                // Relaciones
                entity.HasOne(e => e.UsuarioCreador)
                    .WithMany(u => u.EventosCreados)
                    .HasForeignKey(e => e.UsuarioCreadorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Eventos_Usuarios");
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

        // Configuración de la entidad Proveedor
        protected void ConfigureProveedor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(p => p.IdentificacionProveedor);

                // Definir la tabla correspondiente
                entity.ToTable("Proveedores");

                // Configuración de propiedades
                entity.Property(p => p.IdentificacionProveedor)
                    .IsRequired()
                    .HasMaxLength(20) // Longitud máxima
                    .HasColumnName("IdentificacionProveedor");

                entity.Property(p => p.NombreProveedor)
                    .IsRequired()
                    .HasMaxLength(100) // Longitud máxima
                    .HasColumnName("NombreProveedor");

                entity.Property(p => p.TipoDocumento)
                    .IsRequired()
                    .HasConversion<int>(); // Convertir el enum a int
            });
        }

        // Configuración de la entidad Empresa
        protected void ConfigureEmpresa(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empresa>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IndentificacionEmpresa);

                // Definir la tabla correspondiente
                entity.ToTable("Empresas");

                // Configuración de propiedades
                entity.Property(e => e.IndentificacionEmpresa)
                    .IsRequired()
                    .HasMaxLength(20) // Longitud máxima
                    .HasColumnName("IdentificacionEmpresa");

                entity.Property(e => e.NombreEmpresa)
                    .IsRequired()
                    .HasMaxLength(100) // Longitud máxima
                    .HasColumnName("NombreEmpresa");

                entity.Property(e => e.TipoDocumento)
                    .IsRequired()
                    .HasConversion<int>(); // Convertir el enum a int

                entity.Property(e => e.NombreContactoEmpresa)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("NombreContactoEmpresa");

                entity.Property(e => e.NumeroContatoEmpresa)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("NumeroContatoEmpresa");
            });
        }
    }
}
