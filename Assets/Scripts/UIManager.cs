using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{   
    //Score and Levels
    public static float totalArmor = 1f;
    public static float totalFuel = 1f;
    public static int playerAircraft = 1;
    public static int playerArtillery = 1;
    public static int totalScore = 0;
    public static int totalEnemiesDown = 0;
    public static int playerRank = 1;
    public static int totalAwards = 0;

    //Awards
    public static int currentAwardIndex = 0;
    
    public static int planeCrashes = 0;
    public static int totalSupplies = 0;
    public static int totalRefuels = 0;

    public static bool awardIndestructible = false;
    public static bool awardEndurance = false;
    public static bool awardUpComer = false;
    public static bool awardSmartWing = false;
    public static bool awardMadDog = false;
    public static bool awardBackliner = false;
    public static bool awardLastDrop = false;
    public static bool awardSharpshooter = false;
    public static bool awardThirstyJoe = false;
    public static bool awardFlyEmAll = false;
    public static bool awardArtilleryExpert = false;
    public static bool criticalArmor = false;
    public static bool awardPhoenix = false;
    public static bool awardAce = false;
    public static bool awardEliteFighter = false;
    public static bool awardAirForceHero = false;


    //Records
    public static int recordScore = 0;
    public static int recordEnemiesDown = 0;
    public static int recordRank = 1;
    public static int recordAwards = 0;

    private void Awake()
    {
        totalArmor = 1f;
        totalFuel = 1f;
        playerAircraft = 1;
        playerArtillery = 1;
        totalEnemiesDown = 0;
        totalScore = 0;
        EnemySpawner.destroyedPlanes = 0;
    }
}