using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drifter : MonoBehaviour
{

    [SerializeField]
    private float minSpeedX, maxSpeedX, minSpeedY, maxSpeedY;

    [SerializeField]
    private float maxDistanceFromOriginX, maxDistanceFromOriginY;

    private float speedX, speedY;

    private Vector2 curPos, newPos;

    private Game_Scaler gameScaler;

    // Start is called before the first frame update
    void Start()
    {
        if (minSpeedX == maxSpeedX){
            speedX = minSpeedX;
        }
        else{
            speedX = Random.Range(minSpeedX, maxSpeedX);
        }

        if (minSpeedY == maxSpeedY){
            speedY = minSpeedY;
        }
        else{
            speedY = Random.Range(minSpeedY, maxSpeedY);
        }

        gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        checkPos();

    }

    void move(){
        curPos = transform.position;
        newPos.x = curPos.x  + (gameScaler.Scale_Value_To_Screen_Width(speedX) * Time.deltaTime);
        newPos.y = curPos.y  + (gameScaler.Scale_Value_To_Screen_Height(speedY) * Time.deltaTime);
        transform.position = newPos;
    }
    

    void checkPos(){
        
        if (Mathf.Abs(transform.position.x) > gameScaler.Scale_Value_To_Screen_Width(maxDistanceFromOriginX) 
        || Mathf.Abs(transform.position.y) > gameScaler.Scale_Value_To_Screen_Height(maxDistanceFromOriginY)){            
            //Debug.Log("DESTROYING... LOCATION IS: " + Mathf.Abs(gameScaler.Scale_Value_To_Screen_Width(transform.position.x)) + ">" + gameScaler.Scale_Value_To_Screen_Width(maxDistanceFromOriginX));
            Destroy(this.gameObject);
        }
    }

}
