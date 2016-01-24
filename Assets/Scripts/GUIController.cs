using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

    // Use this for initialization

    public bool pauseOn = false;
    public Canvas PauseCanvas;

    public void Start()
    {


    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseOn == false) {
                Pause();
                            }
            else if (pauseOn == true)
            {
                UnPause();
             }
     
        }
    }

    public void LoadLevel()
    {
        Time.timeScale = 1f;
        Application.LoadLevel("Sandbox");
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void Pause()
    {

        pauseOn = true;
        PauseCanvas.enabled = true;
        Time.timeScale = 0f;
        Debug.Log("time is actually paused");
    }
    public void UnPause()
    {
        pauseOn = false;
        PauseCanvas.enabled = false;
        Time.timeScale = 1f;
        Debug.Log("time is actually unpaused");

    }

}
