using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : NetworkBehaviour
{
    public TextMeshProUGUI healthText;
    public int maxHealth = 10;
    public GameObject UIContainer;
    [SyncVar(hook = nameof(UpdateEnemyHealth))] public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UIContainer.transform.LookAt(LobbyController.Instance.LocalPlayerObject.transform);
    }

    public void DamageEnemy(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            NetworkServer.Destroy(this.gameObject);
        }
    }

    public void UpdateEnemyHealth(int oldVal, int newVal)
    {
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
}
