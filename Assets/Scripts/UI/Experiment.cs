using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Denomination{
    Gems = 0,
    Coins = 1
}

public enum ExperimentId{
    Autopilot_System = 1,
    Lateral_Boosters = 2,
    Particle_Shield = 3,
    Cow_Catcher = 4,
    Turbo_Boost = 5,
    Da_Bomb = 6,
    Gem_Magnet = 7
} // If we have one of these that isn't an upgrade, make sure the int value isn't in the upgrade enum values

public enum Duration{
    Permanent = 1,
    Three_Launches = 2,
    Three_Uses = 3
}

public class Experiment
{

    public ExperimentId experimentId;
    public string experimentName;
    public string ExperimentDescription;
    public bool isPersistent;
    public Denomination denomination;
    public double price;
    public Duration duration;
    public Sprite experimentSprite;


    public Experiment(ExperimentId ExperimentId, string ExperimentName, string ExperimentDescription, bool IsPersistent, Denomination Denomination, double Price, Duration Duration, Sprite ExperimentSprite){
        experimentId = ExperimentId;
        experimentName = ExperimentName;
        ExperimentDescription = ExperimentDescription;
        isPersistent = IsPersistent;
        denomination = Denomination;
        price = Price;
        duration = Duration;
        experimentSprite = ExperimentSprite;
    }
}
