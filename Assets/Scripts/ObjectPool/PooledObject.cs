using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    /// <summary>
    /// オブジェクトをためるときに発火する処理
    /// </summary>
    protected abstract void OnStackAction();
    /// <summary>
    /// オブジェクトを取り出す時に発火する処理
    /// </summary>
    protected abstract void OnReleaseAction();


    /// <summary>
    /// オブジェクトをためるときに呼ぶ処理
    /// 非アクティブ化する
    /// </summary>
    public void OnStack()
    {
        OnStackAction();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// オブジェクトを取り出す時に呼ぶ処理
    /// Transformの各要素を初期化してアクティブ化
    /// </summary>
    /// <param name="position">生成座標</param>
    /// <param name="rotation">生成回転</param>
    /// <param name="parent">親オブジェクト</param>
    public void OnRelease(Vector3 position, Quaternion rotation, Transform parent)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.parent = parent;

        gameObject.SetActive(true);
        OnReleaseAction();
    }

    /// <summary>
    /// オブジェクトを取り出す時に呼ぶ処理
    /// Transformの各要素を初期化してアクティブ化
    /// </summary>
    /// <param name="position">生成座標</param>
    /// <param name="rotation">生成回転</param>
    /// <param name="scale">生成スケール</param>
    public void OnRelease(Vector3 position, Quaternion rotation)
    {
        OnRelease(position, rotation, transform.parent);

        gameObject.SetActive(true);
    }
}
