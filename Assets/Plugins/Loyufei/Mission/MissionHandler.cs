using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.DomainEvents;

namespace Loyufei.Mission
{
    public class MissionHandler : AggregateRoot
    {
        public IMultiplexer MissionList         { get; }
        public IMultiplexer MissionRepositories { get; }
        public Settings     Setting             { get; }

        public Dictionary<object, DynamicStore> Dictionary { get; }

        public MissionHandler(
            IMultiplexer    list, 
            IMultiplexer    repositories, 
            Settings        setting,
            DomainEventService service)
            : base(service)
        {
            MissionList         = list;
            MissionRepositories = repositories;
            Setting             = setting;

            Dictionary = new Dictionary<object, DynamicStore>();

            //Init();

            service.Register<AllMissionRequest>(GetAllMissionInfo);
            service.Register<MissionRequest>   (GetMissionInfo);
            service.Register<MissionEndEvent>  (MissionStateChange);
        }

        protected virtual void Init() 
        {
            foreach (IForm form in MissionList.To<IEnumerable>())
            {
                var repository = MissionRepositories[form.Identity]
                    .To<IRepository>();
                var identity   = form.Identity;
                var infos      = form.GetInfos(item =>
                {
                    var mission = item.To<IMission>();
                    var reposit = repository.SearchAt(item.Identity);

                    return new DynamicStore.Tuple(mission, reposit);
                });

                Dictionary.Add(identity, new DynamicStore(identity, infos));
            }
        }

        public void GetAllMissionInfo(AllMissionRequest request)
        {
            var list = request.List;

            if (list != MissionList.To<IIdentity>().Identity) { return; }

            var progress = new List<(object form, IMission mission, EMissionState state)>();
            var complete = new List<(object form, IMission mission, EMissionState state)>();
            var end      = new List<(object form, IMission mission, EMissionState state)>();
            var hidden   = Setting.HiddenSameGroup;
            
            foreach (DynamicStore store in Dictionary.Values)
            {
                var form = store.Form;

                var c = store
                    .GetTuples(EMissionState.Complete, t => (form, t.Mission, (EMissionState)t.Reposit.Data))
                    .ToList();
                var p = store
                    .GetTuples(EMissionState.Progress, t => (form, t.Mission, (EMissionState)t.Reposit.Data))
                    .ToList();
                var e = store
                    .GetTuples(     EMissionState.End, t => (form, t.Mission, (EMissionState)t.Reposit.Data))
                    .ToList();

                complete.AddRange(c.Any() ? c.GetRange(0, hidden ? 1 : c.Count) : new());
                progress.AddRange(p.Any() ? p.GetRange(0, hidden ? 1 : p.Count) : new());
                end     .AddRange(e.Any() ? e.GetRange(0, hidden ? 1 : e.Count) : new());
            }

            var result = complete.Concat(progress).Concat(end);

            this.SettleEvents(new AllMissionResponse(list, result));
        }

        public void GetMissionInfo(MissionRequest request)
        {
            var list = request.List;
            var form = request.Form;

            if (list != MissionList.To<IIdentity>().Identity) { return; }

            if (!Dictionary.TryGetValue(form, out var store)) { return; }

            var progress = new List<(IMission mission, EMissionState state)>();
            var complete = new List<(IMission mission, EMissionState state)>();
            var end      = new List<(IMission mission, EMissionState state)>();
            var hidden   = Setting.HiddenSameGroup;

            var c = store
                .GetTuples(EMissionState.Complete, t => (t.Mission, (EMissionState)t.Reposit.Data))
                .ToList();
            var p = store
                .GetTuples(EMissionState.Progress, t => (t.Mission, (EMissionState)t.Reposit.Data))
                .ToList();
            var e = store
                .GetTuples(EMissionState.End, t => (t.Mission, (EMissionState)t.Reposit.Data))
                .ToList();

            complete.AddRange(c.Any() ? c.GetRange(0, hidden ? 1 : c.Count) : new());
            progress.AddRange(p.Any() ? p.GetRange(0, hidden ? 1 : p.Count) : new());
            end     .AddRange(e.Any() ? e.GetRange(0, hidden ? 1 : e.Count) : new());

            var result = complete.Concat(progress).Concat(end);

            this.SettleEvents(new MissionResponse(list, form, result));
        }

        public void MissionStateChange(MissionEndEvent missionEnd)
        {
            var list     = missionEnd.List;
            var form     = missionEnd.Form;
            var identity = missionEnd.Identity;

            if (list != MissionList.To<IIdentity>().Identity) { return; }

            var store    = Dictionary[form];
            var end      = store[EMissionState.End];
            var complete = store[EMissionState.Complete];
            var tuple    = complete
                .FirstOrDefault(t => t.Reposit.Identity == identity);
            var reposit  = tuple.Reposit;

            reposit.Preserve(EMissionState.End);

            complete.Remove(tuple);
            end     .Add(tuple);
        }

        public void MissionCheck(MissionCheckEvent check) 
        {
            var list    = check.List;
            var form    = check.Form;
            var pending = check.Pending;

            if (!MissionList.To<IIdentity>().Identity.IsEqual(list)) { return; }

            var store    = Dictionary[form];
            var progress = store[EMissionState.Progress];
            var complete = store[EMissionState.Complete];

            complete.AddRange(progress.Where(t =>
            {
                var state = t.Mission.Checking(pending);

                if (state == EMissionState.Complete)
                {
                    t.Reposit.Preserve(state);

                    return true;
                }

                return false;
            }));

            progress.RemoveAll(t => t.Reposit.Data.IsEqual(EMissionState.Complete));
        }

        public class DynamicStore
        {
            public struct Tuple
            {
                public Tuple(IMission mission, IReposit reposit) 
                {
                    Mission = mission;
                    Reposit = reposit;
                }

                public IMission Mission { get; }
                public IReposit Reposit { get; }
            }

            public DynamicStore(object form, IEnumerable<Tuple> tuples) 
            {
                Form   = form;
                Tuples = new Dictionary<EMissionState, List<Tuple>>()
                {
                    { EMissionState.Progress, new List<Tuple>() },
                    { EMissionState.Complete, new List<Tuple>() },
                    { EMissionState.End     , new List<Tuple>() },
                };

                tuples.ForEach(t =>
                {
                    Tuples[(EMissionState)t.Reposit.Data].Add(t);
                });
            }

            public List<Tuple> this[EMissionState state] => Tuples[state];

            public object Form { get; }

            public Dictionary<EMissionState, List<Tuple>> Tuples { get; }

            public IEnumerable<TOutput> GetTuples<TOutput>(EMissionState state, Func<Tuple, TOutput> selector) 
            {
                return this[state].ConvertAll(t => selector(t));
            }
        }

        [Serializable]
        public class Settings
        {
            public Settings(bool hidden) 
            {
                _HiddenSameGroup = hidden;
            }

            [SerializeField]
            private bool _HiddenSameGroup;

            public bool HiddenSameGroup => _HiddenSameGroup;
        }
    }
}