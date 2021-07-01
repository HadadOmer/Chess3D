using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    [Header("Objects")]
    public UserHandler user;
    LoginPageUI pageUI;

    public InputField emailIF;
    public InputField passwordIF;
    public Text alertText;

    [Header("DbVars")]
    MongoClient client = new MongoClient("mongodb+srv://chessgame:chessgame@chessgamecluster.poidg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase db;
    IMongoCollection<BsonDocument> collection;

    // Start is called before the first frame update
    void Start()
    {
        db = client.GetDatabase("PlayersDB");
        collection = db.GetCollection<BsonDocument>("PlayersCollection");

        user = GameObject.Find("UserHandler").GetComponent<UserHandler>();
        pageUI = GameObject.Find("Canvas").GetComponent<LoginPageUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckValidLogin()
    {
        string email = emailIF.text;
        string password = passwordIF.text;

        //Gets the password of the user from database
        var filter = Builders<BsonDocument>.Filter.Eq("Email", email);
        var projection = Builders<BsonDocument>.Projection.Include("Password").Exclude("_id");
        List<BsonDocument> result = collection.Find<BsonDocument>(filter).Project(projection).ToList();

        if (result.Count == 0)
            alertText.text="Email do not exist";
        else
        {
            string realPassword = result[0].GetValue("Password").ToString();
            if (realPassword != password)
                alertText.text = "Invalid password";
            else
            {
                if(user.Login(email))
                {
                   pageUI.UserLogined();
                }
                else
                {
                    alertText.text = "Login Failed";
                }
            }
        }
    }
}
