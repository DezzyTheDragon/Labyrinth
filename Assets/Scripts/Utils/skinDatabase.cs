using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine;

public enum databaseCol { PLAYER, PISTOL, RIFLE}

public struct SkinData {
    public int playerSkin;
    public int pistolSkin;
    public int rifleSkin;
}

public class skinDatabase
{

    private string connectionString = "URI=file:" + Application.dataPath + "/SkinDatabase.db";
    private IDbConnection dbConnection;

    public skinDatabase()
    { }

    //Opens the database
    private void openDatabase()
    {
        dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
    }

    //Closes the database
    private void closeDatabase()
    {
        dbConnection.Close();
    }

    //Insert a new user into the database with default values
    public void newEntry(ulong id)
    {
        IDbCommand newSet = dbConnection.CreateCommand();
        newSet.CommandText = "INSERT INTO Skins (STEAMID, PLAYERSKINS, PISTOLSKINS, RIFLESKINS) VALUES (" + id + " , " + 1 + " , " + 1 + " , " + 1 + ")";
        newSet.ExecuteNonQuery();
    }

    //Get what skins the user has in their registry
    public SkinData getEntry(ulong id)
    {
        SkinData skinData = new SkinData();

        openDatabase();

        IDbCommand skinQuery = dbConnection.CreateCommand();
        skinQuery.CommandText = "SELECT * FROM Skins WHERE STEAMID = " + id;
        IDataReader reader = skinQuery.ExecuteReader();


        if (reader.Read())
        {
            skinData.playerSkin = reader.GetInt32(1);
            skinData.pistolSkin = reader.GetInt32(2);
            skinData.rifleSkin = reader.GetInt32(3);
        }
        else
        {
            newEntry(id);
            skinData.playerSkin = 1;
            skinData.pistolSkin = 1;
            skinData.rifleSkin = 1;
        }
        

        reader.Close();

        closeDatabase();

        return skinData;
    }

    //Update a particular skinset for a user
    public void updateEntry(ulong id, databaseCol col, int skinID)
    {
        //Get whatever the player currently has
        SkinData data = getEntry(id);
        //Skins are stored by setting a specific bit to 1
        //bit position to represent skin is determened by id
        int skinValue = 1 << skinID;
        string collum;
        switch (col)
        {
            case databaseCol.PLAYER:
                skinValue = skinValue | data.playerSkin;
                collum = "PLAYERSKINS";
                break;
            case databaseCol.PISTOL:
                skinValue = skinValue | data.pistolSkin;
                collum = "PISTOLSKINS";
                break;
            case databaseCol.RIFLE:
                skinValue = skinValue | data.rifleSkin;
                collum = "RIFLESKINS";
                break;
            default:
                Debug.LogError("Invalid collum selection");
                return;
        }
        
        openDatabase();
        IDbCommand update = dbConnection.CreateCommand();
        update.CommandText = "UPDATE Skins SET " + collum + " = " + skinValue + " WHERE STEAMID = " + id;
        update.ExecuteNonQuery();
        closeDatabase();
    }

    //This function is only used to reset an entry to default values after debugging
    public void DEBUG_RESET(ulong id)
    {
        if (Application.isEditor)
        {
            openDatabase();
            IDbCommand reset = dbConnection.CreateCommand();
            reset.CommandText = "UPDATE Skins Set PLAYERSKINS = " + 1 + " , PISTOLSKINS = " + 1 + " , RIFLESKINS = " + 1 + " WHERE STEAMID = " + id;
            reset.ExecuteNonQuery();
            closeDatabase();
        }
    }
}
