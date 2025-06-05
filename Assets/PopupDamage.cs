using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    public TextMeshPro damageText;
    public float destroyTime = 1f;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    public void SetDamage(int damage)
    {
        if (damageText != null)
        {
            damageText.text = damage.ToString();
        }
        else
        {
            Debug.LogWarning("Damage text component is not assigned.");
        }
    }
}
