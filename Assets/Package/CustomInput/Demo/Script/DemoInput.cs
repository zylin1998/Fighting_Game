using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;

public class DemoInput : MonoBehaviour, IInputClient
{
    [SerializeField]
    private InputList _InputList;

    private void Awake()
    {
        InputClient.SetBasic(this);
        InputClient.SetPlatform(RuntimePlatform.WindowsPlayer);
        InputClient.SetInput(this._InputList);
    }

    public void GetValue() 
    {
        var direct = new Vector2(InputClient.GetAxis("Horizontal"), InputClient.GetAxis("Vertical"));

        Debug.Log(direct);

        if (InputClient.GetKeyDown("Event")) { Debug.Log("Event"); }
        if (InputClient.GetKeyDown("Inventory")) { Debug.Log("Inventory"); }
    }

    private void OnDestroy()
    {
        InputClient.Clear();
    }
}
