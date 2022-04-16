// This is a script that finds the bounding box of any object that has a SpriteRenderer
// Also finds out the pixel locations of the bounding boxes in pixels on the canvas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounding_Box_Finder : MonoBehaviour
{

    [SerializeField]
    private bool active = false;

    [SerializeField]
    private GameObject[] obj;

    [SerializeField]
    private float pxPerUnit = 54.102565f;
    [SerializeField]
    private int sceneWidthPx = 1708;
    [SerializeField]
    private int sceneHeightPx = 758;


    private float sceneWidth;
    private float sceneHeight;

    private bool firstUpdate = true;


    private Vector2 botLeft, botRight, topLeft, topRight;
    private Vector2 botLeftPx, botRightPx, topLeftPx, topRightPx;
    private Vector3 curCenter, curExtents;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Make sure we're checking after all the other scaling is done
    void LateUpdate()
    {
        if (firstUpdate && active){
            //object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
            
            foreach(object o in obj){
                GameObject g = (GameObject) o;
                if (g.GetComponent<SpriteRenderer>() != null) // If we end up finding an object that has a sprite renderer....
                {
                    curCenter = g.GetComponent<SpriteRenderer>().bounds.center;
                    curExtents = g.GetComponent<SpriteRenderer>().bounds.extents;

                    botLeft = new Vector2(curCenter.x - curExtents.x, curCenter.y - curExtents.y);
                    botRight = new Vector2(curCenter.x + curExtents.x, curCenter.y - curExtents.y);
                    topLeft = new Vector2(curCenter.x - curExtents.x, curCenter.y + curExtents.y);
                    topRight = new Vector2(curCenter.x + curExtents.x, curCenter.y + curExtents.y);

                    sceneWidth = ((float)sceneWidthPx) / pxPerUnit;
                    sceneHeight = ((float)sceneHeightPx) / pxPerUnit;

                    botLeftPx = gameLoc2PxLoc(botLeft);
                    botRightPx = gameLoc2PxLoc(botRight);
                    topLeftPx = gameLoc2PxLoc(topLeft);
                    topRightPx = gameLoc2PxLoc(topRight);


                    print("---------------------------------------------");
                    print(g.name);
                    print(g.GetComponent<SpriteRenderer>().bounds);
                    print("BotLeft GameLoc: " + botLeft);
                    print("BotRight GameLoc: " + botRight);
                    print("TopLeft GameLoc: " + topLeft);
                    print("TopRight GameLoc: " + topRight);
                    print(" ");
                    print("BotLeft PxLoc: " + botLeftPx);
                    print("BotRight PxLoc: " + botRightPx);
                    print("TopLeft PxLoc: " + topLeftPx);
                    print("TopRight PxLoc: " + topRightPx);
                    print("---------------------------------------------");


                }
            }
        }
        firstUpdate = false;
    }


    Vector2 gameLoc2PxLoc(Vector2 gameLoc)
    {
     float x = Mathf.Abs((-sceneWidth/2f) - gameLoc.x) * pxPerUnit;
     if (gameLoc.x < 0)
     {
        x = Mathf.Ceil(x);
     }
     else if (gameLoc.x > 0)
     {
        x = Mathf.Floor(x);
     }
     else
     {
        x = Mathf.Round(x);
     }
     float y = ((sceneHeight/2f) - gameLoc.y) * pxPerUnit;
    if (gameLoc.y < 0)
     {
        y = Mathf.Floor(y);
     }
     else if (gameLoc.y > 0)
     {
        y = Mathf.Ceil(y);
     }
     else
     {
        y = Mathf.Round(y);
     }
     return new Vector2(x, y);
    }


}
