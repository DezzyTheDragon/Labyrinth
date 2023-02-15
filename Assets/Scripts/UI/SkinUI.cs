using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinUI : MonoBehaviour
{
    private skinDatabase skinDatabase;

    void Start()
    {
        skinDatabase = new skinDatabase();
        ulong steamID = (ulong)SteamUser.GetSteamID();
        Debug.Log(steamID);
        skinDatabase.getEntry(steamID);
    }
}
