using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    private RectTransform fuelBar;

    void Start()
    {
        fuelBar = GetComponent<RectTransform>();
        SetFuelBarSize(UIManager.totalFuel);
    }

    public void FuelLoadBar(float fuelSpent)
    {
        UIManager.totalFuel -= fuelSpent;
        
        if (UIManager.totalFuel < 0 )
        {
            UIManager.totalFuel = 0;
        }

        if (UIManager.totalFuel > 1)
        {
            UIManager.totalFuel = 1;
        }

        SetFuelBarSize(UIManager.totalFuel);
    }

    public void SetFuelBarSize(float size)
    {
        fuelBar.localScale = new Vector3(size, 1f);
    }
}
