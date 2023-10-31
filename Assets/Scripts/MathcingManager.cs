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
    [SerializeField] GameObject matchingButton;
    [SerializeField] GameObject loadingImage;
    [SerializeField] RectTransform loadingImageTransform;
    bool isLoading = false;
    bool isJoinedRoom = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        writeNickNameText.SetActive(false);
        loadingImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        LoadScene();
        if (!isLoading) return;
        Loading();
    }

    void Loading()
    {
        time += Time.deltaTime;
        if (time >= 0.5f)
        {
            loadingImageTransform.Rotate(0, 0, -45);
            time = 0;
        }
    }

    public void OnMatchingButton()
    {
        if (playerNickNameText.text == "")
        {
            writeNickNameText.SetActive(true);
        }
        else
        {
            matchingButton.SetActive(false);
            loadingImage.SetActive(true);
            isLoading = true;
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
