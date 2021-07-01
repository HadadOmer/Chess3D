using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour
{
    [Header("Objects")]
    public Text levelText;
    public Text gamesPlayedText;
    public Text winrateText;


    public void DisplayStats() 
    {
        UserHandler user = GameObject.Find("UserHandler").GetComponent<UserHandler>();

        levelText.text = "Level:" + user.level;
        gamesPlayedText.text = "Games played:" + (user.wins + user.loses);

        if(user.wins + user.loses!=0)
        {
            float winrate = (float)user.wins / (float)(user.wins + user.loses) * 100;
            winrateText.text = $"Winrate:{winrate}%";
        }
        else
            winrateText.text = $"Winrate:0%";
    }
}
