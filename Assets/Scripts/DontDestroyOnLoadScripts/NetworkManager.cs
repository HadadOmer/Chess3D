using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public MainMenuUi ui;
    public bool connectedToMaster;

    // Start is called before the first frame update
    void Start()
    {
        if (instance!=null&&instance!=this)
        {           
            Destroy(this.gameObject);
            Destroy(this);
        }
        else
        {
            instance = this;
                   
           //Connects to the closest server on region
            PhotonNetwork.ConnectUsingSettings();

            //Keeps this script active when a new scene is load
            DontDestroyOnLoad(this);
        }
    }
    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null&& SceneManager.GetActiveScene().buildIndex == 1)
            DisplayRoomScreen();
    }
    //Connection to master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to region:"+PhotonNetwork.CloudRegion);

        //Displays the main menu only after the connection to the region was established
        ui.EnableScreen("MainMenu");

        connectedToMaster = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectedToMaster = false;
    }


    //Room Create
    #region
    public void CreateRoom(string roomName)
    {
        //Creates a room on the photon servers
        PhotonNetwork.CreateRoom(roomName);

    }
    public override void OnCreatedRoom()
    {
        //Ensures the scene will be synced for all client when the master is loading the scene
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log($"Room:{PhotonNetwork.CurrentRoom.Name} Created ");

        //Enables the room screen when a room is opened
        //DisplayRoomScreen();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //Displays the error messege on screen
        ui.createRoomAlert.text = message;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Only the master client need to load the level
        if (PhotonNetwork.IsMasterClient)
            LoadScene("Game");
    }
    #endregion


    //Room join
    #region
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //Joins a room on the photon server
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"Room:{PhotonNetwork.CurrentRoom.Name} Joined ");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Displays the error messege on screen
        ui.joinRoomAlert.text = message;
    }
    #endregion

    //Room Leave
    #region
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();       
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Room left");
        //Loads the main menu scene upon leaving a room
        SceneManager.LoadScene(1);
        //Gives some time for the scene to load to prevent bugs
        StartCoroutine(LoadMainMenu(0.1f));
       
    }
    IEnumerator LoadMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PhotonNetwork.IsConnected)
        {
            //Finds the ui script
            ui = GameObject.Find("Canvas").GetComponent<MainMenuUi>();

            ui.EnableScreen("MainMenu");
        }
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public IEnumerator ForceExit(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel(1);
    }
    public void DisplayRoomScreen()
    {
        ui = GameObject.Find("Canvas").GetComponent<MainMenuUi>();
        ui.EnableScreen("Room");
        ui.roomName.text = "Room Name:" + PhotonNetwork.CurrentRoom.Name;
    }
    #endregion
    public void LoadScene(string sceneName)
    {
        //Loads a level onto the phton server
        PhotonNetwork.LoadLevel(sceneName);
    }
}
