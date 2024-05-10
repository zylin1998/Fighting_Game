using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Mission
{
    [CreateAssetMenu(fileName = "Mission List", menuName = "Loyufei/Mission/Mission List", order = 1)]
    public class MissionList :
        ScriptableObject,
        IMultiplexer<IForm<IMission>>,
        IIdentity,
        IExtract,
        IEnumerable<IForm>
    {
        [SerializeField]
        private string                  _Identity;
        [SerializeField]    
        private string                  _RepositoryIdentity;
        [SerializeField]
        private MissionHandler.Settings _Settings;
        [SerializeField]
        private List<MissionForm>       _Forms;

        public object Identity => _Identity;

        public MissionHandler.Settings Settings => _Settings;

        public IForm<IMission> this[object identity] 
            => _Forms.FirstOrDefault(form => form.Identity.Equals(identity));

        public IForm<IMission> this[IIdentity identity]
            => _Forms.FirstOrDefault(form => form.Identity.Equals(identity.Identity));

        public bool TryGet(object identify, out IForm<IMission> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public bool TryGet(IIdentity identify, out IForm<IMission> result)
        {
            result = this[identify];

            return !result.IsDefault();
        }

        public virtual object Extract() 
        {
            return new Repositories(_RepositoryIdentity, _Forms.Select(form => form.Extract().To<MissionForm.Repository>()));
        }

        public IEnumerator<IForm> GetEnumerator()
            => _Forms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [Serializable]
        public class Repositories : 
            MultiFlexibleRepository<EMissionState, MissionReposit, MissionForm.Repository>,
            IIdentity
        {
            [SerializeField]
            private string _Identity;

            public object Identity => _Identity;

            public Repositories(string identity, IEnumerable<MissionForm.Repository> repositories) 
            {
                _Identity = identity;
                _Elements = repositories.ToArray();
            }
        }
    }
}
