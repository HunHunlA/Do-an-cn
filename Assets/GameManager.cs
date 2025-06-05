using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameObject popupDamage;
    public Player player;
    public GameObject panel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PopupDamage(Transform popupDamagePoint, int damage)
    {
        GameObject popup = Instantiate(popupDamage, popupDamagePoint.position, Quaternion.identity);
        popup.GetComponent<PopupDamage>().SetDamage(damage);
    }

}
