using System.Collections.Generic;
using Fingo.Auth.DbAccess.Models.Base;
using Fingo.Auth.DbAccess.Models.Statuses;
using Xunit;
using Fingo.Auth.Domain.Infrastructure.ExtensionMethods;
using System.Linq;

namespace Fingo.Auth.Domain.Infrastructure.Tests.ExtensionMethods
{
    public class CheckInProjectStatusesTest
    {
        //Test : T WithStatuses<T>(this T model, params Status[] list)

        [Fact]
        public void Recive_Null_When_Filtering_Null_Entity()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = null;

            //Act

            var result = baseEntity.WithStatuses(ProjectStatus.Active , ProjectStatus.Deleted);

            //Assert

            Assert.Null(result);
            Assert.Null(baseEntity);
        }

        [Fact]
        public void Recive_Null_When_Filtering_One_Entity_With_Wrong_Status()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = new BaseEntityWithProjectStatus() { Status = ProjectStatus.Deleted };
            //Act

            var result = baseEntity.WithStatuses(ProjectStatus.Active);

            //Assert

            Assert.Null(result);
            Assert.NotNull(baseEntity);
        }

        [Fact]
        public void Recive_Entity_When_Filtering_One_Entity_With_Valid_Status()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = new BaseEntityWithProjectStatus() { Status = ProjectStatus.Active };

            //Act

            var result = baseEntity.WithStatuses(ProjectStatus.Active);

            //Assert

            Assert.NotNull(result);
            Assert.NotNull(baseEntity);
        }

        //Test : IEnumerable<T> WithStatuses<T>(this IEnumerable<T> modelList , params Status[] list)

        [Fact]
        public void Recive_Null_When_Filtering_Null_Collection()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = null;

            //Act

            var result = nullCollection.WithStatuses(ProjectStatus.Active , ProjectStatus.Deleted);

            Assert.Null(result);
            Assert.Null(nullCollection);
        }

        [Fact]
        public void Recive_Empty_Collection_When_Filtering_Collection_With_Wrong_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Deleted},
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Deleted}
            };

            //Act

            var result = nullCollection.WithStatuses(ProjectStatus.Active);

            //Assert

            Assert.Empty(result);
        }

        [Fact]
        public void Recive_Collection_When_Filtering_Collection_With_Valid_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active},
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active}
            };

            //Act

            var result = nullCollection.WithStatuses(ProjectStatus.Active);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void Recive_Collection_When_Filtering_Collection_With_Valid_Multiple_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active},
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Deleted}
            };

            //Act

            var result = nullCollection.WithStatuses(ProjectStatus.Active , ProjectStatus.Deleted);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 2);
        }

        //Test : T WithoutStatuses<T>(this T model , params Status[] list)

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_Null_Entity()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = null;

            //Act

            var result = baseEntity.WithoutStatuses(ProjectStatus.Active , ProjectStatus.Deleted);

            //Assert

            Assert.Null(result);
            Assert.Null(baseEntity);
        }

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_One_Entity_With_Matching_Status()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = new BaseEntityWithProjectStatus() { Status = ProjectStatus.Deleted };
            //Act

            var result = baseEntity.WithoutStatuses(ProjectStatus.Deleted);

            //Assert

            Assert.Null(result);
            Assert.NotNull(baseEntity);
        }

        [Fact]
        public void WithoutStatuses_Recive_Entity_When_Filtering_One_Entity_With_Not_Matching_Status()
        {
            //Arrange

            BaseEntityWithProjectStatus baseEntity = new BaseEntityWithProjectStatus() { Status = ProjectStatus.Active };

            //Act

            var result = baseEntity.WithoutStatuses(ProjectStatus.Deleted);

            //Assert

            Assert.NotNull(result);
            Assert.NotNull(baseEntity);
        }

        //Test : IEnumerable<T> WithoutStatuses<T>(this IEnumerable<T> modelList , params Status[] list)

        [Fact]
        public void WithoutStatuses_Recive_Null_When_Filtering_Null_Collection()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = null;

            //Act

            var result = nullCollection.WithoutStatuses(ProjectStatus.Deleted);

            Assert.Null(result);
            Assert.Null(nullCollection);
        }

        [Fact]
        public void WithoutStatuses_Recive_Empty_Collection_When_Filtering_Collection_With_Matching_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Deleted},
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active}
            };

            //Act

            var result = nullCollection.WithoutStatuses(ProjectStatus.Active , ProjectStatus.Deleted);

            //Assert

            Assert.Empty(result);
        }

        [Fact]
        public void WithoutStatuses_Recive_Collection_When_Filtering_Collection_With_Not_Matching_Status()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active},
            };

            //Act

            var result = nullCollection.WithoutStatuses(ProjectStatus.Deleted);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 1);
        }

        [Fact]
        public void WithoutStatuses_Recive_Collection_When_Filtering_Collection_With_Valid_Multiple_Statuses()
        {
            //Arrange

            IEnumerable<BaseEntityWithProjectStatus> nullCollection = new List<BaseEntityWithProjectStatus>()
            {
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Active},
                new BaseEntityWithProjectStatus() {Status = ProjectStatus.Deleted},
            };

            //Act

            var result = nullCollection.WithoutStatuses(ProjectStatus.Active);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count() == 1);
        }
    }
}