using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ItemBase : NetworkBehaviour
{
    public string objectName = "BaseObject";

    public string GetName()
    {
        return objectName;
    }

    public void OnPickup()
    {
        Debug.Log("Base Object picked up");
        NetworkServer.Destroy(this.gameObject);
    }
}
