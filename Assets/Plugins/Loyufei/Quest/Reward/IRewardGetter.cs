using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Quest
{
    public interface IRewardGetter
    {
        public void GetResult(IQuestResult questResult);
    }

    public interface IRewardGetter<TQuestResult> : IRewardGetter
    {
        public void GetResult(TQuestResult questResult);

        void IRewardGetter.GetResult(IQuestResult questResult) 
            => GetResult(questResult is TQuestResult result ? result : default);
    }
}