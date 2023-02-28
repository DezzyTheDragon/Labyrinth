using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Victory!");
            Application.Quit();
        }
    }
}
