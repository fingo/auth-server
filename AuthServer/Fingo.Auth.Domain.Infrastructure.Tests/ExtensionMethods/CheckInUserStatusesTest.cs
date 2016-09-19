using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using Xunit;
using System.Linq;
using Fingo.Auth.DbAccess.Models.Statuses;

namespace Fingo.Auth.Domain.Infrastructure.Tests.ExtensionMethods
{
    public class CheckInUserStatusesTest
    {
        //Test : T WithStatuses<T>(this T model, params Status[] list)

        [Fact]
        public void Recive_Null_When_Filtering_Null_Entity()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = null;

            //Act

            var result = baseEntity.WithStatuses(UserStatus.Active , UserStatus.Deleted);

            //Assert

            Assert.Null(result);
            Assert.Null(baseEntity);
        }

        [Fact]
        public void Recive_Null_When_Filtering_One_Entity_With_Wrong_Status()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = new BaseEntityWithUserStatus() { Status = UserStatus.Deleted };
            //Act

            var result = baseEntity.WithStatuses(UserStatus.Active);

            //Assert

            Assert.Null(result);
            Assert.NotNull(baseEntity);
        }

        [Fact]
        public void Recive_Entity_When_Filtering_One_Entity_With_Valid_Status()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = new BaseEntityWithUserStatus() { Status = UserStatus.Active };

            //Act

            var result = baseEntity.WithStatuses(UserStatus.Active);

            //Assert

            Assert.NotNull(result);
            Assert.NotNull(baseEntity);
        }

        //Test : IEnumerable<T> WithStatuses<T>(this IEnumerable<T> modelList , params Status[] list)

        [Fact]
        public void Recive_Null_When_Filtering_Null_Collection()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = null;

            //Act

            var result = nullCollection.WithStatuses(UserStatus.Active , UserStatus.Deleted);

            Assert.Null(result);
            Assert.Null(nullCollection);
        }

        [Fact]
        public void Recive_Empty_Collection_When_Filtering_Collection_With_Wrong_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Deleted},
                new BaseEntityWithUserStatus() {Status = UserStatus.Deleted}
            };

            //Act

            var result = nullCollection.WithStatuses(UserStatus.Active);

            //Assert

            Assert.Empty(result);
        }

        [Fact]
        public void Recive_Collection_When_Filtering_Collection_With_Valid_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Active},
                new BaseEntityWithUserStatus() {Status = UserStatus.Active}
            };

            //Act

            var result = nullCollection.WithStatuses(UserStatus.Active);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count()==2);
        }

        [Fact]
        public void Recive_Collection_When_Filtering_Collection_With_Valid_Multiple_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Active},
                new BaseEntityWithUserStatus() {Status = UserStatus.Deleted}
            };

            //Act

            var result = nullCollection.WithStatuses(UserStatus.Active,UserStatus.Deleted);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 2);
        }

        //Test : T WithoutStatuses<T>(this T model , params Status[] list)

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_Null_Entity()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = null;

            //Act

            var result = baseEntity.WithoutStatuses(UserStatus.Active , UserStatus.Deleted, UserStatus.Registered);

            //Assert

            Assert.Null(result);
            Assert.Null(baseEntity);
        }

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_One_Entity_With_Matching_Status()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = new BaseEntityWithUserStatus() { Status = UserStatus.Deleted };
            //Act

            var result = baseEntity.WithoutStatuses(UserStatus.Deleted);

            //Assert

            Assert.Null(result);
            Assert.NotNull(baseEntity);
        }

        [Fact]
        public void WithoutStatuses_Recive_Entity_When_Filtering_One_Entity_With_Not_Matching_Status()
        {
            //Arrange

            BaseEntityWithUserStatus baseEntity = new BaseEntityWithUserStatus() { Status = UserStatus.Active };

            //Act

            var result = baseEntity.WithoutStatuses(UserStatus.Deleted,UserStatus.Registered);

            //Assert

            Assert.NotNull(result);
            Assert.NotNull(baseEntity);
        }

        //Test : IEnumerable<T> WithoutStatuses<T>(this IEnumerable<T> modelList , params Status[] list)

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_Null_Collection()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = null;

            //Act

            var result = nullCollection.WithoutStatuses(UserStatus.Deleted);

            Assert.Null(result);
            Assert.Null(nullCollection);
        }

        [Fact]
        public void WithoutStatuses_Recive_Empty_Collection_When_Filtering_Collection_With_Matching_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Deleted},
                new BaseEntityWithUserStatus() {Status = UserStatus.Active}
            };

            //Act

            var result = nullCollection.WithoutStatuses(UserStatus.Active,UserStatus.Deleted);

            //Assert

            Assert.Empty(result);
        }

        [Fact]
        public void WithoutStatuses_Recive_Collection_When_Filtering_Collection_With_Not_Matching_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Active},
                new BaseEntityWithUserStatus() {Status = UserStatus.Registered}
            };

            //Act

            var result = nullCollection.WithoutStatuses(UserStatus.Deleted);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void WithoutStatuses_Recive_Collection_When_Filtering_Collection_With_Valid_Multiple_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithUserStatus> nullCollection = new List<BaseEntityWithUserStatus>()
            {
                new BaseEntityWithUserStatus() {Status = UserStatus.Active},
                new BaseEntityWithUserStatus() {Status = UserStatus.Deleted},
                new BaseEntityWithUserStatus() {Status = UserStatus.Registered}
            };

            //Act

            var result = nullCollection.WithoutStatuses(UserStatus.Active , UserStatus.Deleted);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 1);
        }
    }
}