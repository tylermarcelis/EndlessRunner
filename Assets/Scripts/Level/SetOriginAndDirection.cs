using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOriginAndDirection : MonoBehaviour
{
    public Vector2Reference originPoint;
    public Vector2Reference travelDirection;

    public void SetValues()
    {
        originPoint.Value = transform.position;
        travelDirection.Value = transform.right;
    }
}
