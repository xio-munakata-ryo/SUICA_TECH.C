using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    // プール
    private Queue<PooledObject> _pool;

    public ObjectPool()
    {
        _pool = new Queue<PooledObject>();
    }

    /// <summary>
    /// 引数で渡されたオブジェクトをプールに保存
    /// </summary>
    /// <param name="stackObj"></param>
    public void OnStack(PooledObject stackObj)
    {
        stackObj.OnStack();
        _pool.Enqueue(stackObj);
    }

    /// <summary>
    /// 1つオブジェクトを取り出し、プールから削除
    /// </summary>
    /// <returns></returns>
    /// <param name="position">生成座標</param>
    /// <param name="rotation">生成回転</param>
    /// <param name="scale">生成スケール</param>
    /// <param name="parent">親オブジェクト</param>
    public PooledObject  OnRelease(Vector3 position, Quaternion rotation, Transform parent)
    {
        if (_pool.Count > 0)
        {
            PooledObject  obj = _pool.Dequeue();
            obj.OnRelease(position, rotation, parent);
            return obj;
        }

        return null;
    }

    /// <summary>
    /// 1つオブジェクトを取り出し、プールから削除
    /// </summary>
    /// <returns></returns>
    /// <param name="position">生成座標</param>
    /// <param name="rotation">生成回転</param>
    /// <param name="scale">生成スケール</param>
    public PooledObject  OnRelease(Vector3 position, Quaternion rotation)
    {
        if (_pool.Count > 0)
        {
            PooledObject  obj = _pool.Dequeue();
            obj.OnRelease(position, rotation, obj.transform);
            return obj;
        }

        return null;
    }
}
