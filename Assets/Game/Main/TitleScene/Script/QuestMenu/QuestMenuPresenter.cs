using FightingGame.TitleScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class QuestMenuPresenter : MonoBehaviour
    {
        [Inject]
        public void Construction(QuestMenuModel model, QuestMenuView view) 
        {

        }
    }
}