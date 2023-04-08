using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom.Page;

public class PageButton : MonoBehaviour
{
    [SerializeField]
    private string _GroupName;

    private Button _Button;

    private void Awake()
    {
        this._Button = this.GetComponent<Button>();
    }

    private void Start()
    {
        this._Button.onClick.AddListener(() =>
        {
            PageClient.OpenGroup(this._GroupName);
        });
    }
}