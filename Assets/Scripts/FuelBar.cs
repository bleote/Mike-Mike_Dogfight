using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    private RectTransform fuelBar;

    void Start()
    {
        fuelBar = GetComponent<RectTransform>();
        SetFuelBarSize(GameManager.totalFuel);
    }

    public void FuelLoadBar(float fuelSpent)
    {
        GameManager.totalFuel -= fuelSpent;
        
        if (GameManager.totalFuel < 0 )
        {
            GameManager.totalFuel = 0;
        }

        if (GameManager.totalFuel > 1)
        {
            GameManager.totalFuel = 1;
        }

        SetFuelBarSize(GameManager.totalFuel);
    }

    public void SetFuelBarSize(float size)
    {
        fuelBar.localScale = new Vector3(size, 1f);
    }
}
