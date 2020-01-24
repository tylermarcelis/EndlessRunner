using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    protected PooledObject prefab;

    // Enum for determining what to do when an object is requested when none are available
    public enum PoolEmptyReaction
    {
        None,
        ExpandPool,
        TemporaryInstance,
    }

    [Tooltip("Defines the reaction to requesting an object when none are available:\n" +
        "None - Returns null\n" +
        "ExpandPool - Instantiates a new object into the object pool, permanently increasing its size\n" +
        "TemporaryInstance - Instantiates a new object that gets destroyed when returned to the pool")]
    [SerializeField]
    protected PoolEmptyReaction emptyReaction;

    List<PooledObject> availableObjects = new List<PooledObject>();

    public static ObjectPool CreatePool(PooledObject prefab, int preallocateAmount, PoolEmptyReaction emptyReaction = PoolEmptyReaction.None)
    {
        // Creates a new GameObject with an object pool component
        GameObject poolObject = new GameObject(prefab.name + " Pool");
        ObjectPool pool = poolObject.AddComponent<ObjectPool>();

        // Sets up variables and creates initial objects
        pool.prefab = prefab;
        pool.emptyReaction = emptyReaction;
        pool.PreallocateObjects(preallocateAmount);

        return pool;
    }

    protected void PreallocateObjects(int amount)
    {
        // If prefab is unset, exits early
        if (!prefab)
            return;

        // Instantiates and adds 'amount' copies of the prefab to the availableObjects list
        for (int i = 0; i < amount; i++)
        {
            CreateNewObject();
        }
    }

    public T RequestObject<T>() where T : PooledObject
    {
        // If there are objects available
        if (availableObjects.Count > 0)
        {
            // Enab
            PooledObject obj = availableObjects[availableObjects.Count - 1];
            obj.gameObject.SetActive(true);
            availableObjects.RemoveAt(availableObjects.Count - 1);
            return (T)obj;
        }

        // If no objects, decide what to do based on emptyReaction
        switch(emptyReaction)
        {
            case PoolEmptyReaction.ExpandPool:
                return (T)CreateNewObject(true);

            case PoolEmptyReaction.TemporaryInstance:
                PooledObject obj = Instantiate(prefab, transform);
                obj.destroyOnReturn = true;
                return (T)obj;

            default:
                return null;
        }
    }

    protected PooledObject CreateNewObject(bool enabled = false)
    {
        PooledObject po = Instantiate(prefab, transform);
        po.gameObject.SetActive(enabled);
        po.objectPool = this;
        availableObjects.Add(po);
        return po;
    }

    public void ReturnObject(PooledObject obj)
    {
        // if obj is null or does not belong to this pool, exits early
        if (!obj || obj.destroyOnReturn)
            return;

        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }
}
