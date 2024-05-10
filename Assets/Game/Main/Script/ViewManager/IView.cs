using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace FightingGame.ViewManagement
{
    public interface IView 
    {
        public Tween Open();

        public Tween Close();
    }

    public abstract class ViewMono : MonoBehaviour, IView 
    {
        [SerializeField]
        private bool _InitActive;

        protected virtual void Awake() 
        {
            gameObject.SetActive(_InitActive);
        }

        [Inject]
        protected abstract void Construction(ViewManager manager);

        public abstract Tween Open();

        public abstract Tween Close();
    }

    public abstract class View : IView
    {
        public View()
        {

        }

        public abstract Tween Open();

        public abstract Tween Close();
    }
}