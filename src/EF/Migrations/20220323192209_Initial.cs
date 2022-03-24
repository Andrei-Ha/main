using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exadel.OfficeBooking.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsFreeParkingAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: false),
                    IsKitchenPresent = table.Column<bool>(type: "bit", nullable: false),
                    IsMeetingRoomPresent = table.Column<bool>(type: "bit", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "ParkingPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    PlaceNumber = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingPlaces_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsNextToWindow = table.Column<bool>(type: "bit", nullable: false),
                    HasPC = table.Column<bool>(type: "bit", nullable: false),
                    HasMonitor = table.Column<bool>(type: "bit", nullable: false),
                    HasKeyboard = table.Column<bool>(type: "bit", nullable: false),
                    HasMouse = table.Column<bool>(type: "bit", nullable: false),
                    HasHeadset = table.Column<bool>(type: "bit", nullable: false),
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EmploymentStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmploymentEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreferredId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Workplaces_PreferredId",
                        column: x => x.PreferredId,
                        principalTable: "Workplaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkplaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParkingPlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: true),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    RecurringWeekDays = table.Column<int>(type: "int", nullable: false),
                    BookingType = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_ParkingPlaces_ParkingPlaceId",
                        column: x => x.ParkingPlaceId,
                        principalTable: "ParkingPlaces",
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
                name: "Vacations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VacationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VacationEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Address", "City", "Country", "IsFreeParkingAvailable", "Name" },
                values: new object[,]
                {
                    { new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), "Naturalistov str # 3", "Minsk", "Belarus", true, "N3" },
                    { new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4"), "Mirzo Ulugbek Avenue # 73", "Tashkent", "Uzbekistan", true, "MU73" },
                    { new Guid("1d426867-997d-4aa8-a6ff-90c639e0a2d9"), "Solnechnaya str # 59", "Minsk", "Belarus", false, "S59" },
                    { new Guid("83ea853c-771b-4fe7-bd21-135ee936145e"), "Yaroslaviv Val # 15", "Odessa", "Ukraine", true, "YV15" },
                    { new Guid("888f4fef-0b4c-4bcf-94a8-b8d639510a12"), "Ilo Mosashvili str #24", "Tbilisi", "Georgia", false, "IM24" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "EmploymentEnd", "EmploymentStart", "FirstName", "LastName", "PreferredId", "Role", "TelegramId" },
                values: new object[,]
                {
                    { new Guid("051f2005-9dd2-49b0-874b-d86046ba198a"), "anvarkhon.khamzaev@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3255), "Anvarkhon", "Khamzaev", null, 3, 112046934L },
                    { new Guid("2485a7ae-4011-4664-b13b-16dfa6ba812a"), "malkou.vasili@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3258), "Vasili", "", null, 2, 957365793L },
                    { new Guid("35f72da2-4747-4a17-ade1-e0056ded53e4"), "majidovanvar30.11.2002@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3237), "Anvarjon", "Majidov", null, 3, 534430877L },
                    { new Guid("83949cc3-b49b-4398-a669-a11130bfb98b"), "asgoreglyad@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3262), "Andrei", "Harahliad", null, 3, 1651658270L },
                    { new Guid("ace4e49a-3d3b-46eb-b88c-477843543210"), "sherashera777@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3263), "Sandro", "Sherazadishvili", null, 3, 5046701077L },
                    { new Guid("cfd380e5-7e37-42de-8f4a-27a0bf9cc24d"), "apfpo2001@gmail.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3256), "Polina", "", null, 2, 635524939L },
                    { new Guid("eb0c6e21-4b2e-4766-b17a-ce28b190b074"), "kdavletov@exadel.com", null, new DateTime(2022, 3, 23, 22, 22, 9, 131, DateTimeKind.Local).AddTicks(3253), "Khamza", "Davletov", null, 3, 194740354L }
                });

            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "Id", "FloorNumber", "IsKitchenPresent", "IsMeetingRoomPresent", "OfficeId" },
                values: new object[,]
                {
                    { new Guid("06468b94-d46e-4219-9d9f-e97ce4e0480c"), 1, true, true, new Guid("888f4fef-0b4c-4bcf-94a8-b8d639510a12") },
                    { new Guid("11acaa7b-97d5-424a-8290-f184d9b14577"), 1, false, true, new Guid("1d426867-997d-4aa8-a6ff-90c639e0a2d9") },
                    { new Guid("159bf96f-9d4d-44b1-b7b9-5918d8186ddd"), 9, true, true, new Guid("83ea853c-771b-4fe7-bd21-135ee936145e") },
                    { new Guid("4b00d312-f54b-49a4-96aa-2d3857c59e11"), 3, true, false, new Guid("888f4fef-0b4c-4bcf-94a8-b8d639510a12") },
                    { new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), 3, true, true, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f") },
                    { new Guid("591810fe-d2cc-4b17-876b-9e4586951d8c"), 5, false, false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f") },
                    { new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), 4, true, false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f") },
                    { new Guid("757520cf-a9ad-4e25-939f-df08ddd1459f"), 7, true, true, new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4") },
                    { new Guid("7924687e-3da2-4417-9eb4-9c3188ce71fb"), 2, false, true, new Guid("888f4fef-0b4c-4bcf-94a8-b8d639510a12") },
                    { new Guid("acffb182-439a-424b-a500-30568344aacf"), 8, false, false, new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4") },
                    { new Guid("be4f04ce-5a69-4aef-afcf-8748c016e8f2"), 6, false, true, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f") }
                });

            migrationBuilder.InsertData(
                table: "ParkingPlaces",
                columns: new[] { "Id", "IsBooked", "OfficeId", "PlaceNumber" },
                values: new object[,]
                {
                    { new Guid("0d1b9bfc-7cee-4900-9a67-ad80937c11b3"), false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), 3 },
                    { new Guid("1465be2a-8be6-4f61-8ebc-5eed7541ea77"), false, new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4"), 16 },
                    { new Guid("17d42c15-fa7e-4e07-b708-8fda8e2a1ee4"), false, new Guid("83ea853c-771b-4fe7-bd21-135ee936145e"), 90 },
                    { new Guid("46c4ec48-71eb-4a5c-b2a4-9150c4017d81"), false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), 2 },
                    { new Guid("927d860a-2c84-44c9-b0d3-873ce8f36462"), false, new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4"), 15 },
                    { new Guid("bc8b5bd4-9768-4bfc-b06f-bcec9b98103b"), false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), 4 },
                    { new Guid("d2fb7ffe-51e6-4dc8-a908-220a9aa12c50"), false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), 1 },
                    { new Guid("d3cb25a9-c136-417d-bfc9-5abd56ae97c7"), false, new Guid("18d792cf-b653-4a79-b0c8-ae5d44b63ea4"), 17 },
                    { new Guid("e9acbefc-6dc8-45f9-ac22-4222186f5c72"), false, new Guid("162c0201-b688-40a2-9a76-31c4d05e226f"), 5 }
                });

            migrationBuilder.InsertData(
                table: "Workplaces",
                columns: new[] { "Id", "HasHeadset", "HasKeyboard", "HasMonitor", "HasMouse", "HasPC", "IsNextToWindow", "MapId", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("052141cc-73db-46f4-a901-fa6a413b6567"), true, true, true, true, false, true, new Guid("591810fe-d2cc-4b17-876b-9e4586951d8c"), "502", 0 },
                    { new Guid("06f2eec8-3470-4182-bff1-45739a49757a"), false, false, false, false, false, false, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "306", 0 },
                    { new Guid("1218ae0d-e00d-43e5-a2bc-3b512209f1a1"), false, true, false, false, true, true, new Guid("757520cf-a9ad-4e25-939f-df08ddd1459f"), "77", 1 },
                    { new Guid("182f8693-302f-413f-9767-f5bf532f26a5"), false, true, false, false, true, false, new Guid("06468b94-d46e-4219-9d9f-e97ce4e0480c"), "3", 0 },
                    { new Guid("1b8b316f-5fdc-4c40-8d80-cde8449dff37"), false, false, false, false, false, false, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "307", 0 },
                    { new Guid("28db1086-163e-403a-aae1-b53dc84d54be"), false, true, true, true, false, false, new Guid("be4f04ce-5a69-4aef-afcf-8748c016e8f2"), "601", 0 },
                    { new Guid("42600a58-4b1d-4a56-8970-f7fa1c6e577d"), false, false, false, false, false, false, new Guid("be4f04ce-5a69-4aef-afcf-8748c016e8f2"), "602", 0 },
                    { new Guid("511b67e3-f1ee-4e69-b2f5-5cd9f78de703"), true, false, false, true, false, true, new Guid("11acaa7b-97d5-424a-8290-f184d9b14577"), "101", 0 },
                    { new Guid("5607563e-3d8c-4c44-90eb-c32c5c6956a1"), true, true, true, true, true, true, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "333", 0 },
                    { new Guid("5c5f4919-55d5-4983-b777-f07f95cacbf3"), true, false, true, true, false, false, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "305", 0 },
                    { new Guid("5fcfb0f1-aa49-4817-9d28-17d70fe13d2c"), true, false, false, false, true, false, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "302", 0 },
                    { new Guid("70704092-b3e1-4384-a50f-c348634ac029"), false, true, false, false, true, true, new Guid("159bf96f-9d4d-44b1-b7b9-5918d8186ddd"), "SU", 0 },
                    { new Guid("70b06743-b8f1-4571-a68c-bd08937e3ee9"), false, true, false, false, true, false, new Guid("591810fe-d2cc-4b17-876b-9e4586951d8c"), "501", 0 },
                    { new Guid("82c5d336-df53-40a3-8c4d-56141cb5238f"), true, true, false, false, true, true, new Guid("be4f04ce-5a69-4aef-afcf-8748c016e8f2"), "603", 0 },
                    { new Guid("91179d2a-cd11-40b2-879d-792c006b04fe"), false, true, false, false, true, false, new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), "402", 0 },
                    { new Guid("91e8efd0-c944-4613-b896-548f7f64df3a"), true, true, true, false, false, false, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "303", 0 },
                    { new Guid("952e4d5a-3400-42da-875d-da2b8234c928"), true, true, true, true, false, true, new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), "404", 0 },
                    { new Guid("a01036d3-fdbe-4f0f-b91c-3d916d752de3"), false, false, true, false, false, true, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "304", 0 },
                    { new Guid("ba49af18-c6a9-449c-852e-2e39a9aaacf0"), true, true, false, false, true, true, new Guid("06468b94-d46e-4219-9d9f-e97ce4e0480c"), "1", 0 },
                    { new Guid("ca157299-7037-4c3d-af5b-cb90426e7270"), false, true, true, false, true, true, new Guid("06468b94-d46e-4219-9d9f-e97ce4e0480c"), "2", 0 },
                    { new Guid("cd29debf-565b-45e3-b5d5-a004168ebbd7"), true, false, false, false, true, false, new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), "405", 0 },
                    { new Guid("ece9bae9-ecba-4a47-8f44-1d0fb3552687"), false, false, false, false, true, true, new Guid("52858652-e6fe-48ef-b6a2-6d85f972dbee"), "301", 0 },
                    { new Guid("ed7e5cf4-6db0-44d1-887f-40af33259e88"), false, true, true, false, true, false, new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), "403", 0 },
                    { new Guid("ef5284f2-771d-44ba-9469-40243b7ef009"), true, false, true, false, false, false, new Guid("756aceda-ccbe-4c0c-8337-b0dc5582185a"), "401", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ParkingPlaceId",
                table: "Bookings",
                column: "ParkingPlaceId",
                unique: true,
                filter: "[ParkingPlaceId] IS NOT NULL");

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
                name: "IX_ParkingPlaces_OfficeId",
                table: "ParkingPlaces",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PreferredId",
                table: "Users",
                column: "PreferredId");

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
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Vacations");

            migrationBuilder.DropTable(
                name: "ParkingPlaces");

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
