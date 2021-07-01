using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardHandler : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject tableBlock;
    [Header("DbVars")]
    MongoClient client = new MongoClient("mongodb+srv://chessgame:chessgame@chessgamecluster.poidg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

    // Start is called before the first frame update
    void Start()
    {
        DisplayLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayLeaderBoard()
    {
        List<BsonDocument> users = UserHandler.GetAllUsers("Wins");
        int wins, loses;
        float winrate;
        string username;
        GameObject temp;
        for (int i = 0; i < users.Count&&i<10; i++)
        {
            //Getting the values from the db
            wins = int.Parse(users[i].GetValue("Wins").ToString());
            loses = int.Parse(users[i].GetValue("Loses").ToString());
            winrate=wins+loses>0? (float)wins / (float)(wins +loses) * 100:0;
            username = users[i].GetValue("UserName").ToString();

            //Adds the username block to the table
            temp=Instantiate(tableBlock,transform);
            temp.transform.localPosition = new Vector3(-240, 310 - (i * 90));
            temp.GetComponentInChildren<Text>().text = username;

            //Adds the winrate blocks to the table
            temp = Instantiate(tableBlock, transform);
            temp.transform.localPosition = new Vector3(240, 310 - (i * 90));
            temp.GetComponentInChildren<Text>().text = winrate.ToString();
        }
    }
   
}
