using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameUI : CanvasUI<PregameUI>
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MapGen", LoadSceneMode.Single);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
