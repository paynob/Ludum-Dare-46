using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Transform crosshair = null;
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

    private void Awake()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();

        crosshairOffset = Vector2.Distance( transform.position, crosshair.position );
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePositionInRealWorld = cam.ScreenToWorldPoint( Input.mousePosition );

        Vector2 directionFromCharacterToMouse = (mousePositionInRealWorld - (Vector2)transform.position).normalized;

        crosshair.position = transform.position + (Vector3)directionFromCharacterToMouse;
        ///////////////////////////////////////
        
        if( Input.GetMouseButtonDown(0) )
        {
            Collider2D coll = Physics2D.OverlapPoint( crosshair.position, minableLayers );
            Debug.Log( "Clicked" );
            if ( coll != null )
            {
                Debug.Log( $"Hit {coll.name}" );
                Tile tile = coll.GetComponent<Tile>();
                if ( tile != null )
                {
                    Debug.Log( $"Tile {tile.name}" );
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

    private void LateUpdate()
    {
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
                if( body.velocity.y == 0 )
                    groundStatus = GroundStatus.Grounding;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(Input.GetAxis( "Horizontal" ) * movementSpeed, body.velocity.y);

        if( groundStatus == GroundStatus.StartingJump )
        {
            body.AddForce( new Vector2(Input.GetAxis("Horizontal"), jumpForce), ForceMode2D.Impulse );
            groundStatus = GroundStatus.Jumping;
        }
    }
}
