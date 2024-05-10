using FightingGame.Player;
using Loyufei;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace FightingGame.TitleScene
{
    public class QuestMenuModel : MonoBehaviour
    {
        private IEntityForm _QuestInfo;
        private IRepository _QuestData;

        [Inject]
        public void Construction(
            [Inject(Id = "Quest")] IEntityForm entityForm, 
            [Inject(Id = "Quest")] IRepository questData) 
        {
            _QuestInfo = entityForm;
            _QuestData = questData;
        }

        public IEnumerable<(IReposit<bool> clear, IEntity<QuestInfo> info)> GetQuestInfo() 
        {
            return _QuestInfo.Select(e => (_QuestData.SearchAt(e.Identity).To<IReposit<bool>>(), e.To<IEntity<QuestInfo>>()));
        }

        public IEntity<QuestInfo> GetQuestInfo(int id) 
        {
            return _QuestInfo[id]?.Data.To<IEntity<QuestInfo>>();
        }
    }
}