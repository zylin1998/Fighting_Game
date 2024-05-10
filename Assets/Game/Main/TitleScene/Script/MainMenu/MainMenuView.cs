using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FightingGame.ViewManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using Loyufei;

namespace FightingGame.TitleScene
{
    public class MainMenuView : ViewMono
    {
        [SerializeField]
        private CanvasGroup _CanvasGroup;
        [SerializeField, Range(0.1f, 1f)]
        private float _FadeDuration;
        [SerializeField]
        private Button _Quest;
        [SerializeField]
        private Button _Setting;
        [SerializeField]
        private Button _Quit;

        protected override void Construction(ViewManager manager)
        {
            var groupId = TitleGroupNames.Group;
            var viewId  = TitleGroupNames.Main;

            manager.Register(groupId, viewId, this);
        }

        public override Tween Open()
        {
            _CanvasGroup.alpha = 0;

            return _CanvasGroup.DOFade(1f, _FadeDuration);
        }

        public override Tween Close()
        {
            _CanvasGroup.alpha = 1;

            return _CanvasGroup.DOFade(0f, _FadeDuration);
        }

        public void SetClick(UnityAction quest, UnityAction setting, UnityAction quit) 
        {
            _Quest.AddListener(quest);
            _Setting.AddListener(setting);
            _Quit.AddListener(quit);
        }
    }
}