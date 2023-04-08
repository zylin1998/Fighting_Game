using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.InputSystem;

public class MobileDemo : MonoBehaviour, IInputClient
{
    [SerializeField]
    private InputList _InputList;

    private void Awake()
    {
        InputClient.SetBasic(this);
        InputClient.SetPlatform(RuntimePlatform.Android);
        InputClient.SetInput(this._InputList);
    }

    #region IInputClient

    public void GetValue() 
    {
        var x = InputClient.GetAxis("Horizontal");
        var y = InputClient.GetAxis("Vertical");

        Debug.Log(new Vector2(x, y));

        if (InputClient.GetKeyDown("Attack")) { Debug.Log("Attack"); }
        if (InputClient.GetKeyDown("Jump")) { Debug.Log("Jump"); }
        if (InputClient.GetKeyDown("Flash")) { Debug.Log("Flash"); }
        if (InputClient.GetKeyDown("Skill 1")) { Debug.Log("Skill 1"); }
        if (InputClient.GetKeyDown("Skill 2")) { Debug.Log("Skill 2"); }
        if (InputClient.GetKeyDown("Skill 3")) { Debug.Log("Skill 3"); }
    }

    #endregion

    private void OnDestroy()
    {
        InputClient.Clear();
    }
}
