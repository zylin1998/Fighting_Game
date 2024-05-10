using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Mission
{
    public abstract class MissionForm :
        ScriptableObject,
        IForm<IMission>,
        IExtract
    {
        [SerializeField]
        private string _Identity;

        public object Identity => _Identity;

        public abstract IMission this[string uuid] { get; }

        public abstract IEnumerable<TOutput> GetInfos<TOutput>(Func<IMission, TOutput> predicate);

        public abstract object Extract();

        [Serializable]
        public class Repository :
            IdentifiedFlexibleRepository<EMissionState, MissionReposit>
        {
            public Repository(string identity, IEnumerable<MissionReposit> reposits) : base(identity, reposits)
            {

            }
        }
    }

    public class MissionForm<TMission> : MissionForm where TMission : IMission
    {
        [SerializeField]
        private TMission[] _Missions;

        public override IMission this[string identity] 
            => _Missions.FirstOrDefault(m => (string)m.Identity == identity);

        public override IEnumerable<TOutput> GetInfos<TOutput>(Func<IMission, TOutput> predicate) 
        {
            return _Missions.Select(mission => predicate.Invoke(mission));
        }

        public override object Extract() 
        {
            return new Repository((string)Identity, _Missions.Select(item => new MissionReposit((string)item.Identity)));
        }
    }
}
