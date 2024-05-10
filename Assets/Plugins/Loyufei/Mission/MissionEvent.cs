using Loyufei.DomainEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Mission 
{
    public struct MissionRequest : IDomainEvent 
    {
        public MissionRequest(object list, object form)
        {
            List       = list;
            Form       = form;
            InvokeTime = Time.realtimeSinceStartup;
        }

        public object List       { get; }
        public object Form       { get; }
        public float  InvokeTime { get; }
    }

    public struct MissionResponse : IDomainEvent
    {
        public MissionResponse(
            object list,
            object form, 
            IEnumerable<(IMission mission, EMissionState state)> missionInfos) 
        {
            List       = list;
            Form       = form;
            InvokeTime = Time.realtimeSinceStartup;
            MissionInfos = missionInfos;
        }

        public object List       { get; }
        public object Form       { get; }
        public float  InvokeTime { get; } 

        public IEnumerable<(IMission mission, EMissionState state)> MissionInfos { get; }
    }

    public struct AllMissionRequest : IDomainEvent
    {
        public AllMissionRequest(object list)
        {
            List       = list;
            InvokeTime = Time.realtimeSinceStartup;
        }

        public object List       { get; }
        public float  InvokeTime { get; }
    }

    public struct AllMissionResponse : IDomainEvent
    {
        public AllMissionResponse(
            object list,
            IEnumerable<(object form, IMission mission, EMissionState state)> missionInfos)
        {
            List         = list;
            InvokeTime   = Time.realtimeSinceStartup;
            MissionInfos = missionInfos;
        }

        public object List       { get; }
        public float  InvokeTime { get; }

        public IEnumerable<(object form, IMission mission, EMissionState state)> MissionInfos { get; }
    }

    public struct MissionEndEvent : IDomainEvent 
    {
        public MissionEndEvent(object list, object form, object identity) 
        {
            List     = list;
            Form     = form;
            Identity = identity;

            InvokeTime = Time.realtimeSinceStartup;
        }

        public object List         { get; }
        public object Form         { get; }
        public object Identity     { get; }
        public float  InvokeTime   { get; }
    }

    public struct MissionCheckEvent : IDomainEvent 
    {
        public MissionCheckEvent(string list, string form, object pending) 
        {
            List       = list;
            Form       = form;
            Pending    = pending;
            InvokeTime = Time.realtimeSinceStartup;
        }

        public string List       { get; }
        public string Form       { get; }
        public object Pending    { get; }
        public float  InvokeTime { get; }
    }
}