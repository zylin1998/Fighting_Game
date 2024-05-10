using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FightingGame.ViewManagement;

namespace FightingGame.TitleScene
{
    public class SettingMenuView : ViewMono
    {
        [SerializeField]
        private CanvasGroup _CanvasGroup;
        [SerializeField, Range(0.1f, 1f)]
        private float _FadeDuration;

        protected override void Construction(ViewManager manager)
        {
            var groupId = TitleGroupNames.Group;
            var viewId  = TitleGroupNames.Setting;

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
    }
}