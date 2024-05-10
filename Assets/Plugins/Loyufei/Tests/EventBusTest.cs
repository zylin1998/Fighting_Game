using System;
using System.Linq;
using NUnit.Framework;
using Zenject;

namespace Loyufei.DomainEvents.TestUnit
{
    [TestFixture]
    public class EventBusTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<IDomainEvent>();
            Container.BindInterfacesAndSelfTo<DomainEventBus>().AsSingle();
        }

        [Test]
        public void CreateDomainEventBus()
        {
            var eventBus = Container.Resolve<DomainEventBus>();
            var callBacks = eventBus.Mapping;

            Assert.NotNull(callBacks);
        }

        [Test]
        public void RegisterAndTriggerTest()
        {
            var service = Container.Resolve<DomainEventService>();
            var greet   = new GreetTrigger(service);

            service.Register<GreetEvent>(Greeting);

            greet.Greeting();
        }

        [Test]
        public void GreetEventCreateTest()
        {
            var greet = new GreetEvent(Name);

            Assert.AreEqual(string.Format(GreetFormat, Name), greet.Greet);
        }

        private void Greeting(GreetEvent greet)
        {
            Assert.AreEqual(string.Format(GreetFormat, Name), greet.Greet);
        }

        private static string Name = "John";
        private static string GreetFormat = "Hello, {0}!";

        private class GreetEvent : DomainEventBase
        {
            public string Greet { get; set; }

            public GreetEvent(string name) : base()
            {
                Greet = string.Format(GreetFormat, name);
            }
        }

        private class GreetTrigger : AggregateRoot
        {
            public GreetTrigger(DomainEventService service) : base(service) { }

            public void Greeting()
            {
                AddEvent(new GreetEvent(Name));

                DomainEventService.Post(this);
            }
        }

        private class GreetHandler : EventHandler<GreetEvent>
        {
            public GreetHandler(Action<GreetEvent> action) : base(action) { }

            public override void Invoke(GreetEvent eventData)
            {
                Assert.AreEqual(typeof(GreetEvent), eventData.GetType());

                base.Invoke(eventData);
            }
        }
    }
}