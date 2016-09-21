using System;
using System.Linq;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Implementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Tests.MockService;
using Xunit;

namespace Fingo.Auth.DbAccess.Tests.Repository
{
    public class ProjectRepositoryTest
    {
        public ProjectRepositoryTest()
        {
            var createDbInMemory = new MockDbInMemory();
            context = createDbInMemory.CreateNewContextOptions();
        }

        private readonly AuthServerContext context;

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void Can_Add_Project()
        {
            //Arrange 

            IProjectRepository _repo = new ProjectRepository(context);
            var newProject = new Project
            {
                Id = 5 ,
                Information = new ClientInformation {ContactData = "contactData5"} ,
                Name = "name5"
            };

            //Act

            _repo.Add(newProject);
            var datas = _repo.GetAll().Last();

            //Assert

            Assert.True(newProject.Information.ContactData == datas.Information.ContactData);
            Assert.True(newProject.Name == datas.Name);
            Assert.True(context.Set<Project>().Count() == 5);
        }

        [Fact]
        public void Can_Delete_Project()
        {
            //Arrange 

            IProjectRepository _repo = new ProjectRepository(context);

            //Act
            var toDelete = _repo.GetById(4);
            _repo.Delete(toDelete);
            var data = context.Set<Project>().Count();

            //Assert

            Assert.True(data == 3);
        }

        [Fact]
        public void Can_Edit_Project()
        {
            //Arrange

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var toChange = _repo.GetById(4);
            toChange.Name = "newName4";
            toChange.Information = new ClientInformation {ContactData = "newClientInformation"};
            _repo.Edit(toChange);
            var result = _repo.GetById(4);

            //Assert

            Assert.True(result.Name == "newName4");
            Assert.True(result.Information.ContactData == "newClientInformation");
            Assert.True(context.Set<Project>().Count() == 4);
        }

        [Fact]
        public void Can_Get_Project_By_Guid()
        {
            //Arrange

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var project = _repo.GetByGuid(Guid.Parse("1dbcbea0-1633-4790-a0e6-2e70acf944a1"));

            //Assert

            Assert.NotNull(project);
            Assert.True(project.ProjectGuid == Guid.Parse("1dbcbea0-1633-4790-a0e6-2e70acf944a1"));
            Assert.True(project.Name == "name3");
        }

        [Fact]
        public void Can_GetAll_Project()
        {
            //Arrange 

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var data = _repo.GetAll();

            //Assert

            Assert.True(data.Count() == 4);
        }

        [Fact]
        public void Can_GetById_Project()
        {
            //Arrange

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var result = _repo.GetById(1);

            //Assert

            Assert.True(result.Id == 1);
            Assert.True(result.Name == "name1");
            Assert.True(result.Information.ContactData == "contactData1");
        }

        [Fact]
        public void Can_GetUsersFromProjectById()
        {
            //Arrange

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var users = _repo.GetAllUsersFromProject(1);

            //Assert

            Assert.True(users.Count() == 2);
            Assert.True(users.First().FirstName == "pierwszy");
            Assert.True(users.Last().FirstName == "trzeci");
        }

        [Fact]
        public void Get_Null_When_Given_Invalid_Guid_GetProjectByGuid()
        {
            //Arrange

            IProjectRepository _repo = new ProjectRepository(context);

            //Act

            var project = _repo.GetByGuid(Guid.Parse("00000000-0000-0000-0000-000000000000"));

            //Assert

            Assert.Null(project);
        }
    }
}