using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskingSystem.Data;
using TaskingSystem.Models;

namespace TaskingSystem.Services
{
    public class Initialization
    {
        private ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public Initialization(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public async Task InitializeRoles()
        {
            if (_context is null)
                return;
            string[] roles = new string[] { Roles.SuperAdmin, Roles.Admin, Roles.Manger, Roles.User };

            foreach (string role in roles)
            {
                RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(_context);

                if (!_context.Roles.Any(r => r.Name == role))
                {
                    IdentityRole newRole = new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };

                    await roleStore.CreateAsync(newRole);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task InitializeUsers()
        {



            ApplicationUser? configUser = _configuration.GetSection("ApplicationUser").Get<ApplicationUser>();

            if (configUser is null)
                return;

            ApplicationUser user = new ApplicationUser
            {
                FirstName = configUser.FirstName,
                LastName = configUser.LastName,
                Email = configUser.Email,
                NormalizedEmail = configUser.Email?.ToUpper(),
                UserName = configUser.UserName,
                NormalizedUserName = configUser.UserName?.ToUpper(),
                PhoneNumber = configUser.PhoneNumber,
                ProfilePicture = null,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),

            };

            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
                return;


            PasswordHasher<ApplicationUser> password = new PasswordHasher<ApplicationUser>();
            //TODO
            //Get password from json appsettings
            string hashed = password.HashPassword(user, "7nJ4oq7@f*Dg");
            user.PasswordHash = hashed;

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(_context);
            await userStore.CreateAsync(user);
            await _context.SaveChangesAsync();

            string? role = await _context.Roles.Where(a => a.Name == Roles.SuperAdmin).Select(a => a.Id).FirstOrDefaultAsync();

            if (role is not null)
                await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = user.Id, RoleId = role });

            await _context.SaveChangesAsync();

        }
    }

}


