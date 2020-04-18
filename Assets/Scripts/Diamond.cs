using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Diamond : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag("Player") )
        {
            Destroy( gameObject );
            FindObjectOfType<GameManager>().CollectDiamond();
        }
    }
}
