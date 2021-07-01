using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MongoDB.Bson;
using UnityEngine.SceneManagement;

public class GameReplayScreen : MonoBehaviour
{
    [Header("Objests")]
    UserHandler userHandler;
    public Text numOfGamesPlayed;

    [Header("Prefabs")]
    public GameObject replayGameBtnPrefab;

    List<BsonDocument> gamesPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        userHandler = GameObject.Find("UserHandler").GetComponent<UserHandler>();

        GameReplayDBHandler.Start();
        gamesPlayed = GameReplayDBHandler.GetPlayerGames(userHandler.username);
        numOfGamesPlayed.text = $"You played {gamesPlayed.Count} games";

        CreateReplayGameButtons();
    }

    public void CreateReplayGameButtons()
    {
        GameObject temp;
        for (int i = 0; i < 5&&i<gamesPlayed.Count; i++)
        {
            //Initialize a new game replay button
            temp = Instantiate(replayGameBtnPrefab,transform);
            temp.transform.localPosition = new Vector3(0, 200 - i * 100);
            temp.GetComponentInChildren<Text>().text = $"Game {(i + 1)}";

            //Adds the relevant function to the button
            int gameNumber = int.Parse(gamesPlayed[i].GetValue("GameNumber").ToString());
            temp.GetComponent<Button>().onClick.AddListener(delegate { ReplayButtonClick(gameNumber); });
        }
    }
    public void ReplayButtonClick(int gameNumber)
    {
        userHandler.gameReplayNumber = gameNumber;
        SceneManager.LoadScene("GameReplay");
    }
}
