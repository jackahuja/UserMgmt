using Microsoft.EntityFrameworkCore;
using UserMgmt.Helpers;
using BCryptNet = BCrypt.Net.BCrypt;

namespace UserMgmt
{
    public class DummyData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(
                serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {                
                if (context.Users.Any())
                {
                    return;
                }

                context.Users.AddRange(
                    new User
                    {
                        Id = 1,
                        FirstName = "Ravi",
                        LastName = "Ahuja",
                        Username = "RaviAhuja",
                        PasswordHash = BCryptNet.HashPassword("ravipassword"),
                        Role = Role.Edit
                    },
                    new User
                    {
                        Id = 2,
                        FirstName = "Test",
                        LastName = "User",
                        Username = "TestUser",
                        PasswordHash = BCryptNet.HashPassword("testpassword"),
                        Role = Role.View
                    },
                    new User
                    {
                        Id = 3,
                        FirstName = "Admin",
                        LastName = "Admin",
                        Username = "Admin",
                        PasswordHash = BCryptNet.HashPassword("admin"),
                        Role = Role.Admin
                    },
                    new User
                    {
                        Id = 4,
                        FirstName = "New",
                        LastName = "User",
                        Username = "NewUser",
                        PasswordHash = BCryptNet.HashPassword("View"),
                        Role = Role.View
                    });

                context.SaveChanges();
            }
        }
    }
}
