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
        SetArmorBarSize(GameManager.totalArmor);
    }

    public void DamageBar(float damage)
    {
        GameManager.totalArmor -= damage;

        if (GameManager.totalArmor < 0)
        {
            GameManager.totalArmor = 0;
        }

        if (GameManager.totalArmor > 1)
        {
            GameManager.totalArmor = 1;
        }

        if (GameManager.totalArmor <= 0.7f && GameManager.totalArmor > 0.3f)
        {
            barImage.color = Color.yellow;
        }
        else if (GameManager.totalArmor <= 0.3f)
        {
            barImage.color = Color.red;
        }
        else
        {
            barImage.color = Color.green;
        }

        SetArmorBarSize(GameManager.totalArmor);
    }

    public void SetArmorBarSize(float size)
    {
        armorBar.localScale = new Vector3(size, 1f);
    }
}
