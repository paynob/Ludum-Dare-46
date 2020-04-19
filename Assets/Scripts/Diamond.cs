using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Diamond : MonoBehaviour
{
    public ParticleSystem particles;
    public SpriteRenderer spriteRenderer;

    private void Awake () {
        if (particles != null) {
            ParticleSystem.MainModule mainModule = particles.main;
            mainModule.startColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag("Player") )
        {
            Destroy( gameObject );
            FindObjectOfType<GameManager>().CollectDiamond();
        }
    }
}
