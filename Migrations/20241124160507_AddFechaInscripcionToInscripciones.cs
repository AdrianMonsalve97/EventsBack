using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaInscripcionToInscripciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInscripcion",
                table: "Inscripciones",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaInscripcion",
                table: "Inscripciones");
        }
    }
}
