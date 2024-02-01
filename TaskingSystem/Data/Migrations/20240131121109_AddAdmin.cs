using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) " +
                        $"VALUES (N'{Guid.NewGuid().ToString()}', N'admin', N'ADMIN', N'admin@mail.com', N'ADMIN@MAIL.COM', 0, N'AQAAAAEAACcQAAAAEAWH/eLXv3ucFRs/Tpb1+bsXh5NHCidhn+QQotrYOmaUUnI72vKLagO4ojuwg5Dkng==', N'ZKHQLKZMOM3JJXOJ2ELXDLOPYBPLXGI5', N'4b72312a-29db-40a9-9eb6-58f5e39c9a84', NULL, 0, 0, NULL, 1, 0, N'Super', N'Admin', NULL)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE UserName = 'admin'");

        }
    }
}
