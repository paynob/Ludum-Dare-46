using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = int.MaxValue; // Almost unbreakable

    [SerializeField]
    private int toughness = 0; // int.MaxValue -> totally unbreakable

    [SerializeField]
    private Sprite[] sprites = null;

    private SpriteRenderer spriteRenderer;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    private void UpdateSprite()
    {
        int spriteIndex = (int)((maxHealth-currentHealth) / (float)maxHealth * sprites.Length);
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="force"></param>
    public void Hit( int force )
    {
        if( toughness < force )
            currentHealth -= force - toughness;
        if( currentHealth > 0 )
            UpdateSprite();
        else
            Destroy( gameObject );
    }
}
