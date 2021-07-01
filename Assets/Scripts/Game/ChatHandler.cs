using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatHandler : MonoBehaviour
{
    public PhotonView PV;
    [Header("Objects")]
    public Button chatOpenButton;
    public InputField messegeIF;
    public Text chatBox;

    [Header("Values")]
    string username;
    List<ChatMessege> chatMesseges;
    private void Awake()
    {
        username = GameObject.Find("UserHandler").GetComponent<UserHandler>().username;
        PV = GetComponent<PhotonView>();
        chatMesseges = new List<ChatMessege>();
    }
    public void PostMessege()
    {           
        PV.RPC("RefreshChat", RpcTarget.All, username, messegeIF.text);
    }
    [PunRPC]
    public void RefreshChat(string username,string messege)
    {
        ChatMessege newMessege = new ChatMessege(username, messege);
        chatMesseges.Add(newMessege);
        chatBox.text = "";
        foreach (ChatMessege chatMessege in chatMesseges)
        {
            chatBox.text+=$"{chatMessege.username}:{chatMessege.messege}\n";
        }
    }

    public void OpenChat(GameObject chat)
    {
        chat.SetActive(true);
        chatOpenButton.gameObject.SetActive(false);
    }
    public void CloseChat(GameObject chat)
    {
        chat.SetActive(false);
        chatOpenButton.gameObject.SetActive(true);
    }
}
public class ChatMessege
{
    public string username;
    public string messege;

    public ChatMessege(string username,string messege)
    {
        this.username = username;
        this.messege = messege;
    }
}
