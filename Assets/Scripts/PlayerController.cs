using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : LivingBeing
{
    [SerializeField]
    Transform crosshair = null, hand = null;
    [SerializeField]
    LayerMask minableLayers;
    [SerializeField]
    int force = 10;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float jumpForce;

    Camera cam = null;
    Rigidbody2D body;

    float crosshairOffset;
    bool jumping;

    private enum GroundStatus { Grounding, StartingJump, Jumping, Falling }
    private GroundStatus groundStatus;
    private Animator animator;
    public Transform modelGameObject;

    private void Awake()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();

        crosshairOffset = Vector2.Distance( transform.position, crosshair.position );
        animator = GetComponent<Animator>();
        modelGameObject = transform.Find("Model");
    }

    // Update is called once per frame
    void Update()
    {
        if( isDead )
            return;

        Vector2 mousePositionInRealWorld = cam.ScreenToWorldPoint( Input.mousePosition );

        Vector2 directionFromCharacterToMouse = (mousePositionInRealWorld - (Vector2)transform.position).normalized;

        crosshair.position = transform.position + (Vector3)directionFromCharacterToMouse;
        ///////////////////////////////////////
        
        if( Input.GetMouseButtonDown(0) )
        {
            Collider2D coll = Physics2D.OverlapPoint( crosshair.position, minableLayers );
            if ( coll != null )
            {
                Tile tile = coll.GetComponent<Tile>();
                if ( tile != null )
                {
                    tile.Hit( force );
                }

            }
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
