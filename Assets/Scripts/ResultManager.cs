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
    [SerializeField] GameObject titleButton;
    [SerializeField] RectTransform titleButtonTransform;
    [SerializeField] GameObject retryPanel;
    [SerializeField] GameObject wentOutPanel;
    [SerializeField] GameObject loadingImage;
    [SerializeField] RectTransform loadingImageTransform;
    bool isLoading = false;
    bool isPlayerReady = false;
    bool isEnemyReady = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        Judge();
        HideRetryPanel();
        HideWentOutPanel();
        loadingImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Title");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        wentOutPanel.SetActive(true);
        titleButton.SetActive(true);
        loadingImage.SetActive(false);
        isLoading = false;
        Vector2 pos = titleButtonTransform.anchoredPosition;
        pos.x = 0;
        pos.y = -150;
        titleButtonTransform.anchoredPosition = pos;
        retryButton.SetActive(false);
    }

    public void OnRetryButton()
    {
        isPlayerReady = true;
        retryButton.SetActive(false);
        titleButton.SetActive(false);
        loadingImage.SetActive(true);
        isLoading = true;
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
