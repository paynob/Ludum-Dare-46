using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSpriteRotation : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sprites.Length; i++) {
            sprites[i].transform.rotation = Quaternion.identity;
        }
    }
}
