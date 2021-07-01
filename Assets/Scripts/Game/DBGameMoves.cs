using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using Photon.Pun;

public class DBGameMoves : MonoBehaviour
{
    [Header("DbVars")]
    MongoClient client = new MongoClient("mongodb+srv://chessgame:chessgame@chessgamecluster.poidg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase db;
    IMongoCollection<BsonDocument> collection;

    PhotonView PV;
    BsonDocument doc;
    public string player1;
    public string player2;
    string moves;
    // Start is called before the first frame update
    void Start()
    {
        db = client.GetDatabase("PlayersDB");
        collection = db.GetCollection<BsonDocument>("GamesMoves");
        Debug.Log(GetLatestGameNumber());

        PV = GetComponent<PhotonView>();
        //Only the master client is sending the data to the database
        if (!PhotonNetwork.IsMasterClient)
        {
            //Gets this client's  usename and sends it to the master client
            string myName = GameObject.Find("UserHandler").GetComponent<UserHandler>().username;
            PV.RPC("Player2Name", RpcTarget.MasterClient, myName);
            Destroy(this);
            return;
        }
        
       
        doc = new BsonDocument();
        moves = "";
        player1= GameObject.Find("UserHandler").GetComponent<UserHandler>().username;

        

    }
    [PunRPC]
    public void Player2Name(string username)
    {
        player2 = username;
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public void AddMove(string move)
    {
        moves+= move;
    }
    public  void InsertGameRecord()
    {
        //Adds the game number
        int gameNumber= GetLatestGameNumber() + 1;

        //Adds the game number
        doc.Add(new BsonElement("GameNumber", gameNumber ));

        //Adds the players name
        doc.Add(new BsonElement("Player1", player1));
        doc.Add(new BsonElement("Player2", player2));

        //Adds the moves log
        doc.Add(new BsonElement("Moves", moves));

        collection.InsertOne(doc);
    }   

    public int GetLatestGameNumber()
    {
        return int.Parse(GetAllGameRecords("GameNumber")[1].GetValue("GameNumber").ToString());
    }
    //Get All the game records on the database and sorts it by the sort value
    public List<BsonDocument> GetAllGameRecords(string sortValue)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var sortDefenition = Builders<BsonDocument>.Sort.Descending(sortValue);

        List<BsonDocument> games = collection.Find<BsonDocument>(filter).Sort(sortDefenition).ToList<BsonDocument>();
        return games;
    }
}
