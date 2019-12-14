using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOriginAndDirection : MonoBehaviour
{
    public Vector2Reference originPoint;
    public Vector2Reference travelDirection;

    public void SetValues()
    {
        travelDirection.Value = transform.right;
        originPoint.Value = transform.position;
    }
}
