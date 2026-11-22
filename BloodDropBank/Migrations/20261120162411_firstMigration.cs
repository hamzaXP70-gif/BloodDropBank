using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodDropBank.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeightKg = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.CheckConstraint("CK_Users_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                });

            migrationBuilder.CreateTable(
                name: "BloodInventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    UnitsAvailable = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodInventories", x => x.InventoryId);
                    table.CheckConstraint("CK_BloodInventories_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                    table.CheckConstraint("CK_BloodInventories_UnitsAvailable", "[UnitsAvailable] BETWEEN 0 AND 10000");
                    table.ForeignKey(
                        name: "FK_BloodInventories_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalId");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequests",
                columns: table => new
                {
                    BloodRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    UnitsNeeded = table.Column<int>(type: "int", nullable: false),
                    RemainingUnits = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MedicalCondition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodRequests", x => x.BloodRequestId);
                    table.CheckConstraint("CK_BloodRequests_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                    table.CheckConstraint("CK_BloodRequests_RemainingUnits", "[RemainingUnits] >= 0 AND [RemainingUnits] <= [UnitsNeeded]");
                    table.CheckConstraint("CK_BloodRequests_Status", "[Status] BETWEEN 1 AND 4");
                    table.CheckConstraint("CK_BloodRequests_UnitsNeeded", "[UnitsNeeded] BETWEEN 1 AND 40");
                    table.ForeignKey(
                        name: "FK_BloodRequests_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalId");
                    table.ForeignKey(
                        name: "FK_BloodRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "InventoryDonations",
                columns: table => new
                {
                    InventoryDonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodRequestId = table.Column<int>(type: "int", nullable: false),
                    BloodInventoryId = table.Column<int>(type: "int", nullable: false),
                    UnitsDonated = table.Column<int>(type: "int", nullable: false),
                    DonatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDonations", x => x.InventoryDonationId);
                    table.CheckConstraint("CK_InventoryDonations_UnitsDonated", "[UnitsDonated] BETWEEN 1 AND 40");
                    table.ForeignKey(
                        name: "FK_InventoryDonations_BloodInventories_BloodInventoryId",
                        column: x => x.BloodInventoryId,
                        principalTable: "BloodInventories",
                        principalColumn: "InventoryId");
                    table.ForeignKey(
                        name: "FK_InventoryDonations_BloodRequests_BloodRequestId",
                        column: x => x.BloodRequestId,
                        principalTable: "BloodRequests",
                        principalColumn: "BloodRequestId");
                });

            migrationBuilder.CreateTable(
                name: "UserDonations",
                columns: table => new
                {
                    UserDonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    BloodRequestId = table.Column<int>(type: "int", nullable: true),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    UnitDonated = table.Column<int>(type: "int", nullable: false),
                    UserDonationType = table.Column<int>(type: "int", nullable: false),
                    DonationStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDonations", x => x.UserDonationId);
                    table.CheckConstraint("CK_UserDonations_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                    table.CheckConstraint("CK_UserDonations_DonationStatus", "[DonationStatus] BETWEEN 1 AND 3");
                    table.CheckConstraint("CK_UserDonations_StatusAndDate", "([DonationStatus] = 2 AND [DonatedAt] IS NOT NULL) OR ([DonationStatus] IN (1, 3) AND [DonatedAt] IS NULL)");
                    table.CheckConstraint("CK_UserDonations_TypeAndRequest", "([UserDonationType] = 1 AND [BloodRequestId] IS NULL) OR ([UserDonationType] = 2 AND [BloodRequestId] IS NOT NULL)");
                    table.CheckConstraint("CK_UserDonations_UnitDonated", "[UnitDonated] = 1");
                    table.CheckConstraint("CK_UserDonations_UserDonationType", "[UserDonationType] BETWEEN 1 AND 2");
                    table.ForeignKey(
                        name: "FK_UserDonations_BloodRequests_BloodRequestId",
                        column: x => x.BloodRequestId,
                        principalTable: "BloodRequests",
                        principalColumn: "BloodRequestId");
                    table.ForeignKey(
                        name: "FK_UserDonations_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalId");
                    table.ForeignKey(
                        name: "FK_UserDonations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_PhoneNumber",
                table: "Admins",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventories_HospitalId_BloodGroup",
                table: "BloodInventories",
                columns: new[] { "HospitalId", "BloodGroup" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_HospitalId",
                table: "BloodRequests",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_UserId",
                table: "BloodRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Email",
                table: "Hospitals",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_PhoneNumber",
                table: "Hospitals",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDonations_BloodInventoryId",
                table: "InventoryDonations",
                column: "BloodInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDonations_BloodRequestId",
                table: "InventoryDonations",
                column: "BloodRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDonations_BloodRequestId",
                table: "UserDonations",
                column: "BloodRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDonations_HospitalId",
                table: "UserDonations",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDonations_UserId_DonationStatus_DonatedAt",
                table: "UserDonations",
                columns: new[] { "UserId", "DonationStatus", "DonatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NationalId",
                table: "Users",
                column: "NationalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "InventoryDonations");

            migrationBuilder.DropTable(
                name: "UserDonations");

            migrationBuilder.DropTable(
                name: "BloodInventories");

            migrationBuilder.DropTable(
                name: "BloodRequests");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
