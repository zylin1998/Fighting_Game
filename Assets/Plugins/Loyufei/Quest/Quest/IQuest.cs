using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Quest
{
    public interface IQuestInfo : IIdentity 
    {
        public object Data    { get; }

        public IEnumerable<RewardInfo> RewardInfos { get; }
    }

    public interface IQuest
    {
        public string     Chapter { get; }
        public IQuestInfo QuestInfo { get; }

        public IQuestResult GetResult(EQuestState questState);
    }

    public struct QuestEvent : IDomainEvent 
    {
        public IQuest Quest      { get; }
        public float  InvokeTime { get; }
        public object Source     { get; }

        public QuestEvent(IQuest quest) 
        {
            Quest      = quest;
            InvokeTime = Time.realtimeSinceStartup;
            Source     = Quest;
        }
    }

    public struct QuestResult : IQuestResult
    {
        public QuestResult(EQuestState state, object preserve, IQuest source) 
        {
            var info = source.QuestInfo;

            Chapter    = source.Chapter;
            Identity   = info.Identity;
            Rewards    = info.RewardInfos.ToList();
            QuestState = state;
            Preserve   = preserve;
            Source     = source;
            InvokeTime = Time.realtimeSinceStartup;
        }

        public string      Chapter    { get; }
        public object      Identity   { get; }
        public EQuestState QuestState { get; }
        public object      Preserve   { get; }
        public float       InvokeTime { get; }
        public object      Source     { get; }

        public List<RewardInfo> Rewards { get; }
    }

    public interface IQuestResult : IDomainEvent
    {
        public string           Chapter    { get; }
        public object           Identity   { get; }
        public object           Preserve   { get; }
        public EQuestState      QuestState { get; }
        public List<RewardInfo> Rewards    { get; }
    }
}