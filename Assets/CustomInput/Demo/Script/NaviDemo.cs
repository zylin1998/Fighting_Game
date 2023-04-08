using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Custom.Navigation;
using Custom.InputSystem;

public class NaviDemo : MonoBehaviour, IInputClient
{
    [SerializeField]
    private InputList _InputList;
    [SerializeField]
    private NaviCollect[] _Collects;

    private void Awake()
    {
        InputClient.SetBasic(this);
        InputClient.SetPlatform(RuntimePlatform.WindowsPlayer);
        InputClient.SetInput(this._InputList);
    }

    private void Start()
    {
        foreach (NaviCollect collect in this._Collects) 
        {
            collect.GetSelectables();
            collect.SetNavigation();

            collect.Selectables.ToList().ForEach(s =>
            {
                var e = s.Selectable.GetComponent<IEventCollect>();

                e.AddEvent(EventTriggerType.PointerClick, (data) => Debug.Log("Button Click"));
            });
        }

        this._Collects.FirstOrDefault().Select(INaviCollect.ESelectType.First);
    }

    public void GetValue() { }

    private void OnDestroy()
    {
        InputClient.Clear();
    }
}
