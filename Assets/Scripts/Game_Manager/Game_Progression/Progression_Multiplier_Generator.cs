using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Progression_Multiplier_Generator : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }


    public static double generateMineCartCoinsCapacityUpgradeValue(double mineCartCoinsCapacity){
        return Math.Round(mineCartCoinsCapacity * 1.1);
    }

    public static double generateMineCartCoinsPerSecondUpgradeValue(double mineCartCoinsPerSecond){
        return Math.Round(mineCartCoinsPerSecond * 1.1);
    }

    public static double generateMineGameHitCoinsUpgradeValue(double mineGameHitCoins){
        return Math.Round(mineGameHitCoins * 1.1);
    }

    public static double generateMineGameSolveCoinsUpgradeValue(double mineGameSolveCoins){
        return Math.Round(mineGameSolveCoins * 1.1);
    }


    public static double generateMineCartCoinsCapacityUpgradePriceValue(double mineCartCoinsCapacityUpgradePrice){
        return Math.Round(mineCartCoinsCapacityUpgradePrice * 1.1);
    }

    public static double generateMineCartCoinsPerSecondUpgradePriceValue(double mineCartCoinsPerSecondUpgradePrice){
        return Math.Round(mineCartCoinsPerSecondUpgradePrice * 1.1);
    }

    public static double generateMineGameHitCoinsUpgradePriceValue(double mineGameHitCoinsUpgradePrice){
        return Math.Round(mineGameHitCoinsUpgradePrice * 1.1);
    }

    public static double generateMineGameSolveCoinsUpgradePriceValue(double mineGameSolveCoinsUpgradePrice){
        return Math.Round(mineGameSolveCoinsUpgradePrice * 1.1);
    }
    
}
