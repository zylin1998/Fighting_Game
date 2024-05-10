using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Quest
{
    [Serializable]
    public class QuestInfoForm<TQuestInfo, TPreserve> : FormBase<TQuestInfo>, IExtract 
        where TQuestInfo : IQuestInfo
    {
        public QuestInfoForm(string category, IEnumerable<TQuestInfo> items) : base(category, items)
        {
            
        }

        public object Extract()
        {
            var identtity = Identity;
            var reposits  = _Items.ConvertAll(r => new RepositBase<TPreserve>((string)r.Identity, default(TPreserve)));
            
            return new QuestInfoRepository<TPreserve>((string)Identity, reposits); 
        }
    }

    [Serializable]
    public class QuestInfoRepository<TPreserve> : IdentifiedRepository<TPreserve> 
    {
        public QuestInfoRepository() : base()
        {

        }

        public QuestInfoRepository(string identify, IEnumerable<RepositBase<TPreserve>> reposits) 
        {
            _Identity = identify;
            _Reposits = reposits.ToList();
        }
    }
}
