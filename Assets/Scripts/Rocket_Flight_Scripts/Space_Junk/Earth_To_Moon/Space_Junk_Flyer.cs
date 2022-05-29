using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Flyer : Space_Junk_Base
{
    [SerializeField]
    bool DebugMode = false;

    [SerializeField]
    private float maxRotation = 15.0f; // How severe an angle of flight can the object take
    [SerializeField]
    private float maxSpriteRotation; // Visually, how much can we rotate the sprite (Must be less than or equal to max rotation)
    //protected Rigidbody2D rb;
    protected SpriteRenderer rend;
    protected bool goingRight;
    protected float offsetAngle;
    [SerializeField]
    private float minPossibleThrust, maxPossibleThrust;
    private float thrust; // = 0.5f;
    [SerializeField]
    private float minPossibleTopSpeed, maxPossibleTopSpeed;
    protected float topSpeed; // = 2.0f; 


    [SerializeField]
    protected bool shouldFlipSprite;
    bool spriteFlipped = false;

    protected float speed = 0.0f;

    protected bool hitHappened = false;
    protected Vector2 ForceComponents;
    protected Vector2 thrustVec;
    protected float accelerationMult;

    protected Game_Scaler gameScaler;

    void Awake(){

    }


    protected virtual void OnEnable()
    {
        base.OnEnable();
        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info += onSpaceJunkCollision;

        //spriteFlipped = false;


        if (!DebugMode && gameScaler is null){
            gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        }


        if (rend is null){
            rend = GetComponent<SpriteRenderer>();
        }

        offsetAngle = Random.Range(-maxRotation, maxRotation);
        if (Random.value < 0.5){
            goingRight = false;
        }
        else{
            goingRight = true;
        }

        thrust = Random.Range(minPossibleThrust, maxPossibleThrust);
        topSpeed = Random.Range(minPossibleTopSpeed, maxPossibleTopSpeed);


        if (rb is null){
            rb = GetComponent<Rigidbody2D>();
        }

        //Debug.Log("VECTOR3 ZERO: " + Vector3.zero + " ... " + rb.velocity + " ... " + rb.angularVelocity);

        if(offsetAngle >= 0){
            Debug.Log("OA: " + offsetAngle + " ---> " + Mathf.Min(offsetAngle, maxSpriteRotation));
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Min(offsetAngle, maxSpriteRotation));
        }
        else{
            Debug.Log("OA: " + offsetAngle + " ---> " + -Mathf.Min(Mathf.Abs(offsetAngle), maxSpriteRotation));
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -Mathf.Min(Mathf.Abs(offsetAngle), maxSpriteRotation));
        }
        CalculateXYMovementComponents(offsetAngle, goingRight);
        thrustVec = new Vector2(thrust, thrust);
        thrustVec = thrustVec * ForceComponents;

        // if (!goingRight && shouldFlipSprite && !spriteFlipped){
        //     Debug.Log("FLIPPED SPRITE FROM ENABLE");
        //     flipSprite();
        // }

    }

    void OnDisable(){

        base.OnDisable();

        Space_Junk_Gravity_Controller.Space_Junk_Collision_Occurred_Info -= onSpaceJunkCollision;

        // if (!goingRight){
        //     Vector3 curScale = transform.localScale;
        //     transform.localScale = new Vector3(curScale.x * -1f, curScale.y, curScale.z);
        // }

        

        if (spriteFlipped){
            flipSprite();
            //Debug.Log("FLIPPED SPRITE FROM DISABLE");
            //spriteFlipped = false;
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        
        hitHappened = false;
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {

    }



    // Update is called once per frame
    protected virtual void Update()
    {
        if (!hitHappened){
            if (gameScaler is null){
                accelerationMult = 1 - (rb.velocity.magnitude / gameScaler.Scale_Value_To_Screen_Width(topSpeed));
            }
            else{
                accelerationMult = 1 - (rb.velocity.magnitude / topSpeed);
            }
            speed = rb.velocity.magnitude;
            if (Mathf.Abs(speed) < topSpeed){
                rb.AddForce(thrustVec * accelerationMult);
            }
            if (shouldFlipSprite){
                if (ForceComponents.x < 0 && !spriteFlipped){//!rend.flipX){
                    flipSprite();
                    //rend.flipX = true;
                }
                else if (ForceComponents.x >= 0 && spriteFlipped){// rend.flipX){
                    flipSprite();
                    //rend.flipX = false;
                }
            }
        }
    }


    private void CalculateXYMovementComponents(float angle, bool goingRight){
        float xComp = Mathf.Cos(Mathf.Abs(angle  * Mathf.Deg2Rad));
        float yComp = Mathf.Sin(Mathf.Abs(angle  * Mathf.Deg2Rad));

        if (angle >= 0.0f && goingRight){
            ForceComponents = new Vector2(xComp, yComp).normalized;
        }
        else if(angle < 0.0f && goingRight){
            ForceComponents = new Vector2(xComp, -yComp).normalized;
        }
        else if(angle >= 0.0f && !goingRight){
            ForceComponents = new Vector2(-xComp, -yComp).normalized;
        }
        else{
            ForceComponents = new Vector2(-xComp, yComp).normalized;
        }
        if (!DebugMode){
            ForceComponents.x = ForceComponents.x * gameScaler.ScaleX;
            ForceComponents.y = ForceComponents.y * gameScaler.ScaleY;
        }
    }


    private void onSpaceJunkCollision(GameObject obj){
        if (GameObject.ReferenceEquals(gameObject, obj)){
            hitHappened = true;
        }
    }

    private void flipSprite(){
        Vector3 curScale = transform.localScale;
        transform.localScale = new Vector3(curScale.x * -1f, curScale.y, curScale.z);
        spriteFlipped = !spriteFlipped;
    }

}