using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return mineCartCoinsCapacity * 1.1;
    }

    public static double generateMineCartCoinsPerSecondUpgradeValue(double mineCartCoinsPerSecond){
        return mineCartCoinsPerSecond * 1.1;
    }

    public static double generateMineGameHitCoinsUpgradeValue(double mineGameHitCoins){
        return mineGameHitCoins * 1.1;
    }

    public static double generateMineGameSolveCoinsUpgradeValue(double mineGameSolveCoins){
        return mineGameSolveCoins * 1.1;
    }


    public static double generateMineCartCoinsCapacityUpgradePriceValue(double mineCartCoinsCapacityUpgradePrice){
        return mineCartCoinsCapacityUpgradePrice * 1.1;
    }

    public static double generateMineCartCoinsPerSecondUpgradePriceValue(double mineCartCoinsPerSecondUpgradePrice){
        return mineCartCoinsPerSecondUpgradePrice * 1.1;
    }

    public static double generateMineGameHitCoinsUpgradePriceValue(double mineGameHitCoinsUpgradePrice){
        return mineGameHitCoinsUpgradePrice * 1.1;
    }

    public static double generateMineGameSolveCoinsUpgradePriceValue(double mineGameSolveCoinsUpgradePrice){
        return mineGameSolveCoinsUpgradePrice * 1.1;
    }
    
}
