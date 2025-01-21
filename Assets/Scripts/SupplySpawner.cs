using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    [Header("Spawn Ranges")]
    [SerializeField] private RectTransform supply1SpawnLeft;
    [SerializeField] private RectTransform supply1SpawnRight;
    private float supply1Left;
    private float supply1Right;

    private float armorSupplyTimer;
    private readonly float armorSupplyCD = 31f;

    private float fuelSupplyTimer;
    private readonly float fuelSupplyCD = 83f;

    private float artillerySupplyTimer;
    private readonly float artillerySupplyCD = 29f;

    private float aircraftSupplyTimer;
    private float aircraftSupplyCD = 29f;


    [SerializeField] private GameObject[] supplyPrefs;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        AdjustSpawnRange();
    }

    private void Update()
    {
        if (PlayerController.gameOn)
        {
            CheckForArmorSupplySpawn();

            CheckForFuelSupplySpawn();

            CheckForArtillerySupplySpawn();

            CheckForAircraftSupplySpawn();
        }
    }

    private void AdjustSpawnRange()
    {
        supply1Left = supply1SpawnLeft.position.x;
        supply1Right = supply1SpawnRight.position.x;
    }

    private void CheckForArmorSupplySpawn()
    {
        armorSupplyTimer += Time.deltaTime;

        if (armorSupplyTimer > armorSupplyCD)
        {
            SpawnArmor();
        }
    }

    private void SpawnArmor()
    {
        ItemSpawn(0);
        armorSupplyTimer = 0;
    }

    private void CheckForFuelSupplySpawn()
    {
        fuelSupplyTimer += Time.deltaTime;

        if (fuelSupplyTimer > fuelSupplyCD)
        {
            SpawnFuel();
        }
    }
    private void SpawnFuel()
    {
        ItemSpawn(1);
        fuelSupplyTimer = 0;
    }

    private void CheckForArtillerySupplySpawn()
    {
        if (GameManager.totalEnemiesDown >= 100 && playerController.playerArtillery == 1)
        {
            artillerySupplyTimer += Time.deltaTime;

            if (artillerySupplyTimer > artillerySupplyCD && playerController.playerArtillery == 1)
                SpawnArtillery();
        }
        
        if (GameManager.totalEnemiesDown >= 250 && playerController.playerArtillery == 2)
        {
            artillerySupplyTimer += Time.deltaTime;

            if (artillerySupplyTimer > artillerySupplyCD && playerController.playerArtillery == 2)
                SpawnArtillery();
        }

        if (GameManager.totalEnemiesDown >= 350 && playerController.playerArtillery == 3)
        {
            artillerySupplyTimer += Time.deltaTime;

            if (artillerySupplyTimer > artillerySupplyCD && playerController.playerArtillery == 3)
                SpawnArtillery();
        }
        
        if (GameManager.totalEnemiesDown >= 450 && playerController.playerArtillery == 4)
        {
            artillerySupplyTimer += Time.deltaTime;

            if (artillerySupplyTimer > artillerySupplyCD && playerController.playerArtillery == 4)
                SpawnArtillery();
        }
    }

    private void SpawnArtillery()
    {
        ItemSpawn(3);
        artillerySupplyTimer = 0;
    }

    private void CheckForAircraftSupplySpawn()
    {
        if (GameManager.totalEnemiesDown >= 175 && playerController.playerAircraft == 1)
        {
            aircraftSupplyTimer += Time.deltaTime;

            if (aircraftSupplyTimer > aircraftSupplyCD && playerController.playerAircraft == 1)
                SpawnAircraft();
        }
        else if (GameManager.totalEnemiesDown >= 300 && playerController.playerAircraft == 2)
        {
            aircraftSupplyTimer += Time.deltaTime;

            if (aircraftSupplyTimer > aircraftSupplyCD && playerController.playerAircraft == 2)
                SpawnAircraft();
        }
        else if (GameManager.totalEnemiesDown >= 400 && playerController.playerAircraft == 3)
        {
            aircraftSupplyTimer += Time.deltaTime;

            if (aircraftSupplyTimer > aircraftSupplyCD && playerController.playerAircraft == 3)
                SpawnAircraft();
        }
        else if (GameManager.totalEnemiesDown >= 500 && playerController.playerAircraft == 4)
        {
            aircraftSupplyTimer += Time.deltaTime;

            if (aircraftSupplyTimer > aircraftSupplyCD && playerController.playerAircraft == 4)
                SpawnAircraft();
        }
    }

    private void SpawnAircraft()
    {
        ItemSpawn(2);
        aircraftSupplyTimer = 0;
    }

    private void ItemSpawn(int supplyPrefNumber)
    {
        float spawnPositionX = Random.Range(supply1Left, supply1Right);
        Vector3 spawnPos = new(spawnPositionX, 5.5f, 0);
        Instantiate(supplyPrefs[supplyPrefNumber], spawnPos, Quaternion.identity);
    }
}
