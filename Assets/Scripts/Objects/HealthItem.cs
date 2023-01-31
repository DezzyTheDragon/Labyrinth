using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        objectName = "Health Pack";
        objectTag = ObjectTags.Healing;
    }

    /*
    public override void OnPickup()
    {
        Debug.Log("Eyyy fuck you buddy!");
        NetworkServer.Destroy(this.gameObject);
    }
    */

}
