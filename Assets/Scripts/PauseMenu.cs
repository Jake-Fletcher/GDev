using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;
    //private but editable in inspector
    [SerializeField]GameObject pauseMenu;

    private void Start()
    {
        //pauseMenu = GameObject.Find("FPS_Alt");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();
        }
        
    }

    public void togglePauseMenu()
    {
        pauseMenu.SetActive(!isPaused);
        isPaused = !isPaused;
        //0 if true, 1 if false
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void exit()
    {
        Application.Quit(0);
    }
}
