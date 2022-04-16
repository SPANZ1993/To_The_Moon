using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Spiral_Around : MonoBehaviour
{

    public static Spiral_Around instance;

    //public TextMeshProUGUI text;

    void Awake()
    {
        if (!instance){
            instance = this;
            //text = GameObject.Find("Angle Text").GetComponent<TextMeshProUGUI>();
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    
    IEnumerator _startSpiral(GameObject obj, GameObject target, float targetRadius, float angle, float angleSpeed=2f, float radialSpeed=0.5f, float radialAcceleration=1.1f, Action callBack=null){
       
        float radius = Vector2.Distance(obj.transform.position, target.transform.position);
        while(Vector2.Distance(obj.transform.position, target.transform.position) > targetRadius){
            angle += Time.deltaTime * angleSpeed;
            radius -= Time.deltaTime * radialSpeed * radius * radialAcceleration;
            radialAcceleration += Time.deltaTime * radialAcceleration;


            //instance.text.text = angle.ToString();


            float y = radius * Mathf.Cos(Mathf.Deg2Rad*angle);
            // float z = radius * Mathf.Sin(Mathf.Deg2Rad*angle);
            // float y = transform.position.y;
            float x = radius * Mathf.Sin(Mathf.Deg2Rad*angle);
            float z = obj.transform.position.z;

            obj.transform.position = target.transform.position + new Vector3(x, y, z);

            yield return new WaitForSeconds(0);
        }
        if (callBack != null){
            callBack();
        }
    }
 
    public static void startSpiralAround(GameObject obj, GameObject target, float targetRadius, float AngleSpeed=2f, float RadialSpeed=0.5f, float radialAcceleration=1.1f, Action callBack=null){

        

        if(Vector2.Distance(obj.transform.position, target.transform.position) > targetRadius){

            Vector2 toVector = target.transform.position - obj.transform.position;
            float Angle = Vector2.Angle(obj.transform.up, toVector); // Radial Angle About Target Object
            if(toVector.x > 0){
                Angle += 180f;
            }
            else {
                Angle = 180f - Angle;
            }



            Debug.Log("STARTING ANGLE: " + Angle);

            // float radius = Vector2.Distance(obj.transform.position, target.transform.position);
            // float angle = Angle;
            // float angleSpeed = AngleSpeed;
            // float radialSpeed = RadialSpeed;
            Debug.Log("OBJ: " + obj);
            Debug.Log("INSTANCE: " + instance);
            Debug.Log("TARGET: " + target);
            Debug.Log("TARGET RADIUS: " + targetRadius);
            Debug.Log("ANGLE: " + Angle);
            Debug.Log("ANGLE SPEED: " + AngleSpeed);
            Debug.Log("RADIAL SPEED: " + RadialSpeed);

            

            instance.StartCoroutine(instance._startSpiral(obj, target, targetRadius, Angle, AngleSpeed, RadialSpeed, radialAcceleration, callBack));
        }
        else{
            if (callBack != null){
                callBack();
            }
        }
    }


}
