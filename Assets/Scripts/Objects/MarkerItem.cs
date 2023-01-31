using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerItem : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        objectName = "Marker";
        objectTag = ObjectTags.Marker;
    }
}
