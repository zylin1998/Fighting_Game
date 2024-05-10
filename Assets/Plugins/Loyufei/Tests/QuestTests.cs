using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;
using Loyufei.DomainEvents;

namespace Loyufei.Quest.Test
{
    public class QuestTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<IDomainEvent>();
            Container.BindInterfacesAndSelfTo<DomainEventBus>().AsSingle().NonLazy();
            Container.Bind<DomainEventService>().AsSingle().NonLazy();

            Container.Bind<CountQuest>().AsSingle().NonLazy();
        }

        [Test]
        public void QuestTestsSimplePasses()
        {
            var achievement  = 10;
            var quest        = Container.Resolve<CountQuest>();
            var service      = Container.Resolve<DomainEventService>();
            var eventTrigger = new CountEventTrigger(service);
            var rule         = new CountRule(achievement);

            Assert.IsNotNull(service);
            Assert.IsNotNull(quest);

            for (var i = 0; i <= achievement; i++)
            {
                eventTrigger.Trigger(i);

                Assert.AreEqual(i, quest.Count);
                Assert.AreEqual(
                    i < achievement ? EQuestState.Progress : EQuestState.Fulfilled, 
                    rule.CheckRule(quest));
            }
        }

        public class CountQuest : QuestRoot, ICount
        {
            public int Count { get; private set; }

            public CountQuest(DomainEventService service) : base(service)
            {
                DomainEventService.Register<CountEvent>(SetCount);
            }

            public void SetCount(CountEvent count)
            {
                Count = count.Count;

                AddEvent(new QuestEvent(this));

                DomainEventService.Post(this);
            }

            public override IQuestResult GetResult(EQuestState state)
            {
                return new QuestResult(state, null, this);
            }
        }

        public class CountEvent : DomainEventBase
        {
            public int Count { get; }

            public CountEvent(int count) : base()
            {
                Count = count;
            }
        }

        

        public class CountEventTrigger : AggregateRoot
        {
            public CountEventTrigger(DomainEventService service) : base(service)
            {

            }

            public void Trigger(int count)
            {
                AddEvent(new CountEvent(count));

                DomainEventService.Post(this);
            }
        }

        public class CountRule : IRule<ICount>
        {
            public CountRule(int achievement)
            {
                Achievement = achievement;
            }

            public int Achievement { get; }
            
            public bool IsClear { get; private set; }

            public EQuestState CheckRule(ICount pending)
            {
                if (IsClear) { return EQuestState.None; }

                if (pending.Count < 0) { return EQuestState.Defeated; }

                if (pending.Count < Achievement) { return EQuestState.Progress; }
                
                IsClear = true;

                return EQuestState.Fulfilled;
            }
        }

        public interface ICount
        {
            int Count { get; }
        }
    }
}