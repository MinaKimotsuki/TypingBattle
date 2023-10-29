using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ResultManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text resultText;
    [SerializeField] GameObject retryButton;
    [SerializeField] RectTransform titleButton;
    [SerializeField] GameObject retryPanel;
    [SerializeField] GameObject wentOutPanel;
    bool isPlayerReady = false;
    bool isEnemyReady = false;

    // Start is called before the first frame update
    void Start()
    {
        Judge();
        HideRetryPanel();
        HideWentOutPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Judge()
    {
        if (GameDataManager.Instance.masterWinNumber > GameDataManager.Instance.anotherWinNumber)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
            else
            {
                resultText.text = "LOSE";
                resultText.color = Color.red;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                resultText.text = "LOSE";
                resultText.color = Color.red;
            }
            else
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
        }
    }

    public void OnTitleButton()
    {
        photonView.RPC(nameof(ShowWentOutPanel), RpcTarget.Others);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("Title");
    }

    [PunRPC]
    void ShowWentOutPanel()
    {
        wentOutPanel.SetActive(true);
        titleButton.position = new Vector3(0, -150, 0);
        retryButton.SetActive(false);
    }

    public void OnRetryButton()
    {
        isPlayerReady = true;
        retryButton.SetActive(false);
        photonView.RPC(nameof(OnRecieveRetryMessage), RpcTarget.Others);    
        if (isPlayerReady && isEnemyReady)
        {
            SceneManager.LoadScene("Main");
        }
    }

    [PunRPC]
    void OnRecieveRetryMessage()
    {
        isEnemyReady = true;
        if (isPlayerReady)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            ShowRetryMassage();
        }
    }

    void ShowRetryMassage()
    {
        retryPanel.SetActive(true);
    }

    void HideRetryPanel()
    {
        retryPanel.SetActive(false);
    }

    void HideWentOutPanel()
    {
        wentOutPanel.SetActive(false);
    }
}
