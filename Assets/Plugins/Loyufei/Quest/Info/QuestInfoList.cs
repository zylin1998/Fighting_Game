using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Quest
{
    public class QuestInfoList<TQuestInfo, TPreserve> :
        ScriptableObject,
        IMultiplexer<IForm<TQuestInfo>>,
        IIdentity,
        IExtract,
        IEnumerable<IForm> 
        where TQuestInfo : IQuestInfo
    {
        [SerializeField]
        private string              _Identity;
        [SerializeField]
        private string              _RepositoryIdentifier;
        [SerializeField]
        private List<QuestInfoForm<TQuestInfo, TPreserve>> _Forms;

        public object Identity => _Identity;

        public IForm<TQuestInfo> this[object identify] 
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify));

        public IForm<TQuestInfo> this[IIdentity identify]
            => _Forms.FirstOrDefault(f => f.Identity.Equals(identify.Identity));

        public bool TryGet(object identify, out IForm<TQuestInfo> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out IForm<TQuestInfo> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public object Extract() 
        {
            var identity     = _RepositoryIdentifier;
            var repositories = _Forms.Select(f => f.Extract().To<QuestInfoRepository<TPreserve>>());

            return new Repositories(identity, repositories); 
        }

        public IEnumerator<IForm> GetEnumerator() 
            => _Forms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [Serializable]
        public class Repositories : MultiplexerBase<QuestInfoRepository<TPreserve>>, IIdentity
        {
            public Repositories() 
            {
                _Elements = new QuestInfoRepository<TPreserve>[0];
            }

            public Repositories(string identity, IEnumerable<QuestInfoRepository<TPreserve>> repositories)
            {
                _Identity = identity;
                _Elements = repositories.ToArray();
            }

            [SerializeField]
            private string _Identity;

            public object Identity => _Identity;
        }
    }
}