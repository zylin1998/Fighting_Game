using FightingGame.ViewManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame.TitleScene
{
    public class MainMenuModel : MonoBehaviour
    {
        [Inject]
        private ViewManager _View;
        
        public void QuestMenu() 
        {
            var groupId = TitleGroupNames.Group;
            var viewId  = TitleGroupNames.Quest;

            _View.Show(groupId, viewId, null, null);
        }

        public void SettingMenu()
        {
            var groupId = TitleGroupNames.Group;
            var viewId  = TitleGroupNames.Setting;

            _View.Show(groupId, viewId, null, null);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}