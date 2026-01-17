using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolObjectInfo
{
    [SerializeField, Header("プールしたいオブジェクト")]
    private PooledObject _poolObj = null;

    [SerializeField, Header("初期生成数")]
    private uint _firstGenerate = 0;

    /// <summary>
    /// プールしたいオブジェクト
    /// </summary>
    public PooledObject PoolObj => _poolObj;

    /// <summary>
    /// 初期生成数
    /// </summary>
    public uint FirstGenerate => _firstGenerate;
}

[DefaultExecutionOrder(-10)]
public class PoolManager : MonoBehaviour
{
    [SerializeField, Header("プールしたいオブジェクトたち")]
    private List<PoolObjectInfo> _poolObjs = new List<PoolObjectInfo>();

    private Dictionary<string, ObjectPool> _pools = new Dictionary<string, ObjectPool>();

    private const string CloneName = "(Clone)";

    private bool SetUpPool(PoolObjectInfo objectInfo)
    {
        if (objectInfo.PoolObj == null)
        {
            Debug.LogWarning("PoolManager：“PoolObj” has not been assigned.");
            return false;
        }
        string key = objectInfo.PoolObj.gameObject.name;
        if (!_pools.TryAdd(key, new ObjectPool()))
        {
            Debug.LogWarning($"そのキーは既に存在します：{key}");
            return false;
        }
        for (int i = 0; i < objectInfo.FirstGenerate; i++)
        {
            PooledObject instance = Instantiate(objectInfo.PoolObj);
            ObjectPool pool = null;
            if (!_pools.TryGetValue(key, out pool))
            {
                Debug.LogWarning($"そのキーは存在しません：{key}");
                return false;
            }
            // Debug.Log($"スタックしたキー：{key}");
            pool?.OnStack(instance);
        }

        return true;
    }

    private void SetUpAllPools()
    {
        foreach (PoolObjectInfo obj in _poolObjs)
        {
            if (!SetUpPool(obj)) continue;
        }
    }

    public void StackObject(PooledObject obj)
    {
        string key = obj.gameObject.name;
        if (key.Contains(CloneName))
            key = key.Replace(CloneName, "");
        ObjectPool pool = null;
        if (!_pools.TryGetValue(key, out pool))
        {
            Debug.LogWarning($"そのキーは存在しません：{key}");
            return;
        }
        // Debug.Log($"スタックしたキー：{key}");
        pool?.OnStack(obj);
    }

    public PooledObject ReleaseObject(PooledObject obj, Vector3 position, Quaternion rotation, Transform parent)
    {
        string key = obj.gameObject.name;
        ObjectPool pool = null;
        if (!_pools.TryGetValue(key, out pool))
        {
            Debug.LogWarning($"そのキーは存在しません：{key}");
            return null;
        }
        PooledObject instance = pool?.OnRelease(position, rotation, parent);
        if (instance != null)
        {
            return instance;
        }
        else
        {
            return Instantiate(obj, position, rotation, parent);
        }
    }

    public PooledObject ReleaseObject(PooledObject obj, Vector3 position, Quaternion rotation)
    {
        return ReleaseObject(obj, position, rotation, obj.transform.parent);
    }

    void Start()
    {
        SetUpAllPools();
    }


    #region シングルトンパターン
    private static PoolManager _instance = null;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogWarning("PoolManager is nothing");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }
    #endregion
}
