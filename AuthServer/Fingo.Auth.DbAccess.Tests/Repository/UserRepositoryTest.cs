using System.Linq;
using Fingo.Auth.DbAccess.Context;
using Fingo.Auth.DbAccess.Models;
using Fingo.Auth.DbAccess.Repository.Implementation;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.DbAccess.Tests.MockService;
using Xunit;

namespace Fingo.Auth.DbAccess.Tests.Repository
{
    public class UserRepositoryTest
    {
        private AuthServerContext context;

        public UserRepositoryTest()
        {
            MockDbInMemory createDbInMemory = new MockDbInMemory();
            context = createDbInMemory.CreateNewContextOptions();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void Can_Get_User_By_Id()
        {
            //Arrange

            IUserRepository repo = new UserRepository(context);

            //Act

            User result = repo.GetById(1);
            User noResult = repo.GetById(5);

            //Assert

            Assert.True(result.Id == 1);
            Assert.True(result.Login == "tekst");
            Assert.True(result.FirstName == "pierwszy");
            Assert.Null(noResult);
        }

        [Fact]
        public void Can_Add_User()
        {
            //Arrange

            IUserRepository repo = new UserRepository(context);
            User newUser = new User() { Id = 5 , FirstName = "piaty" , Password = "piec" , Login = "piec" };

            //Act

            repo.Add(newUser);
            User addedUser = repo.GetById(5);

            //Assert

            Assert.True(context.Set<User>().Count() == 5);
            Assert.NotNull(addedUser);
            Assert.True(addedUser.FirstName == "piaty");
        }

        [Fact]
        public void Can_Remove_User()
        {
            //Arrange

            IUserRepository repo = new UserRepository(context);

            //Act
            User toDelete = repo.GetById(1);
            repo.Delete(toDelete);
            User removedUser = repo.GetById(1);
            User firstUser = context.Set<User>().FirstOrDefault();

            //Assert

            Assert.True(context.Set<User>().Count() == 3);
            Assert.Null(removedUser);
            Assert.True(firstUser.Id == 2);
        }

        [Fact]
        public void Can_Change_User_Password()
        {
            //Arrange

            IUserRepository repo = new UserRepository(context);
            User toUpdate = repo.GetById(3);

            //Act

            repo.UpdateUserPassword(toUpdate , "newPassword3");
            User afterChangePassword = repo.GetById(3);

            //Assert

            Assert.True(afterChangePassword.Password == "newPassword3");
            Assert.True(afterChangePassword.Login == "trzeci");
            Assert.True(context.Set<User>().Count() == 4);
        }

        [Fact]
        public void Can_Get_All_Projects_From_User()
        {
            //Arrange

            IUserRepository repo = new UserRepository(context);

            //Act

            var result = repo.GetAllProjectsFromUser(1);

            //Assert

            Assert.True(result.Count()==1);
            Assert.True(result.First().Name== "name1");
        }
    }
}