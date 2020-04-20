using System.Collections.Generic;
using UnityEngine;

public class Canary : LivingBeing
{
    [SerializeField]
    private AudioSource audioSource;

    Animator animator;
    Rigidbody2D body;

    Dictionary<Collider2D, float> enemiesDangerRatio = new Dictionary<Collider2D, float>();

    public Warning canaryWarning;

    private bool touchingPlayer;
    private PlayerController player;

    public GameObject pickMeUpSprite;

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

            if (canaryWarning != null) {
                canaryWarning.SetWarningLevel(totalDanger);
            }

            if (animator != null) {
                animator.Play(totalDanger > 0f ? "Base Layer.Canary" : "Base Layer.CanaryIdle");
            }

            if( Input.GetMouseButtonDown( 1 ) )
            {
                if( transform.parent == null && touchingPlayer )
                {
                    player.Pick( body );
                    pickMeUpSprite.SetActive(false);
                } else
                {
                    player.Release( body );
                    pickMeUpSprite.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);
                    pickMeUpSprite.SetActive(true);
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
