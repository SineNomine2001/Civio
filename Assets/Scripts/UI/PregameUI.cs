using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameUI : UI
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
