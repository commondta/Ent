using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class registrationNoProfile_update_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_RegistrationNoProfile_RegistrationNoProfileId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_RegNoProfileAttachments_RegistrationNoProfile_RegistrationNoProfileId",
                table: "RegNoProfileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SoftLock_RegistrationNoProfile_RegistrationNoProfileId",
                table: "SoftLock");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "SoftLock",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "RegNoProfileAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "Alerts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_RegistrationNoProfile_RegistrationNoProfileId",
                table: "Alerts",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegNoProfileAttachments_RegistrationNoProfile_RegistrationNoProfileId",
                table: "RegNoProfileAttachments",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoftLock_RegistrationNoProfile_RegistrationNoProfileId",
                table: "SoftLock",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_RegistrationNoProfile_RegistrationNoProfileId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_RegNoProfileAttachments_RegistrationNoProfile_RegistrationNoProfileId",
                table: "RegNoProfileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SoftLock_RegistrationNoProfile_RegistrationNoProfileId",
                table: "SoftLock");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "SoftLock",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "RegNoProfileAttachments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationNoProfileId",
                table: "Alerts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_RegistrationNoProfile_RegistrationNoProfileId",
                table: "Alerts",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegNoProfileAttachments_RegistrationNoProfile_RegistrationNoProfileId",
                table: "RegNoProfileAttachments",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftLock_RegistrationNoProfile_RegistrationNoProfileId",
                table: "SoftLock",
                column: "RegistrationNoProfileId",
                principalTable: "RegistrationNoProfile",
                principalColumn: "Id");
        }
    }
}
