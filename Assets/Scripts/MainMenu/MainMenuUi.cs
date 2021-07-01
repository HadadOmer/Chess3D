using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    public NetworkManager networkManager;
 
    [Header("Objects")]
    public List<Transform> screens;
    public Camera mainCamera;

    [Header("Input fields")]
    public InputField createRoomIP;
    public InputField joinRoomIP;

    [Header("Texts")]
    public Text createRoomAlert;
    public Text joinRoomAlert;
    public Text roomName;
    public Text muteText;

   

    // Start is called before the first frame update
    void Start()
    {
        DisableAllScreens();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        networkManager.ui = this;

        if (networkManager.connectedToMaster)
            EnableScreen("MainMenu");
    }  

    //Sets active false all the screens in the creens list
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

    public void CreateRoomClick()
    {
        string roomName = createRoomIP.text;
        networkManager.CreateRoom(roomName);
    }
    public void JoinRoomClick()
    {
        string roomName = joinRoomIP.text;
        networkManager.JoinRoom(roomName);
    }

    public void LeaveRoomClick()
    {
        networkManager.LeaveRoom();
    }


    public void OpenScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MuteOrUnmute()
    {
        if(!mainCamera.GetComponent<AudioSource>().mute)
        {
            mainCamera.GetComponent<AudioSource>().mute = true;
            muteText.text = "Unmute";
        }
        else
        {
            mainCamera.GetComponent<AudioSource>().mute = false;
            muteText.text = "Mute";
        }
    }
}
