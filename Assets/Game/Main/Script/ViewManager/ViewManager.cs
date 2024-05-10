using DG.Tweening;
using Loyufei;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame.ViewManagement
{
    public enum EShowViewMode
    {
        Single = 0,
        Additive = 1,
    }

    public class ViewManager
    {
        public class Group
        {
            public Group()
            {
                Views = new();
            }

            public Dictionary<object, IView> Views { get; }

            public IView Current { get; private set; }

            public void Register(object key, IView view)
            {
                Views.Add(key, view);
            }

            public bool Unregister(object key)
            {
                return Views.Remove(key);
            }

            public void Show(object key, Action onStart, Action onComplete, EShowViewMode viewMode = EShowViewMode.Single)
            {
                switch (viewMode)
                {
                    case EShowViewMode.Single:
                        ShowSingle(key, onStart, onComplete); break;
                    case EShowViewMode.Additive:
                        ShowAdditive(key, onStart, onComplete); break;
                    default:
                        Debug.Log("Show View Mode Error"); break;
                }
            }

            public void ShowSingle(object key, Action onStart, Action onComplete)
            {
                if (Current.IsDefault()) 
                { 
                    ShowAdditive(key, onStart, onComplete);

                    return;
                }

                CloseCurrent().OnComplete(() =>
                {
                    if(Current is Component component) component.gameObject.SetActive(false); 

                    ShowAdditive(key, onStart, onComplete);
                });
            }

            public Tween ShowAdditive(object key, Action onStart, Action onComplete)
            {
                Views.TryGetValue(key, out IView view);

                Current = view;

                return view
                        .Open()
                        .OnStart(() =>
                        {
                            if (view is Component component) { component.gameObject.SetActive(true); }

                            onStart?.Invoke();
                        })
                        .OnComplete(() => onComplete?.Invoke());
            }

            public Tween CloseCurrent()
            {
                return Current?.Close();
            }
        }

        public Dictionary<object, Group> Groups { get; } = new();
    }

    public static class ViewManagerExtensions
    {
        public static void Register(this ViewManager self, object groupId, object viewId, IView view) 
        {
            var group = self.Groups.GetorAdd(groupId, () => new ViewManager.Group());

            group.Register(viewId, view);
        }

        public static bool Unregister(this ViewManager self, object groupId, object viewId) 
        {
            var group = self.Groups.GetorAdd(groupId, () => new ViewManager.Group());

            return group.Unregister(viewId);
        }

        public static bool Unregister(this ViewManager self, object groupId)
        {
            return self.Groups.Remove(groupId);
        }

        public static void Show(this ViewManager self, object groupId, object viewId, Action onStart, Action onComplete, EShowViewMode viewMode = EShowViewMode.Single) 
        {
            if (self.Groups.TryGetValue(groupId, out var group))
            {
                group.Show(viewId, onStart, onComplete, viewMode);
            }
        }

        public static Tween CloseCurrent(this ViewManager self, object groupId) 
        {
            return self.Groups.TryGetValue(groupId, out var group) ? group.CloseCurrent() : default;
        }
    }
}