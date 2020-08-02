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

            var primaryUserID = new Guid("9338B511-C135-41A9-9ACE-48211DB19BE9");
            var currentDateTime = DateTime.Now;
            var (primarySalt, primaryHash) = CreatePasswordHash("jrrtolkien");

            context.Users.Add(new User
            {
                ID = primaryUserID,
                UserName = "aragorn.elessar",
                FirstName = "Aragorn",
                LastName = "Elessar",
                PasswordHash = primaryHash,
                PasswordSalt = primarySalt,
                UserRoles = new List<UserRole>
                {
                    new UserRole { RoleID = new Guid("dab0807c-822c-4258-ad79-07dd543cb253") }
                },
                CreateDate = currentDateTime,
                UpdateDate = currentDateTime,
                CreateByUserID = primaryUserID,
                UpdateByUserID = primaryUserID
            });

            var secondaryUserID = new Guid("30B801CC-216B-4F1B-6243-08D8312EBC95");
            var (secondarySalt, secondaryHash) = CreatePasswordHash("eruilluvatar");

            context.Users.Add(new User
            {
                ID = secondaryUserID,
                UserName = "arwen.undomiel",
                FirstName = "Arwen",
                LastName = "Undomiel",
                PasswordHash = secondaryHash,
                PasswordSalt = secondarySalt,
                UserRoles = new List<UserRole>
                {
                    new UserRole { RoleID = new Guid("99626019-0a7d-4c79-9058-b727ed7b1fa9") }
                },
                CreateDate = currentDateTime,
                UpdateDate = currentDateTime,
                CreateByUserID = primaryUserID,
                UpdateByUserID = primaryUserID
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
