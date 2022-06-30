using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level_Scriptable_Object", menuName = "ScriptableObjects/Level_Scriptable_Object", order = 5)]
public class Level_Scriptable_Object :  ScriptableObject
{

    public int LevelId { get { return levelId; } private set { levelId = value; } }
    public string LevelName { get { return levelName; } private set { levelName = value; } }

    [SerializeField]
    private int levelId;
    [SerializeField]
    private string levelName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
