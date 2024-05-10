using DG.Tweening;
using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using FightingGame.ViewManagement;
using Zenject;

namespace FightingGame.TitleScene
{
    public class QuestMenuView : ViewMono
    {
        [SerializeField]
        private CanvasGroup _CanvasGroup;
        [SerializeField, Range(0.1f, 1f)]
        private float _FadeDuration;
        [SerializeField]
        private List<Button> _QuestSelects;

        protected override void Awake()
        {
            base.Awake();

            _QuestSelects = GetComponentsInChildren<Button>().ToList();

            _QuestSelects.ForEach(b => b.AddListener(() => InvokeCallBack(b)));
        }

        protected override void Construction(ViewManager manager)
        {
            var groupId = TitleGroupNames.Group;
            var viewId  = TitleGroupNames.Quest;

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

        private Action<int> _CallBack;

        public event Action<int> onClick 
        {
            add    { _CallBack += value; }

            remove { _CallBack -= value; }
        }

        private void InvokeCallBack(Button button) 
        {
            _CallBack?.Invoke(_QuestSelects.IndexOf(button));
        }
    }
}