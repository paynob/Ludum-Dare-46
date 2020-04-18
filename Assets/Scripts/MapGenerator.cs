using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    Vector2Int tileMapDimension;


    // Start is called before the first frame update
    void Start()
    {

        Populate();
    }

    void Populate()
    {
    }
}
