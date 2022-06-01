using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class Metrics_Object
{

    // GamePlay
    public double numGameStartups; // Implemented
    public double profileCreationTimeUnix; // Sort of Implemented... Fix this
    public double secondsPlayed; // Implemented
    public double maxCoinsAlltime; // Implemented
    public double maxAltAllTime; // Implemented

    // FLights
    public int numNonAutopilotFlights {get; private set;} // Implemented
    public double rocketGameTotalNonAutopilotGemsCollected {get; private set;} // Implemented
    public double rocketGameTotalNonAutopilotGemsCollectedSquared {get; private set;} // Implemented
    public int numAutopilotFlights {get; private set;} // Implemented
    public double rocketGameTotalAutopilotGemsCollected {get; private set;} // Implemented 
    public double rocketGameTotalAutopilotGemsCollectedSquared {get; private set;} // Implemented


    // Mine Game
    public int numMineGamePlays; // Implemented
    public double mineGameCoinsCollected; // Implemented

    // Mine Cart
    public double minecartCoinsCollected; // Implemented


    // Ads
    public int numInterstitialAdsWatched; // Implemented
    public int numRewardedAdsOfferedRocketFlight; // Implemented
    public int numRewardedAdsWatchedRocketFlight; // Implemented
    public int numRewardedAdsOfferedMineGame; // Implemented
    public int numRewardedAdsWatchedMineGame; // Implemented


    public Metrics_Object(){
        // GamePlay
        numGameStartups = 1;
        profileCreationTimeUnix = (double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        secondsPlayed = 0;
        maxCoinsAlltime = 0;
        maxAltAllTime = 0;

        // Flights
        numNonAutopilotFlights = 0;
        numAutopilotFlights = 0;
        rocketGameTotalNonAutopilotGemsCollected = 0;
        rocketGameTotalNonAutopilotGemsCollectedSquared = 0;
        rocketGameTotalAutopilotGemsCollected = 0;
        rocketGameTotalAutopilotGemsCollectedSquared = 0;


        // Mine Game
        numMineGamePlays = 0;
        mineGameCoinsCollected = 0;

        // Mine Cart
        minecartCoinsCollected = 0;


        // Ads
        numInterstitialAdsWatched = 0;
        numRewardedAdsOfferedRocketFlight = 0;
        numRewardedAdsWatchedRocketFlight = 0;
        numRewardedAdsOfferedMineGame = 0;
        numRewardedAdsWatchedMineGame = 0;
    }

    
    public Metrics_Object(
                // GamePlay
                double NumGameStartups,
                double ProfileCreationTimeUnix,
                double SecondsPlayed,
                double MaxCoinsAllTime,
                double MaxAltAllTime,
                //Flights
                int NumNonAutopilotFlights,
                int NumAutopilotFlights,
                double RocketGameTotalNonAutopilotGemsCollected,
                double RocketGameTotalNonAutopilotGemsCollectedSquared,
                double RocketGameTotalAutopilotGemsCollected,
                double RocketGameTotalAutopilotGemsCollectedSquared,
                // Mine Game
                int NumMineGamePlays,
                double MineGameCoinsCollected,
                // Mine Cart
                double MinecartCoinsCollected,
                // Ads
                int NumInterstitialAdsWatched,
                int NumRewardedAdsOfferedRocketFlight,
                int NumRewardedAdsWatchedRocketFlight,
                int NumRewardedAdsOfferedMineGame,
                int NumRewardedAdsWatchedMineGame
                ){

        numGameStartups = NumGameStartups;
        profileCreationTimeUnix = ProfileCreationTimeUnix;
        secondsPlayed = SecondsPlayed;
        maxCoinsAlltime = MaxCoinsAllTime;
        maxAltAllTime = MaxAltAllTime;

        numNonAutopilotFlights = NumNonAutopilotFlights;
        numAutopilotFlights = NumAutopilotFlights;
        rocketGameTotalNonAutopilotGemsCollected = RocketGameTotalNonAutopilotGemsCollected;
        rocketGameTotalNonAutopilotGemsCollectedSquared = RocketGameTotalNonAutopilotGemsCollectedSquared;
        rocketGameTotalAutopilotGemsCollected = RocketGameTotalAutopilotGemsCollected;
        rocketGameTotalAutopilotGemsCollectedSquared = RocketGameTotalAutopilotGemsCollectedSquared;


        // Mine Game
        numMineGamePlays = NumMineGamePlays;
        mineGameCoinsCollected = MineGameCoinsCollected;

        // Mine Cart
        minecartCoinsCollected = MinecartCoinsCollected;

        // Ads
        numInterstitialAdsWatched = NumInterstitialAdsWatched;
        numRewardedAdsOfferedRocketFlight = NumRewardedAdsOfferedRocketFlight;
        numRewardedAdsWatchedRocketFlight = NumRewardedAdsWatchedRocketFlight;
        numRewardedAdsOfferedMineGame = NumRewardedAdsOfferedMineGame;
        numRewardedAdsWatchedMineGame = NumRewardedAdsWatchedMineGame;
    }


    public double getMeanRocketGameNonAutopilotGemsCollected(){
        return rocketGameTotalNonAutopilotGemsCollected / numNonAutopilotFlights;
    }

    // https://math.stackexchange.com/questions/198336/how-to-calculate-standard-deviation-with-streaming-inputs
    public double getStdRocketGameNonAutopilotGemsCollected(){
        return Math.Sqrt((rocketGameTotalNonAutopilotGemsCollectedSquared/numNonAutopilotFlights) - (Math.Pow(getMeanRocketGameNonAutopilotGemsCollected(), 2.0)/numNonAutopilotFlights));
    }

    public void updateFlights(double flightGems, bool autopilot, float altitude){
        Debug.Log("UPDATING FLIGHT METRICS: " + flightGems + " GEMS --- AUTOPILOT? " + autopilot + " --- ALTITUDE: " + altitude);
        
        if (autopilot){
            numAutopilotFlights++;
            rocketGameTotalAutopilotGemsCollected += flightGems;
            rocketGameTotalAutopilotGemsCollectedSquared += Math.Pow(flightGems, 2.0);
        }
        else{
            numNonAutopilotFlights++;
            rocketGameTotalNonAutopilotGemsCollected += flightGems;
            rocketGameTotalNonAutopilotGemsCollectedSquared += Math.Pow(flightGems, 2.0);
            if (maxAltAllTime <= altitude){
                maxAltAllTime = altitude;
            }
        }
    }
}
