using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningApp.Service.DB.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUserAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed roles and admin user inside the migration
            SeedRolesAndAdminUser(migrationBuilder);
        }

        private void SeedRolesAndAdminUser(MigrationBuilder migrationBuilder)
        {
            // Create the password hasher
            var hasher = new PasswordHasher<IdentityUser>();

            // Create roles
            var adminRoleId = Guid.NewGuid().ToString();
            var teacherRoleId = Guid.NewGuid().ToString();

            // Insert the Admin role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[] { adminRoleId, "Admin", "ADMIN" }
            );

            // Insert the Teacher role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[] { teacherRoleId, "Teacher", "TEACHER" }
            );

            // Create the admin user
            var adminUserId = Guid.NewGuid().ToString();
            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            // Hash the password 'Admin@123'
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            // Insert admin user into AspNetUsers table
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { adminUser.Id, adminUser.UserName, adminUser.NormalizedUserName, adminUser.Email, adminUser.NormalizedEmail, adminUser.EmailConfirmed, adminUser.PasswordHash, adminUser.SecurityStamp, adminUser.PhoneNumber, adminUser.PhoneNumberConfirmed, adminUser.TwoFactorEnabled, adminUser.LockoutEnabled, adminUser.AccessFailedCount }
            );

            // Assign the Admin role to the admin user
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { adminUserId, adminRoleId }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles;");

            migrationBuilder.Sql("DELETE FROM AspNetUsers;");

            migrationBuilder.Sql("DELETE FROM AspNetRoles;");
        }
    }
}
