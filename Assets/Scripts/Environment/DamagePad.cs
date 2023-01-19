using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePad : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player.DamagePlayer(damage);
        }
    }
}
