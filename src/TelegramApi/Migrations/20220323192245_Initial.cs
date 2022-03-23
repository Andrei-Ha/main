using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exadel.OfficeBooking.TelegramApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkplaceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkplaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    RecurringWeekDays = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    IsParkingPlace = table.Column<bool>(type: "bit", nullable: false),
                    ParkingPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOnlyFirstFree = table.Column<bool>(type: "bit", nullable: false),
                    IsKitchenPresent = table.Column<bool>(type: "bit", nullable: false),
                    IsMeetingRoomPresent = table.Column<bool>(type: "bit", nullable: false),
                    IsNextToWindow = table.Column<bool>(type: "bit", nullable: false),
                    IsVIP = table.Column<bool>(type: "bit", nullable: false),
                    HasPC = table.Column<bool>(type: "bit", nullable: false),
                    HasMonitor = table.Column<bool>(type: "bit", nullable: false),
                    HasKeyboard = table.Column<bool>(type: "bit", nullable: false),
                    HasMouse = table.Column<bool>(type: "bit", nullable: false),
                    HasHeadset = table.Column<bool>(type: "bit", nullable: false),
                    NextStep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Propositions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallbackMessageId = table.Column<int>(type: "int", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOfficeReportSelected = table.Column<bool>(type: "bit", nullable: false),
                    EditTypeEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookView",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    UserStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookView_UserStates_UserStateId",
                        column: x => x.UserStateId,
                        principalTable: "UserStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginUserDto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginUserDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginUserDto_UserStates_UserStateId",
                        column: x => x.UserStateId,
                        principalTable: "UserStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookView_UserStateId",
                table: "BookView",
                column: "UserStateId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginUserDto_UserStateId",
                table: "LoginUserDto",
                column: "UserStateId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookView");

            migrationBuilder.DropTable(
                name: "LoginUserDto");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
