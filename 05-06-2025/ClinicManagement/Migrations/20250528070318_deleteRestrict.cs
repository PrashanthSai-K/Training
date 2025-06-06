using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagement.Migrations
{
    /// <inheritdoc />
    public partial class deleteRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocSpeciality_Doctor",
                table: "DoctorSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_DocSpeciality_Speciality",
                table: "DoctorSpecialities");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocSpeciality_Doctor",
                table: "DoctorSpecialities",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocSpeciality_Speciality",
                table: "DoctorSpecialities",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocSpeciality_Doctor",
                table: "DoctorSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_DocSpeciality_Speciality",
                table: "DoctorSpecialities");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocSpeciality_Doctor",
                table: "DoctorSpecialities",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocSpeciality_Speciality",
                table: "DoctorSpecialities",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
