using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject finishText;
    [SerializeField] GameObject enemyFinishText;
    [SerializeField] Text playerNameText;
    [SerializeField] Text enemyNameText;
    [SerializeField] GameObject playerNameTextObject;
    [SerializeField] GameObject enemyNameTextObject;
    [SerializeField] Text judgementText;
    [SerializeField] GameObject judgementTextObject;
    [SerializeField] GameObject canvas;
    bool isMasterclientFinished = false;
    bool isAnotherFinished = false;
    string judgement;

    private void Start()
    {
        GameDataManager.Instance.SceneNumber++;
        HideCursor();
        HideFinishText(finishText);
        HideFinishText(enemyFinishText);
        HideJudgementText();
    }

    private void Update()
    {
        OnClick();
    }

    /*public void CallSetIsMasterclientFinished()
    {
        photonView.RPC(nameof(SetIsMasterclientFinished), RpcTarget.AllViaServer);
    }

    public void CallSetIsAnotherFinished()
    {
        photonView.RPC(nameof(SetIsAnotherFinished), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void SetIsMasterclientFinished()
    {
        Debug.Log("a");
        isMasterclientFinished = true;
        if (PhotonNetwork.IsMasterClient)
        {
            ShowFinishText();
        }
        FinishJudge();
    }

    [PunRPC]
    public void SetIsAnotherFinished()
    {
        Debug.Log("b");
        isMasterclientFinished = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            ShowFinishText();
        }
        FinishJudge();
    }*/

    public void SetIsMasterClientFinished()
    {
        isMasterclientFinished = true;
        ShowFinishText(finishText);
        if (isAnotherFinished)
        {
            judgement = "LOSE";
            photonView.RPC(nameof(FinishJudge), RpcTarget.AllViaServer);
        }
        else
        {
            judgement = "WIN";
        }
    }

    public void SetIsAnotherFinished()
    {
        isAnotherFinished = true;
        ShowFinishText(finishText);
        if (isMasterclientFinished)
        {
            judgement = "LOSE";

            Debug.Log(isMasterclientFinished);
            Debug.Log(isAnotherFinished);
            photonView.RPC(nameof(FinishJudge), RpcTarget.AllViaServer);
        }
        else
        {
            judgement = "WIN";
        }
    }

    [PunRPC]
    IEnumerator FinishJudge()
    {
        yield return new WaitForSeconds(1f);
        if (isMasterclientFinished == true && isAnotherFinished  == true)
        {
            ShowJudgementText(judgement);
            yield return new WaitForSeconds(1f);
            StartCoroutine("LoadScene");
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        if (GameDataManager.Instance.SceneNumber == 5)
        {
            GameDataManager.Instance.SceneNumber = 0;
            ShowCursor();
            SceneManager.LoadScene("Result");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ShowFinishText(GameObject finishText)
    {
        finishText.SetActive(true);
    }
    
    void HideFinishText(GameObject finishText)
    {
        finishText.SetActive(false);
    }

    public void ShowNameText()
    {
        playerNameTextObject.SetActive(true);
        enemyNameTextObject.SetActive(true);
        playerNameText.text = PhotonNetwork.LocalPlayer.NickName;
        enemyNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
    }

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.visible == false) return;
            HideCursor();
        }
    }

    void HideCursor()
    {
        Cursor.visible = false;
    }

    void ShowCursor()
    {
        Cursor.visible = true;
    }

    void HideJudgementText()
    {
        judgementTextObject.SetActive(false);
    }

    void ShowJudgementText(string judgement)
    {
        judgementTextObject.SetActive(true);
        judgementText.text = judgement;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isMasterclientFinished);
            stream.SendNext(isAnotherFinished);
        }
        else
        {
            isAnotherFinished = (bool)stream.ReceiveNext();
            isMasterclientFinished = (bool)stream.ReceiveNext();
        }
    }
}
