using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Robot_Outfit", menuName = "ScriptableObjects/Robot_Outfit", order = 4)]
public class Robot_Outfit_Scriptable_Object : ScriptableObject
{
    public Sprite RobotOutfitSprite { get { return robotOutfitSprite; } private set { robotOutfitSprite = value; } }
    public int OutfitId { get { return outfitId; } private set {outfitId = value;}}    

    [SerializeField]
    private int outfitId;

    [SerializeField]
    private Sprite robotOutfitSprite;


}
