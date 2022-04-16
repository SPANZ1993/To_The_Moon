using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Control : MonoBehaviour
{

    public float speed = 10.0f;
    private Vector3 prevLoc = new Vector3();
    public Vector3 dir = new Vector3();
    [SerializeField]
    private float cur_speed;
    private Rigidbody2D shipRb;
    private CapsuleCollider2D rocketCollider;
    private ParticleSystem rocketParticleSystem;

    private Touch_Detection touchDetection;

    public double thrust;
    [SerializeField]
    private float maxInstThrust = 300.0f;
    [SerializeField]
    private float maxVertSpeed = 5.0f;
    [SerializeField]
    private float maxHorSpeed = 5.0f;
    [SerializeField]
    private float minIntAngle = 45.0f;
    [SerializeField]
    private Vector2 forceComponents;
    [SerializeField]
    private Vector2 forceComponentsFinal;
    [SerializeField]
    private float int_angle = 90.0f;  
    [SerializeField]
    private float thrustDecrementRate = 1.0f; // How much thrust to decrement per second (At max thrust being applied)

    private bool isBouncing = false;
    public bool reachedTargetAltitude = false;
    public bool startedSpiral = false;

    private bool userHasControl = true; // Is the user controlling the rocket?

    public bool thrustInitialized;

    private Game_Scaler gameScaler;


    public bool alertedFuelEmpty = false;
    public delegate void AlertFuelEmpty();
    public static event AlertFuelEmpty AlertFuelEmptyInfo;


    GameObject cam;




    // Start is called before the first frame update
    void Start()
    {   

        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();


        maxInstThrust = gameScaler.Scale_Value_To_Screen_Width(maxInstThrust); // SCALE TO HEIGHT???
        maxVertSpeed = gameScaler.Scale_Value_To_Screen_Height(maxVertSpeed);
        maxHorSpeed = gameScaler.Scale_Value_To_Screen_Width(maxHorSpeed);

        rocketParticleSystem = GameObject.Find("Rocket_Particles").GetComponent<ParticleSystem>();
        
        rocketParticleSystem.Play(true);
        
        
        touchDetection = GameObject.Find("Input_Detector").GetComponent<Touch_Detection>();
        shipRb = GetComponent<Rigidbody2D>();

        rocketCollider = GetComponent<CapsuleCollider2D>();
        forceComponents = new Vector2(0.0f, maxInstThrust);

        cam = GameObject.Find("Main Camera");
    }



    void OnEnable(){
        Rocket_Game_Manager.PauseLaunchSceneInfo += onGamePause;
    }

    
    void OnDisable(){
        Rocket_Game_Manager.PauseLaunchSceneInfo -= onGamePause;
    }



    // Update is called once per frame
    void Update()
    {
        if (thrustInitialized){ // Game_Manager_Will_Set_This
            if (thrust > 0.0 && userHasControl){
                if(reachedTargetAltitude && !startedSpiral){
                    Debug.Log("Reached Target Altitude.. We're Chilling");
                    RotateRocketInMovementDirection();
                    startedSpiral = true;
                    // if (!startedSpiral){
                    //     Debug.Log("STARTING SPIRAL");
                    //     rocketCollider.enabled = false;
                    //     Spiral_Around.startSpiralAround(gameObject, GameObject.Find("Arriving_Planet"), .001f);
                    //     startedSpiral = true;
                    // }
                }
                else{
                    Debug.Log("HELLO MOVING ROCKET");
                    MoveRocket();
                    MaintainMinRotation();
                }
            }
            else if (!alertedFuelEmpty){
                if (AlertFuelEmptyInfo != null){
                    AlertFuelEmptyInfo();
                }
                alertedFuelEmpty = true;
            }
        }
    }

    // void MoveUp(){

    //     dir = shipRb.velocity;
    //     //transform.rotation = Quaternion.Euler(0, 0, -22.5f);
    //     //prevLoc = transform.position;
    //     if (dir.y < maxSpeed){
    //         shipRb.AddForce(transform.up * maxInstThrust * Time.deltaTime);
    //     }
    //     //transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);
        
    // }

    // void MoveSideways(){
    //     if (touchDetection.dragLocs.Count != 0)
    //         Debug.Log(touchDetection.TouchPos2ScreenPos(touchDetection.dragLocs[touchDetection.dragLocs.Count-1]));
    // }




    float ScaleTouchX2minIntAngle(float touch_loc){
        // https://stats.stackexchange.com/questions/178626/how-to-normalize-data-between-1-and-1

        float angle = ((minIntAngle-(-minIntAngle))*((touch_loc-(-1.0f))/(1.0f - (-1.0f))))  + (-minIntAngle);

        if (angle == 0.0f){
            angle = 90.0f;
        }
        else if (angle > 0.0f){
            angle = 90.0f - angle;
        }
        else if (angle < 0.0f){
            angle = -(90.0f + angle);
        }
        return angle;
    }


    void CalculateForceComponents(float int_angle){


        float _SolveForYComponent(float int_angle){
            // SOHCAHTOA, My Dawg
            float YComp = maxInstThrust;
            if (int_angle == 90.0f){
                YComp = maxInstThrust;
            }
            else if (int_angle < 0.0f){
                YComp = Mathf.Sin((-int_angle) * Mathf.Deg2Rad) * maxInstThrust;
            }
            else if (int_angle > 0.0f){
                YComp = Mathf.Sin(int_angle * Mathf.Deg2Rad) * maxInstThrust;
            }
            return YComp;
        }

        float _SolveForXComponent(float int_angle, float YComp){
            // Big Pythagoras Up In This Piece
            float XComp = Mathf.Sqrt((maxInstThrust*maxInstThrust) - (YComp * YComp));
            if (int_angle < 0.0f){
                XComp = XComp * -1.0f;
            }
            return XComp;
        }
    
        Vector2 IntAngle2ForceVector(float int_angle){
            float YComp = _SolveForYComponent(int_angle);
            float XComp = _SolveForXComponent(int_angle, YComp);
            return new Vector2(XComp, YComp);
        }


        forceComponents = IntAngle2ForceVector(int_angle);
        //Debug.Log("INT ANGLE: " + int_angle + " --- FORCE VECTOR: " + forceComponents);
        //float YComp = Mathf.sin()
    }



    void MoveRocket(){

        dir = shipRb.velocity;
        cur_speed = dir.magnitude;


        if (touchDetection.dragLocs.Count != 0){
            Vector2 touch_loc = touchDetection.TouchPos2ScreenPos(touchDetection.dragLocs[touchDetection.dragLocs.Count-1]);
            int_angle = ScaleTouchX2minIntAngle(touch_loc.x);

            CalculateForceComponents(int_angle);
            RotateRocket(int_angle);
        }
        else{
            //Debug.Log("ROTATING FOR INT ANGLE OF : " + int_angle);
            RotateRocket(int_angle);
        }
        //Vector2 forceComponentsFinal = new Vector2(forceComponents.x, forceComponents.y);
        forceComponentsFinal = new Vector2(forceComponents.x, forceComponents.y); // REMOVE!!!
        if (dir.y >= maxVertSpeed){
            forceComponentsFinal.y = 0.0f;
        }
        if (Mathf.Abs(dir.x) >= maxHorSpeed){
            if (dir.x >= 0.0f){
                if (forceComponents.x >= 0.0f){
                    forceComponentsFinal.x = 0.0f;
                }
            }
            else if (dir.x <= 0.0f){
                if (forceComponents.x <= 0.0f){
                    forceComponentsFinal.x = 0.0f;
                }
            }
        }
       
        //Debug.Log("FORCECOMPONENTS: " + forceComponentsFinal * Time.deltaTime + " SPEED: " + dir.y);

        shipRb.AddForce(forceComponentsFinal * Time.deltaTime);
        DecrementThrust(forceComponentsFinal);
    }


    void DecrementThrust(Vector2 forceComponents){
        thrust -= (forceComponents.magnitude/maxInstThrust) * (Time.deltaTime/(1.0f/thrustDecrementRate));
        if (thrust <= 0){
            thrust = 0;
        }
    }


    void RotateRocket(float int_angle){
        float rot_angle = 0.0f;
        if (int_angle <= 0.0f){
            rot_angle = 90.0f + int_angle;
        }
        else if(int_angle >= 0.0f){
            rot_angle = -90.0f + int_angle;
        }
        //Debug.Log("IA: " + int_angle + " ROT ANG: " + rot_angle);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rot_angle);
    }

    void MaintainMinRotation(){
        Vector2 obj_up = (Vector2) transform.up;
        float signed_angle = Vector2.SignedAngle(obj_up, Vector2.up);
        float angle = signed_angle;

        if (angle > minIntAngle){
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 360.0f-minIntAngle);
        }
        else if (angle < -minIntAngle){
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, minIntAngle);
        }
    }


    void RotateRocketInMovementDirection(){

        Vector3 newPos = transform.position;
        Vector3 oldPos = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        Vector3 direction = Vector3.zero;

        IEnumerator _RotateRocketInMovementDirection(){
            yield return new WaitForSeconds(0);
                oldPos = newPos;
                newPos = transform.position;

                velocity = (newPos - oldPos);
                direction = velocity.normalized;

                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 10000f);
            StartCoroutine(_RotateRocketInMovementDirection());
        }

        StartCoroutine(_RotateRocketInMovementDirection());
    }






    void OnCollisionEnter2D(Collision2D collision)
    {

        if (!isBouncing){
            // Debug.Log("BOUNCE: " + (collision.contacts[0].normal * cur_speed));
            Vector2 bounceDir = collision.contacts[0].normal * maxInstThrust;
            if (bounceDir.y < 0.0f){
                shipRb.AddForce(new Vector2(0.0f, bounceDir.y));
                isBouncing = true;
                Invoke("StopBounce", 0.3f);
            }
        }
    }

    void StopBounce()
    {
        isBouncing = false;
    }


    public void dropRocket(){

        IEnumerator NudgeDownWard(){
            yield return new WaitForSeconds(0);
            shipRb.AddForce(new Vector2(0, -maxInstThrust) * Time.deltaTime);
        }
        // Stop the camera from following the rocket, turn off thrust, and let rocket fall from screen
        // As if rocket ran out of fuel
        Debug.Log("DROPPING ROCKET");
        userHasControl = false;
        rocketParticleSystem.Stop(true);

        StartCoroutine(NudgeDownWard()); // Not sure why but rocket sometimes get stuck in place after reenabling, just give it a gentle nudge

        cam.GetComponent<Follow>().followY = false;
    }



    void onGamePause(bool pause){
        
        if (pause){
            Debug.Log("PAUSING SHIP");
            shipRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else{
            Debug.Log("UNPAUSING SHIP " + shipRb);
            shipRb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        }
    }


}