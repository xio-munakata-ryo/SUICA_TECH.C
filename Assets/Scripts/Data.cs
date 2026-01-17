using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitsType
{
    None = 0,
    Cherry,
    Strawberry,
    Grape,
    Dekopon,
    Persimmon, //かき
    Apple,
    Pear, //なし
    Peach,
    Pineapple,
    Melon,
    Watermelon,
    MAX
}

[System.Serializable]
public class Data
{
    public FruitsType Type;

    public float Radius;
    
    public FruitsController FruitsObj;

    public Data(FruitsType type, FruitsController obj) // コンストラクタ　初期化できる
    {
        Type = type;
        FruitsObj = obj;

        switch (Type)
        {
            case FruitsType.None:
            Radius = 0.1f;
            break;
            default:
            Radius = 0.2f + (float)Type * 0.2f;
            break;
        }

        obj.transform.localScale = Vector3.one * Radius;
    }
}
