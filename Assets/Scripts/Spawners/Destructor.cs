using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{

    [SerializeField]
    private string tagToDestroy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == tagToDestroy){
            Destroy(col.gameObject);
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == tagToDestroy){
            Destroy(col.gameObject);
        }
    }
}
