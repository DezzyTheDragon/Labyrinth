using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkinInfo {
    public SkinInfo(string n, Color c) 
    {
        name = n;
        color = c;
    }
    public string name;
    public Color color;
}

public class SkinUI : MonoBehaviour
{
    public GameObject skinScroll;
    public GameObject p_skinItem;

    private skinDatabase skinDatabase;
    private SkinData skinData;
    private SkinInfo[] skinInfos = { new SkinInfo("Default", Color.white), new SkinInfo("Yellow", Color.yellow),
        new SkinInfo("Green", Color.green), new SkinInfo("Blue", Color.blue), new SkinInfo("Red", Color.red)};
    private bool itemsGenerated = false;

    void Awake()
    {
        if (!itemsGenerated)
        { 
            skinDatabase = new skinDatabase();
            ulong steamID = (ulong)SteamUser.GetSteamID();
            //Debug.Log(steamID);
            skinDatabase.updateEntry(steamID, databaseCol.PISTOL, 2);
            skinData = skinDatabase.getEntry(steamID);
            //skinDatabase.DEBUG_RESET(steamID);
            //SkinData temp = skinDatabase.getEntry(steamID);
            //Debug.Log("Player: " + temp.playerSkin + " Pistol: " + temp.pistolSkin + " Rifle: " + temp.rifleSkin);
            PopulateList();
            itemsGenerated = true;
        }
    }

    private void PopulateList()
    {
        //Just do the pistol for now
        int target = 1;
        for (int i = 0; i < 5; i++)
        {
            GameObject skinItem = Instantiate(p_skinItem);
            SkinItem item = skinItem.GetComponent<SkinItem>();
            item.SetName(skinInfos[i].name);
            item.SetBackdropColor(skinInfos[i].color);
            item.SetID(i);
            if ((skinData.pistolSkin & target) == target)
            {
                item.SetEnabled(true);
            }
            else
            {
                item.SetEnabled(false);
            }
            skinItem.transform.SetParent(skinScroll.transform);
            skinItem.transform.localScale = Vector3.one;
            target = target << 1;
        }
    }
}
