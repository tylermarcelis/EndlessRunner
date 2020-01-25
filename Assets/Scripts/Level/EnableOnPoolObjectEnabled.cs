using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnPoolObjectEnabled : MonoBehaviour, IPoolObjectEnabled
{
    public void PoolObjectEnabled()
    {
        gameObject.SetActive(true);
    }
}
