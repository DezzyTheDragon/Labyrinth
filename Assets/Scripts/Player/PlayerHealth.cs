using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    //Variable that contains player state and updated with a command

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Player is downed
        }
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        healthSlider.value = (float)currentHealth / (float)maxHealth;
    }
}
