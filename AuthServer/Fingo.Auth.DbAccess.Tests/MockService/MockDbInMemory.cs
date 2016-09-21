using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fingo.Auth.DbAccess.Tests.MockService
{
    public class MockDbInMemory
    {
        public AuthServerContext CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<AuthServerContext>();
            builder.UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);

            var context = new AuthServerContext(builder.Options);

            SeedData(context);

            context.SaveChanges();

            return context;
        }

        private void SeedData(AuthServerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var users = new List<User>
            {
                new User {FirstName = "pierwszy" , LastName = "pierwszy" , Login = "tekst" , Password = "tekst1"} ,
                new User {FirstName = "drugi" , LastName = "drugi" , Login = "drugi" , Password = "drugi"} ,
                new User {FirstName = "trzeci" , LastName = "trzeci" , Login = "trzeci" , Password = "trzeci"} ,
                new User {FirstName = "czwarty" , LastName = "czwarty" , Login = "czwarty" , Password = "czwarty"}
            };

            var data = new List<Project>
            {
                new Project
                {
                    ProjectGuid = Guid.Parse("8ae3a84e-a105-4c29-a911-d15d344d3fbe") ,
                    Information = new ClientInformation {ContactData = "contactData1"} ,
                    Name = "name1" ,
                    ProjectUsers = new List<ProjectUser> {new ProjectUser {UserId = 1} , new ProjectUser {UserId = 3}}
                } ,
                new Project
                {
                    ProjectGuid = Guid.Parse("43f3b1f8-f456-435a-880a-f6124395da91") ,
                    Information = new ClientInformation {ContactData = "contactData2"} ,
                    Name = "name2" ,
                    ProjectUsers = new List<ProjectUser> {new ProjectUser {UserId = 2} , new ProjectUser {UserId = 4}}
                } ,
                new Project
                {
                    ProjectGuid = Guid.Parse("1dbcbea0-1633-4790-a0e6-2e70acf944a1") ,
                    Information = new ClientInformation {ContactData = "contactData3"} ,
                    Name = "name3"
                } ,
                new Project
                {
                    ProjectGuid = Guid.Parse("65cda38b-5a0d-473f-a9d4-dae6d8dae55a") ,
                    Information = new ClientInformation {ContactData = "contactData4"} ,
                    Name = "name4"
                }
            };

            context.Set<User>().AddRange(users);
            context.Set<Project>().AddRange(data);
        }
    }
}