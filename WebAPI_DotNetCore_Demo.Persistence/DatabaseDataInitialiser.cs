using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Persistence
{
    public static class DatabaseDataInitialiser
    {
        public static void Initialise(WebAPIDemoDbContext context)
        {
            PopulateInitialUser(context);
        }

        private static void PopulateInitialUser(WebAPIDemoDbContext context)
        {
            if (context.Users.Any()) { return; }

            var userID = new Guid("9338B511-C135-41A9-9ACE-48211DB19BE9");
            var currentDateTime = DateTime.Now;
            var (Salt, Hash) = CreatePasswordHash("jrrtolkien");

            context.Users.Add(new User
            {
                ID = userID,
                UserName = "aragorn.elessar",
                FirstName = "Aragorn",
                LastName = "Elessar",
                PasswordHash = Hash,
                PasswordSalt = Salt,
                UserRoles = new List<UserRole>
                {
                    new UserRole { RoleID = new Guid("dab0807c-822c-4258-ad79-07dd543cb253") }
                },
                CreateDate = currentDateTime,
                UpdateDate = currentDateTime,
                CreateByUserID = userID,
                UpdateByUserID = userID
            });

            context.SaveChanges();
        }

        private static (byte[] Salt, byte[] Hash) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                return (hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
