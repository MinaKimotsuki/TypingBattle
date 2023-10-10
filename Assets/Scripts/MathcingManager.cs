using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathcingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text playerNickNameText;
    [SerializeField] GameObject writeNickNameText;

    bool isJoinedRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        writeNickNameText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LoadScene();
    }

    public void OnMatchingButton()
    {
        if (playerNickNameText.text == "")
        {
            writeNickNameText.SetActive(true);
        }
        else
        {
            PhotonNetwork.NickName = playerNickNameText.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        isJoinedRoom = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    void LoadScene()
    {
        if (isJoinedRoom == false) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
