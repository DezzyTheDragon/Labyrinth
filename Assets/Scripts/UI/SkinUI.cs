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
        //Debug.Log(steamID);
        //skinDatabase.getEntry(steamID);
        //skinDatabase.updateEntry(steamID, databaseCol.PLAYER, 2);
        //skinDatabase.DEBUG_RESET(steamID);
        //SkinData temp = skinDatabase.getEntry(steamID);
        //Debug.Log("Player: " + temp.playerSkin + " Pistol: " + temp.pistolSkin + " Rifle: " + temp.rifleSkin);
    }
}
