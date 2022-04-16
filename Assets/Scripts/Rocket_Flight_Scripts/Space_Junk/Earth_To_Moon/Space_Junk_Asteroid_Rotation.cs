using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Junk_Asteroid_Rotation : MonoBehaviour
{
    [SerializeField]
    private float revolutionTimeMin, revolutionTimeMax;
    private float revolutionTime;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        revolutionTime = Random.Range(revolutionTimeMin, revolutionTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, (Time.deltaTime/revolutionTime)*360.0f);
        updateAnim();
    }

    private float curRot;
    private void updateAnim(){
        curRot = transform.eulerAngles.z;
        if (0 < curRot && curRot <= 90){
            anim.SetInteger("Rotation_Anim_Num", 0);
        }
        else if (90 < curRot && curRot <= 180){
            anim.SetInteger("Rotation_Anim_Num", 1);
        }
        else if (180 < curRot && curRot <= 270){
            anim.SetInteger("Rotation_Anim_Num", 2);
        }
        else if (270 < curRot && curRot <= 360){
            anim.SetInteger("Rotation_Anim_Num", 3);
        }
    }
}
