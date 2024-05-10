using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Quest
{
    public class QuestInfoHandler : AggregateRoot
    {
        public QuestInfoHandler(
            IMultiplexer       questInfoList,
            IMultiplexer       questRepositories,
            DomainEventService service) : base(service) 
        {
            QuestInfoList     = questInfoList; ;
            QuestRepositories = questRepositories;

            DomainEventService.Register<QuestInfoAllRequest>(GetAllQuestInfo);
            DomainEventService.Register<QuestResult>(GetResult);
        }

        public IMultiplexer QuestInfoList     { get; }
        public IMultiplexer QuestRepositories { get; }

        public void GetAllQuestInfo(QuestInfoAllRequest request) 
        {
            var dic        = new Dictionary<object, IEnumerable<(IQuestInfo info, object state)>>();
            var enumerable = QuestInfoList.To<IEnumerable>();

            foreach (IForm form in enumerable) 
            {
                var category   = form.Identity;
                var repository = QuestRepositories[category].To<IRepository>();

                var list = form.GetInfos(identity =>
                {
                    var info  = identity.To<IQuestInfo>();
                    var state = repository.SearchAt(identity.Identity).To<IReposit>();

                    return (info, state.Data);
                });

                dic.Add(category, list);
            }

            this.SettleEvents(new QuestInfoAllResponse(dic, this));
        }

        public void GetResult(QuestResult result) 
        {
            var chapter    = result.Chapter;
            var identity   = result.Identity;
            var repository = QuestRepositories[chapter].To<IRepository>();
            var reposit    = repository.SearchAt(identity).To<IReposit>();
            var preserve   = result.Preserve;
            
            if (reposit.Data != preserve)
            {
                reposit.Preserve(preserve);
            }
        }
    }

    public struct QuestInfoRequest : IDomainEvent
    {
        public QuestInfoRequest(string category, object source) 
        {
            Category   = category;
            InvokeTime = Time.realtimeSinceStartup;
            Source     = source;
        }

        public float  InvokeTime { get; }
        public object Source     { get; }
        public string Category   { get; }
    }

    public struct QuestInfoAllRequest : IDomainEvent
    {
        public QuestInfoAllRequest(object source)
        {
            InvokeTime = Time.realtimeSinceStartup;
            Source     = source;
        }

        public float  InvokeTime { get; }
        public object Source     { get; }
    }

    public struct QuestInfoAllResponse : IDomainEvent
    {
        public QuestInfoAllResponse(
            Dictionary<object, IEnumerable<(IQuestInfo info, object state)>> quests, 
            object source)
        {
            Quests     = quests;
            InvokeTime = Time.realtimeSinceStartup;
            Source     = source;
        }

        public Dictionary<object, IEnumerable<(IQuestInfo info, object state)>> Quests { get; }
        public float  InvokeTime { get; }
        public object Source     { get; }
    }
}