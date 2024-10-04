using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.CheckConstraint("CK_Name_MinLength1", "LEN(Name) >= 3");
                });

            migrationBuilder.CreateTable(
                name: "AnnualLeaveRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AnnualLeave = table.Column<int>(type: "int", nullable: false),
                    CasualLeave = table.Column<int>(type: "int", nullable: false),
                    RestrictedHoliday = table.Column<int>(type: "int", nullable: false),
                    BonusLeave = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualLeaveRecord", x => x.Id);
                    table.CheckConstraint("CK_AnnualLeaveRecord_Year_Length", "[Year] >= 1000 AND [Year] <= 9999");
                    table.ForeignKey(
                        name: "FK_AnnualLeaveRecord_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.CheckConstraint("CK_Email_Format", "[Email] LIKE '%@%.com'");
                    table.CheckConstraint("CK_Name_MinLength", "LEN(Name) >= 3");
                    table.CheckConstraint("CK_Password_MinLength", "LEN(Password) >= 5");
                    table.ForeignKey(
                        name: "FK_Employee_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveType = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveApplication_Employee_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveApplication_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveUsageTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ALUsed = table.Column<int>(type: "int", nullable: false),
                    RHUsed = table.Column<int>(type: "int", nullable: false),
                    BLUsed = table.Column<int>(type: "int", nullable: false),
                    CLUsed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveUsageTracker", x => x.Id);
                    table.CheckConstraint("CK_LeaveUsageTracker_Year_Length", "[Year] >= 1000 AND [Year] <= 9999");
                    table.ForeignKey(
                        name: "FK_LeaveUsageTracker_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualLeaveRecord_RoleId",
                table: "AnnualLeaveRecord",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleId",
                table: "Employee",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplication_ApproverId",
                table: "LeaveApplication",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplication_EmployeeId",
                table: "LeaveApplication",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveUsageTracker_EmployeeId",
                table: "LeaveUsageTracker",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualLeaveRecord");

            migrationBuilder.DropTable(
                name: "LeaveApplication");

            migrationBuilder.DropTable(
                name: "LeaveUsageTracker");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
