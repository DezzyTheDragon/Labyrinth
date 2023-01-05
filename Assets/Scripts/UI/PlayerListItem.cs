using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;


public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionID;
    public ulong PlayerSteamID;
    //TODO: Add support for player profile pics

    public TextMeshProUGUI PlayerNameText;

    private void Start()
    {
        
    }

    public void SetPlayerValues()
    {
        PlayerNameText.text = PlayerName;
    }
}
