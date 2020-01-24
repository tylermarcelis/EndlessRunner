using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [Header("Pool Settings")]
    public int preallocatedAmount = 5;
    public ObjectPool.PoolEmptyReaction emptyReaction;

    [System.NonSerialized]
    public ObjectPool objectPool;

    [System.NonSerialized]
    public bool destroyOnReturn = false;

    // Gets an instance of the prefab from the objectpool
    public T GetInstance<T>() where T : PooledObject
    {
        if (!objectPool)
            CreatePool();
        return objectPool.RequestObject<T>();
    }

    // Returns the object to the pool, or destroys if there is no object pool
    public void ReturnObjectToPool()
    {
        if (destroyOnReturn || !objectPool)
            Destroy(gameObject);
        else
            objectPool.ReturnObject(this);
    }

    public void CreatePool()
    {
        objectPool = ObjectPool.CreatePool(this, preallocatedAmount, emptyReaction);
    }
}
