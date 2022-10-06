using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Golf : Space_Junk_Flyer
{
    [SerializeField]
    private GameObject Golf_Ball_Prefab;
    private GameObject Golf_Ball;
    private GameObject Golf_Ball_Spawner;

    private Space_Junk_Gravity_Controller gravControl;



    void OnEnable(){
        base.OnEnable();
        Golf_Ball_Spawner = Object_Finder.findChildObjectByName(gameObject, "Golf_Ball_Spawner");
        SpawnGolfBall();
        
        gravControl = GetComponent<Space_Junk_Gravity_Controller>();
    }

    // void OnDisable(){
    //     base.OnDisable();
    // }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Golf_Ball != null){
            Golf_Ball.transform.position = Golf_Ball_Spawner.transform.position;
        }
    }

    public void SpawnGolfBall(){
        if(Golf_Ball == null){
            Golf_Ball = (GameObject)Instantiate(Golf_Ball_Prefab, Golf_Ball_Spawner.transform.position, gameObject.transform.rotation);
            Golf_Ball.transform.SetParent(transform);
            Golf_Ball.transform.localScale = new Vector3();
            float ballSize = (transform.localScale.y / 3.5f) * .05f;
            LeanTween.scale(Golf_Ball, new Vector3(ballSize, ballSize, ballSize), .4f).setEase(LeanTweenType.easeOutSine);
        }
    }

    public void LaunchGolfBall(){
        if(gravControl == null || !gravControl.collisionInitiated){
            try{
                
                if (base.rb is null){
                    base.rb = GetComponent<Rigidbody2D>();
                }


                float thrust = base.thrust * 100f;
                //float topSpeed = base.topSpeed * Random.Range(1.5f, 2f);
                //float topSpeed = base.rb.velocity.magnitude + (base.rb.velocity.magnitude * Random.Range(0.05f, 0.1f));
                //topSpeed = base.rb.velocity.magnitude * Random.Range(4f, 6f);
                topSpeed = base.maxPossibleTopSpeed * Random.Range(12f, 17f);
                bool goingRight = base.goingRight;
                float offsetAngle;
                float offsetAngleDelta = Random.Range(10f, 40f);
                if(goingRight){
                    offsetAngle = base.offsetAngle + offsetAngleDelta;
                }
                else{
                    offsetAngle = base.offsetAngle - offsetAngleDelta;
                }
                Golf_Ball.GetComponent<Space_Junk_Golf_Ball>().Launch(offsetAngle, thrust, topSpeed, goingRight);
                //Destroy(Golf_Ball);
                Golf_Ball = null;
            }
            catch(System.Exception e){
                Debug.Log("Couldn't Launch");
            }
        }
    }
}
