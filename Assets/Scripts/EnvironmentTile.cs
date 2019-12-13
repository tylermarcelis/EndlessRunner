using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTile : MonoBehaviour
{
    [SerializeField]
    public Transform entrancePoint;
    [SerializeField]
    protected Transform exitPoint;

    [SerializeField]
    protected Vector2 entranceHeightOffset;

    [SerializeField]
    protected Vector2 exitHeightOffset;

    public void AttachAt(Transform prevExit)
    {
        // Rotate segment so that entrancePoint rotation matches prevExit
        transform.rotation = transform.rotation * (Quaternion.Inverse(entrancePoint.transform.rotation) * prevExit.rotation);

        // Move segment so that entrancePoint position matches prevExit
        transform.position = prevExit.transform.position - (entrancePoint.transform.position - transform.position);
    }

    public void AttachAt(EnvironmentTile prevTile, LevelGenerator level)
    {
        AttachAt(prevTile.exitPoint);
        ApplyOffset(prevTile, level);
    }

    protected void ApplyOffset(EnvironmentTile prevTile, LevelGenerator level)
    {
        Vector3 upDir = prevTile.exitPoint.up;
        Vector2 exitPos = prevTile.exitPoint.position;
        float prevHeight = Vector2.Dot(upDir, exitPos - level.CenterPoint);
        Debug.Log(prevHeight);
        float min = Mathf.Min(prevTile.exitHeightOffset.x, prevTile.exitHeightOffset.y);
        float max = Mathf.Max(prevTile.exitHeightOffset.x, prevTile.exitHeightOffset.y);
        float minHeight = Mathf.Max(-level.maxHeightOffset, prevHeight + min);
        float maxHeight = Mathf.Min(level.maxHeightOffset, prevHeight + max);
        Debug.Log(minHeight + " / " + maxHeight);
        float offsetAmount = Random.Range(minHeight, maxHeight);
        transform.position = prevTile.exitPoint.transform.position - (entrancePoint.transform.position - transform.position) + upDir * (offsetAmount - prevHeight);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        DrawHeightOffset(entrancePoint, entranceHeightOffset);
        DrawHeightOffset(exitPoint, exitHeightOffset);
    }

    private void DrawHeightOffset(Transform trans, Vector2 offset)
    {
        Vector3 direction = trans.up;
        Gizmos.DrawLine(trans.position + direction * offset.x, trans.position + direction * offset.y);
    }
}
