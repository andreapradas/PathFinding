using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DBManager : MonoBehaviour
{
    private string dbUri = "URI=file:dbPathfinding.sqlite";
    private string SQL_COUNT_ELEMENTS = "SELECT count(*) FROM Players;";
    private string SQL_CREATE_PLAYERS = "CREATE TABLE IF NOT EXISTS Players (" +
                                        "PlayerID INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                        "Nombre TEXT NOT NULL, " +
                                        "Puntuacion INTEGER REFERENCES Puntuaciones);";
    private string SQL_CREATE_PUNTUACIONES = "CREATE TABLE IF NOT EXISTS Puntuaciones (" +
                                           "ID INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                           "Value INTEGER);";
    private string[] NOMBRES = {"Andrea","Juan","Maria","Carlos","Laura","Pedro","Ana","Sergio","Elena","David"};
    private int[] PUNTUACIONES = {16,30,50,60,80,100,0}; 
    private int NUM_PLAYERS = 100; 

    //Entrada por teclado del usuario
    public TMP_Text input;
    public Button confirmButton;

    void Start()
    {
        Debug.Log("Connect db");
        //Abrir db
        IDbConnection dbConnection = OpenDataBase();
        InitializeDB(dbConnection);
        //Añadir datos
        AddRandomData(dbConnection);
        //Buscar datos
        SearchByPuntuacion(dbConnection, 100);
        //Borrar datos
        DeletePlayerByName(dbConnection, "Andrea");
        //Update datos por teclado, al iniciar el juego
        confirmButton.onClick.AddListener(() => OnConfirmButtonClick(dbConnection));
        //Debug.Log("End connection db");
    }

    private void OnConfirmButtonClick(IDbConnection dbConnection)
    {
        string namePlayer = input.text;
        if (!string.IsNullOrEmpty(namePlayer)) 
        {
            UpdatePlayerInfo(dbConnection,namePlayer); 
            dbConnection.Close();
            Debug.Log("End connection db");
        }
    }

    private IDbConnection OpenDataBase()
    {
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();
        return dbConnection;
    }

    private void InitializeDB(IDbConnection dbConnection)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = SQL_CREATE_PUNTUACIONES + SQL_CREATE_PLAYERS;
        dbCommand.ExecuteReader();
    }

    private void AddRandomData(IDbConnection dbConnection)
    {
        if(CountNumberElements(dbConnection) > 0)//Para evitar tener elementos duplicados
        {
            return;
        }
        int num_puntuacion = PUNTUACIONES.Length; //6 monedas max puede recoger
        string command = "INSERT INTO Puntuaciones (Value) VALUES ";
        for (int i = 0; i < num_puntuacion; i++)
        {
            command += $"('{PUNTUACIONES[i]}'),";
        }
        command = command.Remove(command.Length - 1, 1);
        command += ";";
        //command += "INSERT INTO Players (Nombre, Puntuacion) VALUES ";
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
        List<int> puntuacionIds = new List<int>();
        dbCommand.CommandText = "SELECT ID FROM Puntuaciones;";
        IDataReader reader = dbCommand.ExecuteReader();
        while(reader.Read())
        {
            puntuacionIds.Add(reader.GetInt32(0));
        }
        reader.Close();
        command = "INSERT INTO Players (Nombre,Puntuacion) VALUES ";
        System.Random rnd = new System.Random();
        for(int i = 0; i < NUM_PLAYERS; i++)
        {
            string nombre = NOMBRES[rnd.Next(NOMBRES.Length)];
            int puntuacionId = puntuacionIds[rnd.Next(puntuacionIds.Count)];
            command += $"('{nombre}', '{puntuacionId}'),";
        }
        command = command.Remove(command.Length - 1, 1);
        command += ";";
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
    }

    private int CountNumberElements(IDbConnection dbConnection)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = SQL_COUNT_ELEMENTS;
        IDataReader reader = dbCommand.ExecuteReader();
        reader.Read();
        int count = reader.GetInt32(0);//Posicion de la columna de ese elemento(0)
        reader.Close();
        return count;
    }

    private void SearchByPuntuacion(IDbConnection dbConnection, int value)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = $"SELECT ID FROM Puntuaciones WHERE Value='{value}';";
        IDataReader reader = dbCommand.ExecuteReader();
        if (!reader.Read())
        {
            return;
        }
        int id_puntuacion = reader.GetInt32(0);
        reader.Close();
        dbCommand.CommandText = $"SELECT * FROM Players WHERE Puntuacion='{id_puntuacion}';";
        reader = dbCommand.ExecuteReader();
        string players = "";
        while(reader.Read())
        {
            players += $"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetInt32(2)}\n"; 
        }
        reader.Close();
        Debug.Log(players);
    }

    private void DeletePlayerByName(IDbConnection dbConnection, string nombre)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = $"DELETE FROM Players WHERE Nombre='{nombre}';";
        dbCommand.ExecuteNonQuery();
    }

    private void UpdatePlayerInfo(IDbConnection dbConnection, string nombre)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = $"INSERT INTO Players (Nombre, Puntuacion) VALUES ('{nombre}', 7);";
        dbCommand.ExecuteNonQuery();
    }
}
