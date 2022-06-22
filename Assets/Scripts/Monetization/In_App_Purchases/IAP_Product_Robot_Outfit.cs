using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Robot_Outfit", menuName = "ScriptableObjects/IAP_Product_Robot_Outfit", order = 2)]
public class IAP_Product_Robot_Outfit : IAP_Product_Scriptable_Object
{
    public Sprite RobotOutfitSprite { get { return robotOutfitSprite; } private set { robotOutfitSprite = value; } }

    [SerializeField]
    private Sprite robotOutfitSprite;
}
