using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    Vector2Int tileMapDimension;

    [SerializeField]
    Tile breakableTilePrefab, unbreakableTilePrefab;

    [SerializeField]
    Gas gasPrefab;

    private float seed;
    private int smoothness;
    private float heightMultiplier;
    private int heightAddition;

    void Awake()
    {
        Populate();
    }

    void Populate()
    {
        Vector2Int end = new Vector2Int( tileMapDimension.x / 2, -tileMapDimension.y / 2 );

        for ( int i=-end.x; i<end.x; i++ )
        {
            int height = Mathf.RoundToInt( Mathf.PerlinNoise( seed, i / smoothness) * heightMultiplier) * heightAddition;

            //for ( int j = 0; j > )
        }
    }
}
