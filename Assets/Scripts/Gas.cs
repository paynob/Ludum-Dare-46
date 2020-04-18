using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField]
    private float minDamage, maxDamage;

    private void OnTriggerStay2D( Collider2D collision )
    {
        if ( collision.CompareTag("Canary"))
        {
             // TODO
        }
        else if (collision.CompareTag("Player") )
        {
            // TODO
        }
    }
}
