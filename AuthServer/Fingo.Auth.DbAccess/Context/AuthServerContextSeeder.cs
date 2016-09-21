using System;
using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models;

namespace Fingo.Auth.DbAccess.Context
{
    public class AuthServerContextSeeder
    {
        public static void Seed(AuthServerContext context)
        {
            SeedUsers(context);

            SeedProjects(context);
        }

        private static void SeedProjects(AuthServerContext context)
        {
            var data = new List<Project>
            {
                new Project
                {
                    Information = new ClientInformation {ContactData = "contactData1"} ,
                    Name = "name1" ,
                    ProjectUsers =
                        new List<ProjectUser>
                        {
                            new ProjectUser {UserId = 1} ,
                            new ProjectUser {UserId = 2} ,
                            new ProjectUser {UserId = 3}
                        }
                } ,
                new Project
                {
                    Information = new ClientInformation {ContactData = "contactData2"} ,
                    Name = "name2" ,
                    ProjectUsers =
                        new List<ProjectUser> {new ProjectUser {UserId = 2} , new ProjectUser {UserId = 4}}
                } ,
                new Project
                {
                    Information = new ClientInformation {ContactData = "contactData3"} ,
                    Name = "name3"
                } ,
                new Project
                {
                    Information = new ClientInformation {ContactData = "contactData4"} ,
                    Name = "name4"
                }
            };

            var data1 = new List<Project>
            {
                new Project
                {
                    Name = "ManagementAppProject" ,
                    ProjectGuid = new Guid("AC1A551CB011EDB100D1E55BEEF511CE")
                }
            };

            context.Set<Project>().AddRange(data);
            context.Set<Project>().AddRange(data1);
            context.SaveChanges();
        }

        private static void SeedUsers(AuthServerContext context)
        {
            var users = new List<User>
            {
                new User
                {
                    FirstName = "pierwszy" ,
                    LastName = "pierwszy" ,
                    Login = "q@q" ,
                    Password = "8e35c2cd3bf6641bdb0e2050b76932cbb2e6034a0ddacc1d9bea82a6ba57f7cf" ,
                    Status = 0
                } ,
                new User {FirstName = "drugi" , LastName = "drugi" , Login = "drugi" , Password = "drugi" , Status = 0} ,
                new User
                {
                    FirstName = "trzeci" ,
                    LastName = "trzeci" ,
                    Login = "trzeci" ,
                    Password = "trzeci" ,
                    Status = 0
                } ,
                new User
                {
                    FirstName = "czwarty" ,
                    LastName = "czwarty" ,
                    Login = "czwarty" ,
                    Password = "czwarty" ,
                    Status = 0
                }
            };
            context.Set<User>().AddRange(users);
            context.SaveChanges();
        }
    }
}