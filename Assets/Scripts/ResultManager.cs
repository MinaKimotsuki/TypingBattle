using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text resultText;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Main");
    }
}
