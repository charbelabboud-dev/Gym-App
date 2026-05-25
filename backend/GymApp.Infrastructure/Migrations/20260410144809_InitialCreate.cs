using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    MemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MemName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MemPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MemDurationDays = table.Column<int>(type: "integer", nullable: false),
                    MemDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MemStatus = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.MemId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserPassword = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UserStatus = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    CoCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CoFname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CoLname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CoBirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CoPhone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    CoEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CoAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CoHireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CoSpecialty = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CoStatus = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.CoCode);
                    table.ForeignKey(
                        name: "FK_Coaches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dietitians",
                columns: table => new
                {
                    DietCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DietFname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DietLname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DietEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DietPhone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    DietStatus = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dietitians", x => x.DietCode);
                    table.ForeignKey(
                        name: "FK_Dietitians_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClFname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClLname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClBirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ClPhone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    ClAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClRegisterDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ClStatus = table.Column<bool>(type: "boolean", nullable: false),
                    ClCoachId = table.Column<string>(type: "character varying(5)", nullable: true),
                    ClMembershipId = table.Column<int>(type: "integer", nullable: true),
                    DietitianId = table.Column<string>(type: "character varying(5)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClCode);
                    table.UniqueConstraint("AK_Clients_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Clients_Coaches_ClCoachId",
                        column: x => x.ClCoachId,
                        principalTable: "Coaches",
                        principalColumn: "CoCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Dietitians_DietitianId",
                        column: x => x.DietitianId,
                        principalTable: "Dietitians",
                        principalColumn: "DietCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_MembershipPlans_ClMembershipId",
                        column: x => x.ClMembershipId,
                        principalTable: "MembershipPlans",
                        principalColumn: "MemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CheckOutTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttId);
                    table.ForeignKey(
                        name: "FK_Attendances_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    GoalType = table.Column<string>(type: "text", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientGoals_Clients_ClientCode",
                        column: x => x.ClientCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientMemberships",
                columns: table => new
                {
                    CmId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    MemId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMemberships", x => x.CmId);
                    table.ForeignKey(
                        name: "FK_ClientMemberships_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientMemberships_MembershipPlans_MemId",
                        column: x => x.MemId,
                        principalTable: "MembershipPlans",
                        principalColumn: "MemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DietPlans",
                columns: table => new
                {
                    DietId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    DietitianId = table.Column<string>(type: "character varying(5)", nullable: false),
                    DietStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DietEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DietDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietPlans", x => x.DietId);
                    table.ForeignKey(
                        name: "FK_DietPlans_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DietPlans_Dietitians_DietitianId",
                        column: x => x.DietitianId,
                        principalTable: "Dietitians",
                        principalColumn: "DietCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    MemId = table.Column<int>(type: "integer", nullable: false),
                    PayAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PayMethod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PayDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PayStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PayId);
                    table.ForeignKey(
                        name: "FK_Payments_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MembershipPlans_MemId",
                        column: x => x.MemId,
                        principalTable: "MembershipPlans",
                        principalColumn: "MemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "numeric", nullable: true),
                    Chest = table.Column<decimal>(type: "numeric", nullable: true),
                    Waist = table.Column<decimal>(type: "numeric", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressEntries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SesClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    SesCoCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    SesType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SesDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SesDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SesDuration = table.Column<int>(type: "integer", nullable: false),
                    SesStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SesId);
                    table.ForeignKey(
                        name: "FK_Sessions_Clients_SesClCode",
                        column: x => x.SesClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sessions_Coaches_SesCoCode",
                        column: x => x.SesCoCode,
                        principalTable: "Coaches",
                        principalColumn: "CoCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    WpId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    CoCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    WpName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WpStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    WpEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.WpId);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Coaches_CoCode",
                        column: x => x.CoCode,
                        principalTable: "Coaches",
                        principalColumn: "CoCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    RevId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    CoCode = table.Column<string>(type: "character varying(5)", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SessionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.RevId);
                    table.ForeignKey(
                        name: "FK_Reviews_Clients_ClCode",
                        column: x => x.ClCode,
                        principalTable: "Clients",
                        principalColumn: "ClCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Coaches_CoCode",
                        column: x => x.CoCode,
                        principalTable: "Coaches",
                        principalColumn: "CoCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SesId");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    WeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WpId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    RestSeconds = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.WeId);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_WorkoutPlans_WpId",
                        column: x => x.WpId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "WpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClCode",
                table: "Attendances",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGoals_ClientCode",
                table: "ClientGoals",
                column: "ClientCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMemberships_ClCode",
                table: "ClientMemberships",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMemberships_MemId",
                table: "ClientMemberships",
                column: "MemId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClCoachId",
                table: "Clients",
                column: "ClCoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClMembershipId",
                table: "Clients",
                column: "ClMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DietitianId",
                table: "Clients",
                column: "DietitianId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_UserId",
                table: "Coaches",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dietitians_UserId",
                table: "Dietitians",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DietPlans_ClCode",
                table: "DietPlans",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_DietPlans_DietitianId",
                table: "DietPlans",
                column: "DietitianId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ClCode",
                table: "Payments",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MemId",
                table: "Payments",
                column: "MemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressEntries_ClientId",
                table: "ProgressEntries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ClCode",
                table: "Reviews",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CoCode",
                table: "Reviews",
                column: "CoCode");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SessionId",
                table: "Reviews",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SesClCode",
                table: "Sessions",
                column: "SesClCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SesCoCode",
                table: "Sessions",
                column: "SesCoCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserEmail",
                table: "Users",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WpId",
                table: "WorkoutExercises",
                column: "WpId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_ClCode",
                table: "WorkoutPlans",
                column: "ClCode");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_CoCode",
                table: "WorkoutPlans",
                column: "CoCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "ClientGoals");

            migrationBuilder.DropTable(
                name: "ClientMemberships");

            migrationBuilder.DropTable(
                name: "DietPlans");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProgressEntries");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Dietitians");

            migrationBuilder.DropTable(
                name: "MembershipPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
