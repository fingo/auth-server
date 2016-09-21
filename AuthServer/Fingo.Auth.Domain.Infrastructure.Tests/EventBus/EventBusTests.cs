using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Xunit;

namespace Fingo.Auth.Domain.Infrastructure.Tests.EventBus
{
    public class EventBusTests
    {
        private class BaseCustomEventClass : EventBase
        {
            public static string Results { get; set; }
            public string Result { get; set; }
        }

        private class CustomEventClass : BaseCustomEventClass
        {
        }

        private class CustomEventClass1 : BaseCustomEventClass
        {
        }

        [Fact]
        public void Can_Subscribe_All()
        {
            //Arrange
            var baseCustomEventClass = new BaseCustomEventClass();
            var eventClass = new CustomEventClass();
            var eventClass1 = new CustomEventClass1();

            IEventBus eventBus = new Infrastructure.EventBus.Implementation.EventBus();

            //Act

            eventBus.SubscribeAll(eb =>
            {
                baseCustomEventClass.Result += "Received CustomEvent";
                BaseCustomEventClass.Results += "Received CustomEvent";
            });
            eventBus.Subscribe<CustomEventClass>(cec => eventClass.Result += "Received CustomEvent");
            eventBus.Subscribe<CustomEventClass1>(cec => eventClass1.Result += "Received CustomEvent");

            eventBus.Publish(eventClass);
            eventBus.Publish(eventClass1);

            //Assert

            Assert.True(eventClass.Result == "Received CustomEvent");
            Assert.True(eventClass1.Result == "Received CustomEvent");
            Assert.True(baseCustomEventClass.Result == "Received CustomEventReceived CustomEvent");
            Assert.True(BaseCustomEventClass.Results == "Received CustomEventReceived CustomEvent");
        }

        [Fact]
        public void Can_Subscribe_One()
        {
            //Arrange

            var eventClass = new CustomEventClass();
            IEventBus eventBus = new Infrastructure.EventBus.Implementation.EventBus();

            //Act

            eventBus.Subscribe<CustomEventClass>(cec => cec.Result = "Received CustomEvent");
            eventBus.Publish(eventClass);

            //Assert

            Assert.True(eventClass.Result == "Received CustomEvent");
        }

        [Fact]
        public void Dont_Recive_Message_When_Not_Subscribing_Event()
        {
            //Arrange

            var eventClass = new CustomEventClass();
            var eventClass1 = new CustomEventClass1();
            IEventBus eventBus = new Infrastructure.EventBus.Implementation.EventBus();

            //Act

            eventBus.Subscribe<CustomEventClass>(cec => eventClass.Result += "Received CustomEvent");
            eventBus.Subscribe<CustomEventClass1>(cec => eventClass1.Result += "Received CustomEvent");

            eventBus.Publish(new CustomEventClass());

            //Assert

            Assert.True(eventClass.Result == "Received CustomEvent");
            Assert.Null(eventClass1.Result);
        }
    }
}