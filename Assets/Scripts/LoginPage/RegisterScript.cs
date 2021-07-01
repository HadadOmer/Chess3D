using MongoDB.Driver;
using MongoDB.Bson;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterScript : MonoBehaviour
{
    [Header("Objects")]
    public UserHandler user;
    LoginPageUI pageUI;
    public Text alertText;

    public InputField userNameIF;
    public InputField emailIF;
    public InputField passwordIF1;
    public InputField passwordIF2;

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
        pageUI= GameObject.Find("Canvas").GetComponent<LoginPageUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RegisterUser()
    {
        string username = userNameIF.text;
        string email = emailIF.text;
        string password =passwordIF1.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            alertText.text="Fill all fields";

        else if (CheckValueExists(username,"UserName"))
            alertText.text="User name already exists";
        else if (CheckValueExists(email, "Email"))
            alertText.text = "Email already exists";

        else if (!ValidEmail(email))
            alertText.text="InvalidEmail";
        else if (passwordIF1.text != passwordIF2.text)
            alertText.text = "Passwords do not match";
        else
        {
            var doc = new BsonDocument { { "UserName", username }, { "Email", email }, { "Password", password }, 
                { "XP", 0 }, { "Level", 1 },{ "Wins", 0 }, { "Loses", 0 }  };
            collection.InsertOne(doc);
            print("Registered");
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

    bool CheckValueExists(string value,string valueName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(valueName, value);
        var projection = Builders<BsonDocument>.Projection.Include("Email").Exclude("_id");
        List<BsonDocument> result = collection.Find<BsonDocument>(filter).Project(projection).ToList();
        return result.Count > 0;
    }

    bool ValidEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
