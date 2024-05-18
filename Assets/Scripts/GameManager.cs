using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using System.Data.Common;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int FPS=60;
    private bool startMenuLoaded = false;
    public Button confirmButton;
    public TMP_Text input;

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate=FPS;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "StartMenu" && !startMenuLoaded)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
            startMenuLoaded = true;
        }
        else if (currentScene.name == "StartMenu")
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
    }

    public void OnConfirmButtonClick()
    {
        string namePlayer = input.text;
        if (string.IsNullOrEmpty(namePlayer))
        {
            return;
        }else{
            SceneManager.LoadScene("MainScene");
            Debug.Log("Estamos en MainsScene");
        }
    }
}