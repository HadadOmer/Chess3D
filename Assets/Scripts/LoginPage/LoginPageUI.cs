using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPageUI : MonoBehaviour
{

    [Header("Objects")]
    public List<Transform> screens;
    public UserHandler user;

    public Button loginButton;
    public Button playButton;
    public Button statsButton;

    public Text usernameText;
    public Text Level;
    
    


    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("UserHandler").GetComponent<UserHandler>();
        DisableAllScreens();
        EnableScreen("MainScreen");

        if(user.isLogined)
            UserLogined();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableAllScreens()
    {
        foreach (Transform screen in screens)
            screen.gameObject.SetActive(false);
    }
    //Sets active true the screen with the declared name
    public void EnableScreen(string name)
    {

        foreach (Transform screen in screens)
        {
            if (screen.name == name)
                screen.gameObject.SetActive(true);
            else
                screen.gameObject.SetActive(false);
        }
    }

    //Changes the relevent ui values when player logins
    public void UserLogined()
    {
        //Opens the main screen after login
        EnableScreen("MainScreen");

        //Deactivates the login button
        loginButton.gameObject.SetActive(false);

        //Inserts the relevent values to the text fields
        usernameText.text = UserHandler.GetValue("Email", user.email, "UserName");
        playButton.GetComponentInChildren<Text>().text = "Play";
        //Level.text= user.GetValue("Email", user.email, "XP");

        //Enables the stats button which leads to the stats screen
        statsButton.gameObject.SetActive(true);

        

    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
