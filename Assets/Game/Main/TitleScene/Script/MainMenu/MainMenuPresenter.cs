using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FightingGame.ViewManagement;

namespace FightingGame.TitleScene
{
    public class MainMenuPresenter : MonoBehaviour
    {
        private MainMenuModel _Model;
        private MainMenuView _View;

        [Inject]
        public void Construction(MainMenuModel model, MainMenuView view, ViewManager manager)
        {
            _Model = model;
            _View  = view;

            _View.SetClick(_Model.QuestMenu, _Model.SettingMenu, _Model.Quit);

            manager.Show(TitleGroupNames.Group, TitleGroupNames.Main, null, null);
        }
    }
}