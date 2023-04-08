using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Page
{
    public class PageClient : MonoBehaviour
    {
        public static IPageCollect Current { get; private set; }
        public static System.Action CallBack { get; set; }
        public static Dictionary<string, IPageCollect> Groups { get; private set; }

        private void Awake()
        {
            if (Groups == null)
            {
                Groups = new Dictionary<string, IPageCollect>();
            }

            CallBack = () => { };
        }

        #region Group Operate

        public static bool AddGroup(PageCollect collect)
        {
            if (Groups == null) 
            {
                Groups = new Dictionary<string, IPageCollect>(); 
            }
            
            return Groups.TryAdd(collect.GroupName, collect);
        }

        public static bool RemoveGroup(string group)
        {
            return Groups.Remove(group);
        }

        public static void OpenGroup(string group)
        {
            OpenGroup(group, "Main");
        }

        public static void OpenGroup(string group, string page)
        {
            Current = Groups[group];
            Current.OpenPage(page);
            CallBack?.Invoke();
        }

        public static void CloseGroup(string group)
        {
            if (Current.GroupName == group)
            {
                Current.CloseAll();
                Current = null;
            }
        }


        #endregion

        #region OperatePage

        public static void AddPage(IPageState page)
        {
            Groups[page.GroupName].Add(page);
        }

        public static bool RemovePage(IPageState page)
        {
            return Groups[page.GroupName].Remove(page);
        }

        public static void OpenPage(PageSlot slot)
        {
            OpenPage(slot.Group, slot.Page);
        }

        public static void OpenPage(string group, string page)
        {
            if (Current.GroupName == group)
            {
                Current.OpenPage(page);
                CallBack?.Invoke();
            }
        }

        #endregion
    }
}