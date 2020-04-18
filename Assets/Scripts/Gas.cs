#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Gas : MonoBehaviour
{
    [SerializeField]
    private float minDamage;

    [SerializeField]
    private float maxDamage;

    [SerializeField]
    private float canaryDetectionRadius, playerDetectionRadius;

    private CircleCollider2D circle;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if( circle == null )
            circle = GetComponent<CircleCollider2D>();
        circle.radius = canaryDetectionRadius;

        if( playerDetectionRadius >= canaryDetectionRadius )
            playerDetectionRadius = canaryDetectionRadius;
    }

    private void OnDrawGizmos()
    {
        if( circle == null )
            circle = GetComponent<CircleCollider2D>();

        Color c = Handles.color;
        Handles.color = Color.red;
        Handles.DrawWireDisc( transform.position // position
                                      , transform.forward                       // normal
                                      , circle.radius );
        Handles.color = Color.yellow;
        Handles.DrawWireDisc( transform.position // position
                                      , transform.forward                       // normal
                                      , playerDetectionRadius );

        Handles.color = c;
    }
#endif

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        circle.isTrigger = true;
    }

    private void OnTriggerStay2D( Collider2D collision )
    {
        if ( collision.CompareTag("Canary"))
        {
            float distance = Vector2.Distance( collision.transform.position, transform.position ) - collision.bounds.extents.magnitude;

            float damage = DamageAtDistance( distance );

            collision.GetComponent<Canary>().TakeDamage( damage  );
        }
        else if (collision.CompareTag("Player") )
        {
            float distance = Vector2.Distance( collision.transform.position, transform.position ) - collision.bounds.extents.magnitude;

            if ( distance <= playerDetectionRadius )
            {
                float damage = DamageAtDistance( distance );
                collision.GetComponent<PlayerController>().TakeDamage( damage );
            }
        }
    }

    private float DamageAtDistance( float distance )
    {
        float ratio = 1 - Mathf.Clamp01( distance / canaryDetectionRadius );

        return ratio * (maxDamage - minDamage) + minDamage;
    }
}
