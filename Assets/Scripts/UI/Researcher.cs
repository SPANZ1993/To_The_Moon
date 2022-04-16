using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Researcher
{


    public static System.Random random = new System.Random();

    public int researcherId;
    public List<Sprite> headshots;
    public Sprite barcode;
    public string name;
    public string favoriteLabel;
    public string favoriteText;
    public string description;
    public Func<double> generateTimeMultiplier;
    public Func<double> generateThrustMultiplier;

    public Researcher(int ResearcherId, List<Sprite> Headshots, Sprite Barcode, string Name, string FavoriteLabel, string FavoriteText, string Description, Func<double> GenerateTimeMultiplier, Func<double> GenerateThrustMultiplier){
        researcherId = ResearcherId;
        headshots = Headshots;
        barcode = Barcode;
        name = Name;
        favoriteLabel = FavoriteLabel;
        favoriteText = FavoriteText;
        description = Description;
        generateTimeMultiplier = GenerateTimeMultiplier;
        generateThrustMultiplier = GenerateThrustMultiplier;
    }


    public static double GetRandomNumber(double minimum, double maximum)
    { 
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

}
