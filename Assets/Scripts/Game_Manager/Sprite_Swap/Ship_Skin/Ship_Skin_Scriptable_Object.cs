using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship_Skin", menuName = "ScriptableObjects/Ship_Skin", order = 5)]
public class Ship_Skin_Scriptable_Object : ScriptableObject
{
    public Sprite ShipSkinSprite { get { return shipSkinSprite; } private set { shipSkinSprite = value; } }
    public Sprite ParticleShieldSprite { get { return particleShieldSprite; } private set { particleShieldSprite = value; } }
    public int SkinId { get { return skinId; } private set {skinId = value;}}    

    [SerializeField]
    private int skinId;

    [SerializeField]
    private Sprite shipSkinSprite;

    [SerializeField]
    private Sprite particleShieldSprite;

}
