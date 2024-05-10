using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;
using Loyufei.DomainEvents;

namespace Loyufei.Quest.Test
{
    using static QuestTests;

    public class QuestWatcherTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            var ruleList = new TUnitRuleList(new IRule[]
            {
                new CountRule(5),
                new CountRule(10),
                new CountRule(15),
                new CountRule(20),
            });

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<IDomainEvent>();
            Container.BindInterfacesAndSelfTo<DomainEventBus>().AsSingle();
            Container.Bind<DomainEventService>().AsSingle().NonLazy();

            Container.Bind<QuestWatcher>().AsSingle().WithArguments(ruleList).NonLazy();
            Container.Bind<CountQuest>().AsSingle().NonLazy();
        }

        [Test]
        public void ConstructTest() 
        {
            var eventBus     = Container.Resolve<IDomainEventBus>();
            var questManager = Container.Resolve<QuestWatcher>();
            var quest        = Container.Resolve<CountQuest>();

            Assert.IsNotNull(eventBus);
            Assert.IsNotNull(questManager);
            Assert.IsNotNull(quest);

            Assert.AreEqual(1, questManager.Mapping.Count);
            Assert.AreEqual(4, questManager.Mapping[typeof(ICount)].Count);
        }

        [Test]
        public void QuestManagerTest() 
        {
            var service      = Container.Resolve<DomainEventService>();
            var questManager = Container.Resolve<QuestWatcher>();
            var quest        = Container.Resolve<CountQuest>();
            var trigger      = new CountEventTrigger(service);

            for (var i = 0; i <= 20; i++) 
            {
                trigger.Trigger(i);
                Assert.AreEqual(i, quest.Count);
            }

            questManager.Mapping[typeof(ICount)].ForEach(rule =>
                Assert.AreEqual(true, ((CountRule)rule).IsClear));
        }

        public class TUnitRuleList : IRuleList 
        {
            public List<IRule> Rules { get; }

            public TUnitRuleList(IEnumerable<IRule> rules) 
            {
                Rules = rules.ToList();
            }

            public RuleMapping Mapping => Rules.TypeMapping<IRule, RuleMapping>(rule => 
            {
                var ruleGeneric = rule.GetType().GetInterfaces().First(type => type.Name == "IRule`1");

                return ruleGeneric?.GetGenericArguments().First();
            });
        }
    }
}