using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine;

public class skinDatabase
{

    private string connectionString = "URI=file:" + Application.dataPath + "/SkinDatabase.db";
    private IDbConnection dbConnection;

    public skinDatabase()
    { }

    private void openDatabase()
    {
        dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();
    }

    private void closeDatabase()
    {
        dbConnection.Close();
    }

    public void newEntry(ulong id)
    {
        IDbCommand newSet = dbConnection.CreateCommand();
        newSet.CommandText = "INSERT INTO Skins (STEAMID, PLAYERSKINS, PISTOLSKINS, RIFLESKINS) VALUES (" + id + " , " + 1 + " , " + 1 + " , " + 1 + ")";
        newSet.ExecuteNonQuery();
    }

    //TODO: change the return type
    public void getEntry(ulong id)
    {
        openDatabase();

        IDbCommand skinQuery = dbConnection.CreateCommand();
        skinQuery.CommandText = "SELECT * FROM Skins WHERE STEAMID = " + id;
        IDataReader reader = skinQuery.ExecuteReader();

        int skinSet = 0;
        int pistolSet = 0;
        int rifleSet = 0;

        if (reader.Read())
        {
            skinSet = reader.GetInt32(1);
            pistolSet = reader.GetInt32(2);
            rifleSet = reader.GetInt32(3);
        }
        else
        {
            newEntry(id);
            skinSet = 1;
            pistolSet = 1;
            rifleSet = 1;
        }
        
        Debug.Log("Player: " + skinSet + " | Pistol: " + pistolSet + " | Rifle: " + rifleSet);

        reader.Close();

        closeDatabase();
    }
}
