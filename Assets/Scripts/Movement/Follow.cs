using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public GameObject leadObj;
    [SerializeField]
    private float offsetX, offsetY;
    [SerializeField]
    private float smoothSpeedX = 1.0f;
    [SerializeField]
    private float smoothSpeedY = 1.0f;
    [SerializeField]
    private bool scaleToScreenSize = false;

    public bool followX = true; // Should we be following in the X direction
    public bool followY = true; // Should we be following in the Y direction
    private Vector3 curPosition;

    // Start is called before the first frame update
    void Start()
    {
        curPosition = new Vector3();
        if (scaleToScreenSize){
            Game_Scaler gameScaler = GameObject.Find("Game_Scaler").GetComponent<Game_Scaler>();
            offsetX = gameScaler.Scale_Value_To_Screen_Width(offsetX);
            offsetY = gameScaler.Scale_Value_To_Screen_Height(offsetY);
        }
    }

    // // Update is called once per frame
    // void FixedUpdate()
    // {
    //     Vector3 desiredPosition = leadObj.transform.position + new Vector3(offsetX, offsetY, transform.position.z);
    //     Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

    //     transform.position = smoothedPosition;
    // }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 posX = new Vector3(transform.position.x, 0.0f, 0.0f);
        Vector3 posY = new Vector3(0.0f, transform.position.y, 0.0f);
        Vector3 posZ = new Vector3(0.0f, 0.0f, transform.position.z);

        Vector3 desiredPositionX = new Vector3(leadObj.transform.position.x + offsetX, 0.0f, 0.0f);
        Vector3 desiredPositionY = new Vector3(0.0f, leadObj.transform.position.y + offsetY, 0.0f);

        Vector3 smoothedPositionX = Vector3.Lerp(posX, desiredPositionX, smoothSpeedX);
        Vector3 smoothedPositionY = Vector3.Lerp(posY, desiredPositionY, smoothSpeedY);

        curPosition.x = 0f;
        curPosition.y = 0f;
        curPosition.z = 0f;
        if (followX){
            curPosition += smoothedPositionX;
        }
        else{
            curPosition.y += transform.position.x;
        }
        if (followY){
            curPosition += smoothedPositionY;
        }
        else{
            curPosition.y += transform.position.y;
        }
        curPosition += posZ;

        transform.position = curPosition;
    }
}
