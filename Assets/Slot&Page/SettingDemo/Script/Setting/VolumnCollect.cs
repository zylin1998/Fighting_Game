using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rate 
{
    [SerializeField]
    private float _Max;
    [SerializeField]
    private float _Min;

    public float Max => this._Max;
    public float Min => this._Min;

    public float Clamp(float value) 
    {
        return Mathf.Clamp(value, this._Min, this._Max);
    }

    public bool IsClamp(float value) 
    {
        return value >= this._Min && value <= this._Max;
    }
}

[CreateAssetMenu(fileName = "Volumn Collection", menuName = "Setting/Collection/Volumn", order = 1)]
public class VolumnCollect : SettingCollect
{
    [SerializeField]
    private Rate _Db;

    public Rate Db => this._Db;
}
