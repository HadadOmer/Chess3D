using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameReplayUiHandler : MonoBehaviour
{
    public Text endText;  
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
