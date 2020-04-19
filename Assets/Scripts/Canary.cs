using System.Collections.Generic;
using UnityEngine;

public class Canary : LivingBeing
{
    [SerializeField]
    private AudioSource audioSource;

    Animator animator;
    Rigidbody2D body;

    Dictionary<Collider2D, float> enemiesDangerRatio = new Dictionary<Collider2D, float>();

    private bool touchingPlayer;
    private PlayerController player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if( !isDead && oxigen <= 1f )
        {
            float totalDanger = 0f;

            foreach( var c in enemiesDangerRatio )
            {
                totalDanger += c.Value;
            }

            audioSource.volume = Mathf.Pow( totalDanger, 2 ) * 2f;

            //TODO animator.SetFloat( "DangerRatio", totalDanger );

            if( Input.GetMouseButtonDown( 1 ) )
            {
                if( transform.parent == null && touchingPlayer )
                {
                    player.Pick( body );
                } else
                {
                    player.Release( body );
                }
            }
        } else if( isDead )
            audioSource.volume = 0;
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if( collision.CompareTag( "Player" ) )
        {
            touchingPlayer = true;
        }
    }
    private void OnTriggerExit2D( Collider2D collision )
    {
        if( collision.CompareTag( "Enemy" ) )
        {
            enemiesDangerRatio.Remove( collision );
        }
        else if ( collision.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }

    private void OnTriggerStay2D( Collider2D collision )
    {
        if ( collision.CompareTag("Enemy") )
        {
            float distanceRatio = Mathf.Clamp01 ( Vector2.Distance( transform.position, collision.transform.position ) / collision.bounds.extents.magnitude );

            enemiesDangerRatio[collision] = 1 - distanceRatio;
        }
    }
}
