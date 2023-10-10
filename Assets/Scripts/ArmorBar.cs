using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    private RectTransform armorBar;
    private Image barImage;

    void Start()
    {
        armorBar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        SetArmorBarSize(UIManager.totalArmor);
    }

    public void DamageBar(float damage)
    {
        UIManager.totalArmor -= damage;

        if (UIManager.totalArmor < 0)
        {
            UIManager.totalArmor = 0;
        }

        if (UIManager.totalArmor > 1)
        {
            UIManager.totalArmor = 1;
        }

        if (UIManager.totalArmor <= 0.7f && UIManager.totalArmor > 0.3f)
        {
            barImage.color = Color.yellow;
        }
        else if (UIManager.totalArmor <= 0.3f)
        {
            barImage.color = Color.red;
        }
        else
        {
            barImage.color = Color.green;
        }

        SetArmorBarSize(UIManager.totalArmor);
    }

    public void SetArmorBarSize(float size)
    {
        armorBar.localScale = new Vector3(size, 1f);
    }
}
