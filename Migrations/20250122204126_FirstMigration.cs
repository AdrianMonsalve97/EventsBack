using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsApi.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    IdentificacionEmpresa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NombreContactoEmpresa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroContatoEmpresa = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdentificacionEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdentificacionProveedor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreProveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IdentificacionProveedor);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CelularPersonal = table.Column<long>(type: "bigint", nullable: false),
                    CelularCorporativo = table.Column<long>(type: "bigint", nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    DocumentoIdentidad = table.Column<long>(type: "bigint", nullable: false),
                    CorreoCorporativo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorreoPersonal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rol = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    FechaContratoInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaContratoFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpresaIndentificacionEmpresa = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_EmpresaIndentificacionEmpresa",
                        column: x => x.EmpresaIndentificacionEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdentificacionEmpresa");
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: false),
                    AsistentesRegistrados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Prioridad = table.Column<int>(type: "int", nullable: false),
                    Fechas_FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fechas_FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fechas_FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fechas_FechaCotizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fechas_FechaAprovacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreadorNombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios",
                        column: x => x.UsuarioCreadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Eventos",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Usuarios",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioCreadorId",
                table: "Eventos",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_EventoId",
                table: "Inscripciones",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_UsuarioId",
                table: "Inscripciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaIndentificacionEmpresa",
                table: "Usuarios",
                column: "EmpresaIndentificacionEmpresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
