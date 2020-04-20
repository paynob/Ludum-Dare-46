using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Diamond : MonoBehaviour
{
    public ParticleSystem particles;
    public SpriteRenderer spriteRenderer;
    public int value = 1;

    private void Awake () {
        if (particles != null) {
            ParticleSystem.MainModule mainModule = particles.main;
            mainModule.startColor = spriteRenderer.color;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon( transform.position, "diamond.png", true , spriteRenderer.color);
    }
#endif
    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag("Player") )
        {
            Destroy( gameObject );
            FindObjectOfType<GameManager>().CollectDiamond(value);
        }
    }
}
