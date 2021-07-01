using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHandler : MonoBehaviour
{

    public static UserHandler instance;

    public bool isLogined;

    [Header("UserValues")]
    public string email;
    public string username;
    public int xp;
    public int level;
    public int wins;
    public int loses;

    [Header("DbVars")]    
    static MongoClient client = new MongoClient("mongodb+srv://chessgame:chessgame@chessgamecluster.poidg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

    public int gameReplayNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
         
            //Keeps this script active when a new scene is load
            DontDestroyOnLoad(this);

            isLogined = false;

            username = "Guest" + Random.Range(1000, 10000);

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Login(string email)
    {
        try
        {
            //Inserts the relevant values based on the database
            this.email = email;
            username = GetValue("Email", email, "UserName");
            xp = int.Parse(GetValue("Email", email, "XP"));
            level = int.Parse(GetValue("Email", email, "Level"));
            wins = int.Parse(GetValue("Email", email, "Wins"));
            loses = int.Parse(GetValue("Email", email, "Loses"));

            //Sets the is logined var to true
            isLogined = true;
            return true;
        }
        catch
        {
            Debug.LogError("User Data invalid");
            return false;
        }
    }



    //User values changes
    #region
    public void AddXP(int value)
    {
        xp += value;
        ChangeValue("Email", email, "XP", xp);

        level =1+( xp / 100);
        ChangeValue("Email", email, "Level", level);
    }
    public void AddWins(int value)
    {
        wins += value;
        ChangeValue("Email", email, "Wins", wins);
    }
    public void AddLoses(int value)
    {
        loses += value;
        ChangeValue("Email", email, "Loses", loses);
    }
    #endregion

    //Database functions
    #region
    //Gets the value from the database
    public static string GetValue(string primaryKeyName,string primaryKey,string valueName)
    {
        //Declares the database and collection
        IMongoDatabase db= client.GetDatabase("PlayersDB"); ;
        IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("PlayersCollection");

        //Gets the relevent bson document
        var filter = Builders<BsonDocument>.Filter.Eq(primaryKeyName, primaryKey);
        BsonDocument result = collection.Find<BsonDocument>(filter).FirstOrDefault();

        //Return null if value doesnt exist
        if (result == null)
            return null;

        //Returns the first occurance value 
        return result.GetValue(valueName).ToString();
    }
    //Changes a value in the database
    public static void ChangeValue(string primaryKeyName, string primaryKey,string valueName,object newValue)
    {
        //Declares the database and collection
        IMongoDatabase db = client.GetDatabase("PlayersDB"); ;
        IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("PlayersCollection");

        var filter = Builders<BsonDocument>.Filter.Eq(primaryKeyName, primaryKey);
        BsonDocument result = collection.Find<BsonDocument>(filter).FirstOrDefault();

        if(result!=null)
        {
            var update= Builders<BsonDocument>.Update.Set(valueName,newValue);
            collection.UpdateOne(filter, update);
        }
    }
    public static List<BsonDocument> GetAllUsers()
    {
        //Declares the database and collection
        IMongoDatabase db = client.GetDatabase("PlayersDB"); ;
        IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("PlayersCollection");

        var filter = Builders<BsonDocument>.Filter.Empty;

        List<BsonDocument> users = collection.Find<BsonDocument>(filter).ToList<BsonDocument>();
        return users;
    }
    public static List<BsonDocument> GetAllUsers(string sortValue)
    {
        //Declares the database and collection
        IMongoDatabase db = client.GetDatabase("PlayersDB"); ;
        IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("PlayersCollection");

        var filter = Builders<BsonDocument>.Filter.Empty;
        var sortDefenition= Builders<BsonDocument>.Sort.Descending(sortValue);

        List<BsonDocument> users = collection.Find<BsonDocument>(filter).Sort(sortDefenition).ToList<BsonDocument>();
        return users;
    }
    #endregion
}
