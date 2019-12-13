using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected EnvironmentTile startTile;
    public List<EnvironmentTile> tiles;
    public int count = 10;

    public float maxHeightOffset = 3;

    public Vector2 CenterPoint
    {
        get;
        protected set;
    } = Vector2.zero;

    public Vector2 TravelDirection
    {
        get;
        protected set;
    } = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        EnvironmentTile previousTile = startTile;

        EnvironmentTile newTile;
        for (int i = 0; i < count; i++)
        {
            newTile = Instantiate(tiles[0], transform);
            newTile.AttachAt(previousTile, this);
            previousTile = newTile;
        }
    }

}
