using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas, controlsCanvas, creditsCanvas;
    
    void Start()
    {
        mainMenuCanvas.SetActive(true);
    }

    public void B_StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void B_Quit()
    {
        Application.Quit();
    }

    public void B_Credits()
    {
        mainMenuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }
    
    public void B_Controls()
    {
        mainMenuCanvas.SetActive(false);
        controlsCanvas.SetActive(true);
    }
}
