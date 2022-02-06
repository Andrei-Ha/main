using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exadel.OfficeBooking.EF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    EmailAdress = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EmailSubject = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MessageBody = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    SendDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Adress = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    IsCityCenter = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsParkingAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FloorNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsKitchenPresent = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMeetingRoomPresent = table.Column<bool>(type: "INTEGER", nullable: false),
                    OfficeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsBooked = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsNextToWindow = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasPC = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasMonitor = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasKeyboard = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasMouse = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasHeadset = table.Column<bool>(type: "INTEGER", nullable: false),
                    MapId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workplaces_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TelegramId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    EmploymentStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EmploymentEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrefferedId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Workplaces_PrefferedId",
                        column: x => x.PrefferedId,
                        principalTable: "Workplaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecuringBooking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecuringBooking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecuringBooking_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vacations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VacationStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VacationEnd = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkplaceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecuringBookingId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_RecuringBooking_RecuringBookingId",
                        column: x => x.RecuringBookingId,
                        principalTable: "RecuringBooking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsBooked = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlaceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    OfficeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookingId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingPlaces_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParkingPlaces_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "EmploymentEnd", "EmploymentStart", "FirstName", "LastName", "PrefferedId", "TelegramId" },
                values: new object[] { new Guid("8c8807d4-134f-4c38-99a3-a28b31c032d8"), "iivanov@gmail.com", null, new DateTime(2022, 2, 6, 10, 43, 57, 560, DateTimeKind.Local).AddTicks(5717), "Ivan", "Ivanov", null, 123465 });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RecuringBookingId",
                table: "Bookings",
                column: "RecuringBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WorkplaceId",
                table: "Bookings",
                column: "WorkplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_OfficeId",
                table: "Maps",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_BookingId",
                table: "ParkingPlaces",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_OfficeId",
                table: "ParkingPlaces",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecuringBooking_UserId",
                table: "RecuringBooking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrefferedId",
                table: "Users",
                column: "PrefferedId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_UserId",
                table: "Vacations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workplaces_MapId",
                table: "Workplaces",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ParkingPlaces");

            migrationBuilder.DropTable(
                name: "Vacations");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "RecuringBooking");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workplaces");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
