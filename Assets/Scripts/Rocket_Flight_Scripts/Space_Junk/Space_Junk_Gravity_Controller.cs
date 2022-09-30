using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Gravity_Controller : MonoBehaviour
{

    [SerializeField]
    private bool disableCollisionOnCollision = true;
    [SerializeField]
    private float minAfterHitDestroyTime = 1.5f;
    [SerializeField]
    private float maxAfterHitDestroyTime = 3.0f;

    private Vector3 origScale;
    private float origGravityScale;

    Rigidbody2D rb;
    Collider2D collider;
    //private LeanTween leanTween;
    public bool collisionInitiated {get; private set;}
    //private bool collisionInitiated = false;
    private int xScaleId, yScaleId;

    private bool firstInstance = true;



    public delegate void Space_Junk_Collision_Occurred(GameObject obj);
    public static event Space_Junk_Collision_Occurred Space_Junk_Collision_Occurred_Info;


    private Space_Junk_Spawner spaceJunkSpawner;


    void OnEnable(){
        collisionInitiated = false;

        if (collider is null){
            if (GetComponent<CapsuleCollider2D>() != null){
                collider = GetComponent<CapsuleCollider2D>();
            }
            else if (GetComponent<BoxCollider2D>() != null){
                collider = GetComponent<BoxCollider2D>();
            }
            else if (GetComponent<CircleCollider2D>() != null){
                collider = GetComponent<CircleCollider2D>();
            }
            else if (GetComponent<PolygonCollider2D>() != null){
                collider = GetComponent<PolygonCollider2D>();
            }
        }

        if (rb is null){
            rb = GetComponent<Rigidbody2D>();
        }




        // collisionInitiated = false;
        // enableCollision();
        if (!firstInstance){
            transform.localScale = origScale;
            rb.gravityScale = origGravityScale;
        }
        
        enableCollision();




        Rocket_Game_Manager.PauseLaunchSceneInfo += onPause;
    }

    void OnDisable(){
        collisionInitiated = false;
    
        Rocket_Game_Manager.PauseLaunchSceneInfo -= onPause;
    }



    // Start is called before the first frame update
    void Start()
    {
        if (firstInstance){
            origScale = transform.localScale;
            origGravityScale = rb.gravityScale;
            firstInstance = false;
        }

        //rb = GetComponent<Rigidbody2D>();
        //collider = GetComponent<Collider2D>();
        //leanTween = GetComponent<LeanTween>();
        gameObject.layer = LayerMask.NameToLayer("Space Junk");
        //Debug.Log("WERE ON LAYER: " + LayerMask.LayerToName(gameObject.layer));

        try{
            spaceJunkSpawner = GameObject.Find("Junk_Spawner").GetComponent<Space_Junk_Spawner>();
        }
        catch{
            Debug.LogError("CAN'T FIND SPAWNER");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionInitiated){
            if (!LeanTween.isTweening(xScaleId) && !LeanTween.isTweening(yScaleId)){
                //Destroy(gameObject);\
                try{
                    spaceJunkSpawner.despawnPoolObj(gameObject);
                }
                catch{
                    Destroy(gameObject);
                }
            }
        }
        
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.gameObject.name == "Rocket"){
            //Debug.Log("HEY WE COLLIDED WITH THE ROCKET.. MY LAYER IS " + LayerMask.LayerToName(gameObject.layer) + " AND MY NAME IS " + gameObject.name);
            collisionInitiated = true;
            rb.gravityScale = 0.1f;
            if (disableCollisionOnCollision){
                disableCollision();
                if (Space_Junk_Collision_Occurred_Info != null){
                    Space_Junk_Collision_Occurred_Info(gameObject);
                }
                float timeToDestroy = Random.Range(minAfterHitDestroyTime, maxAfterHitDestroyTime);
                Vector3 origScale = gameObject.transform.localScale;
                LeanTweenType easeType = LeanTweenType.easeInOutSine;
                xScaleId = LeanTween.value(gameObject, origScale.x, 0.0f, timeToDestroy).setEase(easeType).setOnUpdate(
                    (value) =>
                    {
                        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
                    }
                ).id;
                yScaleId = LeanTween.value(gameObject, origScale.y, 0.0f, timeToDestroy).setEase(easeType).setOnUpdate(
                    (value) =>
                    {
                        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
                    }
                ).id;
            }
        }
        // else{
        //     Physics2D.IgnoreCollision(collision.otherCollider, collider);
        // }
    
    }

    void disableCollision(){
        collider.enabled = false;
    }

    void enableCollision(){
        collider.enabled = true;
    }

    void onPause(bool paused){
    // If we have collided, and are in the process of shrinking towards a despawn, continue doing that

        IEnumerator _onPause(){
            yield return new WaitForSeconds(0);
            LeanTween.resume(xScaleId);
            LeanTween.resume(yScaleId);
        }

        
        if (paused){
            StartCoroutine(_onPause());
        }
    }

}
