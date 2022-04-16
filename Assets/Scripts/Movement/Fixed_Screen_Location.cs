using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed_Screen_Location : MonoBehaviour
{
    private GameObject Main_Camera;

    private Vector3 main_camera_offset;
    private float depth;
    
    private Vector3 newPos;
    // Start is called before the first frame update
    void Start()
    {
        Main_Camera = GameObject.Find("Main Camera");
        main_camera_offset = transform.position - Main_Camera.transform.position;
        depth = transform.position.z;
    }
    
    void OnLevelWasLoaded(){
        Main_Camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        newPos = Main_Camera.transform.position + main_camera_offset;
        newPos.z = depth;
        transform.position = newPos; 
    }
}
