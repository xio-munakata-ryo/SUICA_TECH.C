using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitsType
{
    None = 0,
    Cherry,     // さくらんぼ
    Strawberry, // イチゴ
    Grape,      // ぶどう
    Dekopon,    // デコポン
    Persimmon,  // 柿
    Apple,      // リンゴ
    Pear,       // なし
    Peach,      // もも
    Pineapple,  // パイナップル
    Melon,      // メロン
    Watermelon, // スイカ
    Max,
}

[System.Serializable]
public class FruitsData
{
    // フルーツの種類
    public FruitsType Type;
    // フルーツの半径
    public float Radius;
    // フルーツのゲームオブジェクト
    public FruitsController Object;

    public FruitsData(FruitsType type, FruitsController obj)
    {
        Type = type;
        Object = obj;
        switch (type)
        {
            case FruitsType.None:
                Radius = 0.1f;
                break;
            default:
                Radius = 0.2f + (float)type * 0.2f;
                break;
        }
        Object.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
    }

    public static float GetRadiusAccordingType(FruitsType type)
    {
        float radius;
        switch (type)
        {
            case FruitsType.None:
                radius = 0.1f;
                break;
            default:
                radius = 0.2f + (float)type * 0.2f;
                break;
        }

        return radius;
    }
}
