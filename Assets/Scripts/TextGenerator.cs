using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TextGenerator : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject textObject;
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] GameManager gameManager;

    string[] letters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    List<Text> textArray = new List<Text>();
    List<Text> enemyTextArray = new List<Text>();
    List<GameObject> textArrayObject = new List<GameObject>();
    List<GameObject> enemyTextArrayObject = new List<GameObject>();
    List<string> arrangedLetters = new List<string>();
    /*string[] masterArrangedLetters;
    string[] anotherArrangedLetters;*/
    RectTransform rectTransform;
    float textWidth = 20f;
    float textHight = 50f;

    int letterNumber = 10; //loopEachNumber*loopNumber

    int loopEachNumber = 2;
    int loopNumber = 5;
    List<string> loopText = new List<string>();
    int gameTypeNumber;
    int[] letter;
    /*bool isMasterArrangedLettersNullOrEmpty = true;
    bool isAnotherArrangedLettersNullOrEmpty = true;*/
    int[] masterTextColor;
    int[] anotherTextColor;
    Color[] colorTextArray = new Color[] { Color.white, Color.red, Color.black };


    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        /*UpdateEnemyText();*/
        /*SetIsAnotherrArrangedLettersNullOrEmpty();
        SetIsMasterArrangedLettersNullOrEmpty();*/
    }

    void Prepare()
    {
        gameTypeNumber = Random.Range(0, 2);
        switch (gameTypeNumber)
        {
            case 0:
                letterNumber = Random.Range(5, 11);
                masterTextColor = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    masterTextColor[i] = 0;
                }
                anotherTextColor = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    anotherTextColor[i] = 0;
                }
                letter = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    letter[i] = Random.Range(0, letters.Length);
                }
                break;
            case 1:
                loopEachNumber = Random.Range(2, 5);
                loopNumber = Random.Range(5, 10);
                letterNumber = loopEachNumber * loopNumber;
                masterTextColor = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    masterTextColor[i] = 0;
                }
                anotherTextColor = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    anotherTextColor[i] = 0;
                }
                letter = new int[letterNumber];
                for (int i = 0; i < letterNumber; i++)
                {
                    letter[i] = Random.Range(0, letters.Length);
                }
                break;
        }
        /*for (int i = 0; i < letterNumber; i++)
        {
            arrangedLetters.Add(letters[letter[i]]);
            Debug.Log("prepare: " + arrangedLetters[i]);
        }*/
    }

    public void SwitchGameType()
    {
        switch (gameTypeNumber)
        {
            case 0:
                GenerateTextObject(textArrayObject, textArray, player);
                GenerateTextObject(enemyTextArrayObject, enemyTextArray, enemy);
                RandomTextGenerate();
                break;
            case 1:
                GenerateTextObject(textArrayObject, textArray, player);
                GenerateTextObject(enemyTextArrayObject, enemyTextArray, enemy);
                RoopTextGenerate();
                break;
        }
    }

    void GenerateTextObject(List<GameObject> textArrayObject, List<Text> textArray, Transform canvas)
    {
        for (int i = 0; i < letterNumber; i++)
        {
            textArrayObject.Add(Instantiate(textObject, Vector3.zero, Quaternion.identity, canvas));
            textArray.Add(textArrayObject[i].GetComponent<Text>());
            rectTransform = textArrayObject[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector3(textWidth, textHight);
            rectTransform.localPosition = new Vector3(textWidth * (i - (letterNumber / 2)), 0, 0);
        }
    }

    void RandomTextGenerate()
    {
        for (int i = 0; i < letterNumber; i++)
        {
            textArray[i].text = letters[letter[i]];
            enemyTextArray[i].text = letters[letter[i]];
            arrangedLetters.Add(letters[letter[i]]);
            /*if (PhotonNetwork.IsMasterClient)
            {
                masterArrangedLetters = new string[letterNumber];
                masterArrangedLetters[i] = letters[letter[i]];
            }
            else
            {
                anotherArrangedLetters = new string[letterNumber];
                anotherArrangedLetters[i] = letters[letter[i]];
            }*/
        }
    }

    void RoopTextGenerate()
    {
        for (int i = 0; i < loopEachNumber; i++)
        {
            loopText.Add(letters[letter[i]]);
            for (int j = 0; j < loopNumber; j++)
            {
                textArray[j * loopEachNumber + i].text = letters[letter[i]];
                enemyTextArray[j * loopEachNumber + i].text = letters[letter[i]];
            }
        }
        for (int i = 0; i < loopNumber; i++)
        {
            arrangedLetters.AddRange(loopText);
            for (int j = 0; j < loopEachNumber; j++)
            {
                /*if (PhotonNetwork.IsMasterClient)
                {
                    masterArrangedLetters = new string[letterNumber];
                    masterArrangedLetters[j + loopEachNumber * i] = loopText[j];
                }
                else
                {
                    anotherArrangedLetters = new string[letterNumber];
                    anotherArrangedLetters[j + loopEachNumber * i] = loopText[j];
                }*/
            }
        }


    }

    void InputKey()
    {
        if (arrangedLetters == null) return;
        if (arrangedLetters.Count == 0) return;

        if (Input.GetKeyDown(arrangedLetters[0]))
        {
            TextColorChange(Color.black, letterNumber - arrangedLetters.Count);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(ChangeMasterTextColor), RpcTarget.All, 2, letterNumber - arrangedLetters.Count);
            }
            else
            {
                photonView.RPC(nameof(ChangeAnotherTextColor), RpcTarget.All, 2, letterNumber - arrangedLetters.Count);
            }
            photonView.RPC(nameof(UpdateEnemyTextColor), RpcTarget.Others);
            /*if (PhotonNetwork.IsMasterClient)
            {
                masterArrangedLetters[letterNumber - arrangedLetters.Count] = "";
            }
            else
            {
                anotherArrangedLetters[letterNumber - arrangedLetters.Count] = "";
            }*/
            arrangedLetters.RemoveAt(0);
            if (arrangedLetters.Count == 0)
            {
                arrangedLetters = null;
                if (PhotonNetwork.IsMasterClient)
                {
                    gameManager.CallSetIsMasterclientFinished();
                }
                else
                {
                    gameManager.CallSetIsAnotherFinished();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) { }
        else if (Input.GetMouseButtonDown(0)) { }
        else if (Input.GetMouseButtonDown(1)) { }
        else if (Input.GetMouseButtonDown(2)) { }
        else if (Input.anyKeyDown)
        {
            TextColorChange(Color.red, letterNumber - arrangedLetters.Count);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(ChangeMasterTextColor), RpcTarget.All, 1, letterNumber - arrangedLetters.Count);
            }
            else
            {
                photonView.RPC(nameof(ChangeAnotherTextColor), RpcTarget.All, 1, letterNumber - arrangedLetters.Count);
            }
            photonView.RPC(nameof(UpdateEnemyTextColor), RpcTarget.Others);
        }
    }

    [PunRPC]
    void ChangeMasterTextColor(int colorNumber, int number)
    {
        masterTextColor[number] = colorNumber;
    }
    [PunRPC]
    void ChangeAnotherTextColor(int colorNumber, int number)
    {
        Debug.Log(number);
        anotherTextColor[number] = colorNumber;
    }


    [PunRPC]
    void UpdateEnemyTextColor()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < letterNumber; i++)
            {
                enemyTextArray[i].color = colorTextArray[anotherTextColor[i]];
            }
        }
        else
        {
            for (int i = 0; i < letterNumber; i++)
            {
                enemyTextArray[i].color = colorTextArray[masterTextColor[i]];
            }
        }
    }


    /*void UpdateEnemyText()
    {
        if (masterArrangedLetters == null) return;
        if (anotherArrangedLetters == null) return;
        for (int i = 0; i < letterNumber; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (anotherArrangedLetters[i] == "")
                {
                    enemyTextArray[i].color = Color.black;
                }
            }
            else
            {
                if (masterArrangedLetters[i] == "")
                {
                    enemyTextArray[i].color = Color.black;
                }
            }
        }
    }*/
    

    void TextColorChange(Color color, int number)
    {
        textArray[number].color = color;
    }

    /*void SetIsMasterArrangedLettersNullOrEmpty()
    {
        if (masterArrangedLetters == null)
        {
            isMasterArrangedLettersNullOrEmpty = false;
        }
        else if (masterArrangedLetters.Length == 0)
        {
            isMasterArrangedLettersNullOrEmpty = false;
        }
        else
        {
            isMasterArrangedLettersNullOrEmpty = true;
        }

    }*/

    /*void SetIsAnotherrArrangedLettersNullOrEmpty()
    {
        if (anotherArrangedLetters == null)
        {
            isAnotherArrangedLettersNullOrEmpty = true;
        }
        else if (anotherArrangedLetters.Length == 0)
        {
            isAnotherArrangedLettersNullOrEmpty = true;
        }
        else
        {
            isAnotherArrangedLettersNullOrEmpty = false;
        }
    }*/

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameTypeNumber);
            stream.SendNext(letterNumber);
            stream.SendNext(loopEachNumber);
            stream.SendNext(loopNumber);
            // stream.SendNext(letter);

            stream.SendNext(letter);


            /*if (isMasterArrangedLettersNullOrEmpty == false)
            {
                stream.SendNext(masterArrangedLetters);

            }
            if (isAnotherArrangedLettersNullOrEmpty == false)
            {
                stream.SendNext(anotherArrangedLetters);

            }*/
            //stream.SendNext(anotherArrangedLetters);
        }
        else
        {
            gameTypeNumber = (int)stream.ReceiveNext();
            letterNumber = (int)stream.ReceiveNext();
            loopEachNumber = (int)stream.ReceiveNext();
            loopNumber = (int)stream.ReceiveNext();
            letter = (int[])stream.ReceiveNext();
            /*if (isMasterArrangedLettersNullOrEmpty == false)
            {
                masterArrangedLetters = (string[])stream.ReceiveNext();
            }
            if (isAnotherArrangedLettersNullOrEmpty == false)
            {
                anotherArrangedLetters = (string[])stream.ReceiveNext();
            }*/
        }
    }
}

