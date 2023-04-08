using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Page;

public class BackButton : MonoBehaviour
{
    private Button _Button;
    private string _GroupName = "Setting";

    private void Awake()
    {
        this._Button = this.GetComponent<Button>();
    }

    private void Start()
    {
        this._Button.onClick.AddListener(() => 
        {
            var group = PageClient.Current;

            if (group?.GroupName == this._GroupName) 
            {
                if (group.Current.PageName == "Main") { PageClient.CloseGroup("Setting"); }

                else { PageClient.OpenPage(group.GroupName, "Main"); }
            }
        });
    }
}
