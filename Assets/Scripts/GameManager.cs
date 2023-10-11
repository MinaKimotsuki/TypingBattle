using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject finishText;
    bool isMasterclientFinished = false;
    bool isAnotherFinished = false;

    private void Start()
    {
        GameDataManager.Instance.SceneNumber++;
        HideCursor();
        HideFinishText();
    }

    private void Update()
    {
        OnClick();
        StartCoroutine(FinishJudge());
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
        ShowFinishText();
    }

    public void SetIsAnotherFinished()
    {
        isAnotherFinished = true;
        ShowFinishText();
    }

    IEnumerator FinishJudge()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Master:"+isMasterclientFinished);
        Debug.Log("another:"+isAnotherFinished);
        if (isMasterclientFinished == true && isAnotherFinished  == true)
        {
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

    void ShowFinishText()
    {
        finishText.SetActive(true);
    }
    
    void HideFinishText()
    {
        finishText.SetActive(false);
    }

    void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Cursor.visible == false) return;
            Cursor.visible = false;
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

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(isMasterclientFinished);
            }
            else
            {
                stream.SendNext(isAnotherFinished);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                isAnotherFinished = (bool)stream.ReceiveNext();
            }
            else
            {
                isMasterclientFinished = (bool)stream.ReceiveNext();
            }
        }
    }
}
