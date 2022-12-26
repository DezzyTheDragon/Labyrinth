using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

/*======================================
 * Author: Destiny Dahlgren
 * Description: This script handles the
 *      management of steam lobbies and
 *      how to hanle certain callbacks
 *======================================*/

public class SteamLobby : MonoBehaviour
{
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinLobby;
    protected Callback<LobbyEnter_t> EnterLobby;

    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;
    //public GameObject HostButton;
    public TextMeshProUGUI LobbyTextName;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized");
            return;
        }

        manager = GetComponent<CustomNetworkManager>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinLobby = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        EnterLobby = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void StartLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    public void StopLobby()
    {
        if (NetworkServer.active)
        {
            //Stop the lobby
        }
        else
        {
            //disconnect from the lobby
            SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        }
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        Debug.Log("Lobby was created Succesfully");
        manager.StartHost();
        CSteamID steamID = new CSteamID(callback.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(steamID, HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(steamID, "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request to join lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //Everyone
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        CSteamID steamID = new CSteamID(callback.m_ulSteamIDLobby);
        LobbyTextName.text = SteamMatchmaking.GetLobbyData(steamID, "name");

        //Clients
        if (NetworkServer.active)
        {
            //If we are the host then there is no need to proceed
            return;
        }
        //Force navigate to the lobby page
        GameObject.Find("Manager").GetComponent<UIManager>().navigateButtonPress("LobbyPanel");

        manager.networkAddress = SteamMatchmaking.GetLobbyData(steamID, HostAddressKey);
        manager.StartClient();
    }
}
