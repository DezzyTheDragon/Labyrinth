using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    [SerializeField] private int maxHealth = 100;
    [SyncVar] private int currentHealth;
    private PlayerMovementController playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        playerMovement = GetComponent<PlayerMovementController>();
    }

    public void Heal(int heal)
    {
        if (isOwned)
        { 
            CmdHealPlayer(heal);
        }
    }

    [Command]
    public void CmdHealPlayer(int heal)
    {
        currentHealth += heal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        healthSlider.value = (float)currentHealth / (float)maxHealth;
    }

    public void DamagePlayer(int damage)
    {
        if (isOwned)
        {
            CmdUpdatePlayerHealth(damage);
        }
    }

    [Command]
    public void CmdUpdatePlayerHealth(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerMovement.SetIsDowned();
        }
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        healthSlider.value = (float)currentHealth / (float)maxHealth;
    }
}
