using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

}
