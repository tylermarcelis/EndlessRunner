using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTile : PooledObject
{
    [SerializeField]
    public Transform entrancePoint;
    [SerializeField]
    protected Transform exitPoint;

    [SerializeField]
    protected Vector2 exitHeightOffset;

    public float Height
    {
        get;
        protected set;
    }

    public Vector2 TravelDirection {
        get
        {
            return exitPoint.right;
        }
    }

    public Vector2 UpDirection
    {
        get
        {
            return exitPoint.up;
        }
    }

    protected void SetRotation(Transform prevExit)
    {
        // Rotate segment so that entrancePoint rotation matches prevExit
        transform.rotation = transform.rotation * (Quaternion.Inverse(entrancePoint.transform.rotation) * prevExit.rotation);
    }

    public void AttachAt(EnvironmentTile prevTile, LevelGenerator level)
    {
        SetRotation(prevTile.exitPoint);
        SetPosition(prevTile, level);
    }

    protected void SetPosition(EnvironmentTile prevTile, LevelGenerator level)
    {
        Vector3 upDir = prevTile.exitPoint.up;

        // Finds the minimum and maximum offset values, from the previous tiles exit height offset
        float min = Mathf.Min(prevTile.exitHeightOffset.x, prevTile.exitHeightOffset.y);
        float max = Mathf.Max(prevTile.exitHeightOffset.x, prevTile.exitHeightOffset.y);

        float minHeight = Mathf.Max(-level.maxHeightOffset, prevTile.Height + min);
        float maxHeight = Mathf.Min(level.maxHeightOffset, prevTile.Height + max);

        // Randomly decide upon a height
        float offsetAmount = Random.Range(minHeight, maxHeight);
        Height = offsetAmount;
        // Sets position
        transform.position = prevTile.exitPoint.transform.position - (entrancePoint.transform.position - transform.position) // Calculates position to match entrance and previous exit
            + upDir * (offsetAmount - prevTile.Height); // Add offset
    }


#if UNITY_EDITOR
    // Drawing gizmos to display possible heights for connecting tiles
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        DrawHeightOffset(exitPoint, exitHeightOffset);
    }

    private void DrawHeightOffset(Transform trans, Vector2 offset)
    {
        Vector3 direction = trans.up;
        Gizmos.DrawLine(trans.position + direction * offset.x, trans.position + direction * offset.y);
    }
#endif
}
