using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ), typeof( Animator ) )]
public class LivingBeing : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f,0.5f)]
    private float oxigenConsumptionPerSecond = 0.1f;

    [SerializeField]
    [Range( 0.5f, 1f )]
    private float oxigenRecoveryPerSecondOutsideMine = 1f;
    [SerializeField]
    [Range( 0.001f, 0.02f )]
    private float recoveryLostPerMeterDepth = 0.01f;

    public float oxigen { get; private set; } = 1f;
    public bool isDead { get; private set; } = false;

    public void TakeDamage( float amount )
    {
        oxigen = Mathf.Clamp01( oxigen - amount );

        if( oxigen == 0 )
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger( "Die" );
            // It's too late to do this in an organized way
            GameObject gameManagerGO = GameObject.FindWithTag("GameManager");
           
            if (gameManagerGO != null) {
                gameManagerGO.GetComponent<GameManager>().Die();
            }

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
        }
    }

    #region Private-Methods
    protected void LateUpdate()
    {
        if( !isDead && oxigen < 1f )
        {
            float currentOxigenRecoveryPerSecond = oxigenRecoveryPerSecondOutsideMine + recoveryLostPerMeterDepth * transform.position.y;

            TakeDamage( (oxigenConsumptionPerSecond - currentOxigenRecoveryPerSecond) * Time.deltaTime );
        }
    }
    #endregion Private-Methods
}

