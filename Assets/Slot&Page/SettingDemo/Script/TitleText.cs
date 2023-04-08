using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Page;

public class TitleText : MonoBehaviour
{
    private Text _Text;
    private string _GroupName = "Setting";

    private void Awake()
    {
        this._Text = this.GetComponentInChildren<Text>();
    }

    void Start()
    {
        PageClient.CallBack += SetTitle;
    }

    private void SetTitle() 
    {
        var group = PageClient.Current?.GroupName;

        if (group == this._GroupName) 
        {
            var title = PageClient.Current.Current.PageName;

            this._Text.text = title != "Main" ? title : this._GroupName; 
        }
    }

    private void OnDestroy()
    {
        PageClient.CallBack -= SetTitle;
    }
}
