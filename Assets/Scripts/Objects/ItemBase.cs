using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ObjectTags { Object, Weapon, Healing }

public class ItemBase : MonoBehaviour
{
    protected string objectName = "BaseObject";
    protected ObjectTags objectTag = ObjectTags.Object;

    public string GetName()
    {
        return objectName;
    }

    public ObjectTags GetTag()
    {
        return objectTag;
    }

    public void OnPickup()
    {
        Debug.Log("Base Object picked up");
        NetworkServer.Destroy(this.gameObject);
    }
}
