using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    private float armorTimer;
    private float armorCD = 31f;

    private float fuelTimer;
    private float fuelCD = 83f;

    private bool aircraftDrop = false;

    private float artilleryTimer;
    private float artilleryCD = 57f;

    [SerializeField]
    private GameObject[] supplyPrefs;

    private void Update()
    {
        if (PlayerController.gameOn)
        {
            armorTimer += Time.deltaTime;

            if (armorTimer > armorCD)
            {
                SpawnArmor();
            }

            fuelTimer += Time.deltaTime;

            if (fuelTimer > fuelCD)
            {
                SpawnFuel();
            }

            artilleryTimer += Time.deltaTime;

            if (artilleryTimer > artilleryCD && aircraftDrop == false)
            {
                SpawnArtillery();
                aircraftDrop = true;
            }
            else if (artilleryTimer > artilleryCD && aircraftDrop == true)
            {
                SpawnAircraft();
                aircraftDrop = false;
            }
        }
        
    }

    private void SpawnArmor()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.75f, 5.75f), 5.5f, 0);
        Instantiate(supplyPrefs[0], spawnPos, Quaternion.identity);
        armorTimer = 0;
    }

    private void SpawnFuel()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.75f, 5.75f), 5.5f, 0);
        Instantiate(supplyPrefs[1], spawnPos, Quaternion.identity);
        fuelTimer = 0;
    }

    private void SpawnAircraft()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.75f, 5.75f), 5.5f, 0);
        Instantiate(supplyPrefs[2], spawnPos, Quaternion.identity);
        artilleryTimer = 0;
    }

    private void SpawnArtillery()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5.75f, 5.75f), 5.5f, 0);
        Instantiate(supplyPrefs[3], spawnPos, Quaternion.identity);
        artilleryTimer = 0;
    }
}
