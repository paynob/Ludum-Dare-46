using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : LivingBeing
{
    private const float MINE_ANIM_DURATION = 0.5f;

    [SerializeField]
    GameObject crosshair = null;
    [SerializeField]
    Transform hand = null;
    [SerializeField]
    LayerMask minableLayers;
    [SerializeField]
    int force = 10;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    AudioSource audioSource;

    Camera cam = null;
    Rigidbody2D body;

    float crosshairOffset;
    bool jumping;
    private bool mining = false;
    private float mineStartTime = 0.0f;

    private enum GroundStatus { Grounding, StartingJump, Jumping, Falling }
    private GroundStatus groundStatus;
    private Animator animator;
    public Transform modelGameObject;
    private Color unminableCrosshairColor = new Color(255, 0, 0);
    private Color minableCrosshairColor = new Color(0, 255, 0);

    private Transform crosshairTransform;
    private SpriteRenderer crosshairRenderer;

    private void Awake()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();

        if (crosshair != null) {
            crosshairTransform = crosshair.transform;
            crosshairRenderer = crosshair.GetComponent<SpriteRenderer>();
            crosshairOffset = Vector2.Distance( transform.position, crosshairTransform.position );
        }

        animator = GetComponent<Animator>();
        //modelGameObject = transform.Find("Model");

        
    }

    // Update is called once per frame
    void Update()
    {
        if( isDead )
            return;

        Vector2 mousePositionInRealWorld = cam.ScreenToWorldPoint( Input.mousePosition );

        Vector2 directionFromCharacterToMouse = (mousePositionInRealWorld - (Vector2)transform.position).normalized;

        crosshairTransform.position = transform.position + (Vector3)(directionFromCharacterToMouse * crosshairOffset);
        ///////////////////////////////////////
        Collider2D coll = Physics2D.OverlapPoint( crosshairTransform.position, minableLayers );
        if (crosshairRenderer != null) {
            crosshairRenderer.color = (coll != null) ? minableCrosshairColor : unminableCrosshairColor;
        }

        if( coll != null && Input.GetMouseButtonDown(0) )
        {
            Tile tile = coll.GetComponent<Tile>();

            if ( tile != null )
            {
                tile.Hit( force );
            }

            if  ( directionFromCharacterToMouse.x > 0.5f )
                modelGameObject.localScale = new Vector3( 1f, 1f, 1f );
            else if( directionFromCharacterToMouse.x < -0.5f )
                modelGameObject.localScale = new Vector3( -1f, 1f, 1f );
            // else by body.velocity.x instead of crosshair

            if (!mining) {
                audioSource.Play();
                mining = true;
                animator.SetBool("mining", true);
                mineStartTime = Time.fixedTime;
            }
        } else if (mining && (Time.fixedTime - mineStartTime > MINE_ANIM_DURATION)) {
            mining = false;
            animator.SetBool("mining", false);
        }

        ///////////////////////////////////////
        if (groundStatus == GroundStatus.Grounding && (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") == 1))
        {
            groundStatus = GroundStatus.StartingJump;
        }
    }

    private new void LateUpdate()
    {
        base.LateUpdate();

        if( isDead )
            return;

        switch( groundStatus )
        {
            case GroundStatus.Grounding:
                break;
            case GroundStatus.StartingJump:
                // Do nothing. Wait FixedUpdate to add the impulse
                break;
            case GroundStatus.Jumping:
                if( body.velocity.y <= 0 )
                    groundStatus = GroundStatus.Falling;
                break;
            case GroundStatus.Falling:
                if( body.velocity.y == 0 ) {
                    groundStatus = GroundStatus.Grounding;
                    animator.SetBool("jumping", false);
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if( isDead )
            return;

        body.velocity = new Vector2(Input.GetAxis( "Horizontal" ) * movementSpeed, body.velocity.y);
        animator.SetFloat("speedX", Mathf.Abs(body.velocity.x));

        if (modelGameObject != null && body.velocity.x != 0f) {
            modelGameObject.localScale = new Vector3((body.velocity.x > 0f) ? 1f : -1f, 1f, 1f);
        }


        if( groundStatus == GroundStatus.StartingJump )
        {
            body.AddForce( new Vector2(Input.GetAxis("Horizontal"), jumpForce), ForceMode2D.Impulse );
            groundStatus = GroundStatus.Jumping;
            animator.SetBool("jumping", true);
        }
    }

    public void Pick( Rigidbody2D pickable )
    {
        if( hand.childCount > 0 )
            hand.DetachChildren();
        pickable.transform.SetParent( hand );
        pickable.transform.localPosition = Vector2.zero;
        pickable.isKinematic = true;
    }

    public void Release( Rigidbody2D pickable)
    {
        pickable.transform.SetParent( null );
        pickable.isKinematic = false;
    }
}
