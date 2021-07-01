using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public static class GameReplayDBHandler
{
    [Header("DbVars")]
    static MongoClient client = new MongoClient("mongodb+srv://chessgame:chessgame@chessgamecluster.poidg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    static IMongoDatabase db;
    static IMongoCollection<BsonDocument> collection;

    // Start is called before the first frame update
    public static void Start()
    {
        db = client.GetDatabase("PlayersDB");
        collection = db.GetCollection<BsonDocument>("GamesMoves");
    }
    public static List<BsonDocument> GetAllGames()
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var sortDefenition = Builders<BsonDocument>.Sort.Descending("GameNumber");
        return collection.Find<BsonDocument>(filter).Sort(sortDefenition).ToList<BsonDocument>();
    }
    public static List<BsonDocument> GetPlayerGames(string username)
    {
        var filter1 = Builders<BsonDocument>.Filter.Eq("Player1", username);
        var filter2 = Builders<BsonDocument>.Filter.Eq("Player2", username);
        var sortDefenition = Builders<BsonDocument>.Sort.Descending("GameNumber");

        List<BsonDocument> games1 = collection.Find<BsonDocument>(filter1).Sort(sortDefenition).ToList<BsonDocument>();
        List<BsonDocument> games2 = collection.Find<BsonDocument>(filter2).Sort(sortDefenition).ToList<BsonDocument>();

        //Merges the two lists and sorts the by value
        List<BsonDocument> games = MergeGamesLists(games1, games2);
        games = SortByValue(games, "GameNumber");

        return games;
    }
    //Merges the two games list without duplicates
    public static List<BsonDocument> MergeGamesLists(List<BsonDocument> list1, List<BsonDocument> list2)
    {
        int gameNumber;
        for (int i = 0; i < list2.Count; i++)
        {
            gameNumber = int.Parse(list2[i].GetValue("GameNumber").ToString());
            if (!CheckGameExists(list1, gameNumber))
                list1.Add(list2[i]);
        }

        return list1;
    }
    //Checks if the game number value already exists in the list
    public static bool CheckGameExists(List<BsonDocument> list, int num)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i].GetValue("GameNumber") == num)
                return true;
        return false;
    }
    //Sorts a bson doc by a declered value
    public static List<BsonDocument> SortByValue(List<BsonDocument> list, string value)
    {
        BsonDocument temp;
        for (int i = 1; i < list.Count; i++)
        {
            for (int j = 1; j < list.Count; j++)
            {
                if (list[j - 1].GetValue(value) < list[j].GetValue(value))
                {
                    temp = list[j - 1];
                    list[j - 1] = list[j];
                    list[j] = temp;
                }
            }
        }
        return list;
    }
    //Returns a list of the game moves by its game number from the database
    public static List<string> GetGameMoves( int gameNumber)
    {
        string movesString = "";
        List<BsonDocument> games = GetAllGames();

        for (int i = 0; i < games.Count; i++)
        {
            if (games[i].GetValue("GameNumber") == gameNumber)
            {
                movesString = games[i].GetValue("Moves").ToString();
            }
        }
        if (movesString == "")
        {
            Debug.LogError("No moves where found for this game");
            return null;
        }
        return StringOfMovesToListOfMoves(movesString);

    }
    //Returns a list of the game moves by its game number from declared list
    public static List<string> GetGameMoves(List<BsonDocument> games, int gameNumber)
    {
        string movesString = "";
        for (int i = 0; i < games.Count; i++)
        {
            if (games[i].GetValue("GameNumber") == gameNumber)
            {
                movesString = games[i].GetValue("Moves").ToString();
            }
        }
        if (movesString == "")
        {
            Debug.LogError("No moves where found for this game");
            return null;
        }
        return StringOfMovesToListOfMoves(movesString);

    }
    //Converts the string format of moves to a list of moves
    public static List<string> StringOfMovesToListOfMoves(string movesString)
    {
        //The moves string need to be divided by four without module
        if(movesString.Length%4!=0)
        {
            Debug.LogError("Invalid moves string,needs to be divided by four without module");
            return null;
        }    

        List<string> movesList = new List<string>();
        for (int i = 0; i < movesString.Length/4; i++)
        {
            movesList.Add(movesString.Substring(i * 4, 4));
        }
        return movesList;
    }
}
