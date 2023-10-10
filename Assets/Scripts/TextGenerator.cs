using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextGenerator : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] Transform canvas;
    [SerializeField] GameManager gameManager;

    string[] letters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    List<Text> textArray = new List<Text>();
    List<GameObject> textArrayObject = new List<GameObject>();
    List<string> arrangedLetters = new List<string>();
    RectTransform rectTransform;
    float textWidth = 20f;
    float textHight = 50f;
    
    int letterNumber = 10; //loopEachNumber*loopNumber

    int loopEachNumber = 2;
    int loopNumber = 5;
    List<string> loopText = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        SwitchGameType();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    void SwitchGameType()
    {
        int r = Random.Range(0, 2);
        switch (r)
        {
            case 0:
                letterNumber = Random.Range(5, 11);
                GenerateTextObject();
                RandomTextGenerate();
                break;
            case 1:
                loopEachNumber = Random.Range(2, 5);
                loopNumber = Random.Range(5, 10);
                letterNumber = loopEachNumber * loopNumber;
                GenerateTextObject();
                RoopTextGenerate();
                break;
        }
    }

    void GenerateTextObject()
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
            int r = Random.Range(0, letters.Length);
            textArray[i].text = letters[r];
            arrangedLetters.Add(letters[r]);
        }
    }

    void RoopTextGenerate()
    {
        for (int i = 0; i < loopEachNumber; i++)
        {
            int r = Random.Range(0, letters.Length);
            loopText.Add(letters[r]);
            for (int j = 0; j < loopNumber; j++)
            {
                textArray[j * loopEachNumber + i].text = letters[r];
            }
        }
        for (int i = 0; i < loopNumber; i++)
        {
            arrangedLetters.AddRange(loopText);
        }

    }

    void InputKey()
    {
        if (arrangedLetters == null) return;
        if (Input.GetKeyDown(arrangedLetters[0]))
        {
            TextColorChange(Color.black);
            arrangedLetters.RemoveAt(0);
            if (arrangedLetters.Count == 0)
            {
                arrangedLetters = null;
                StartCoroutine(gameManager.LoadScene());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            
        }
        else if (Input.GetMouseButtonDown(0))
        {

        }
        else if (Input.GetMouseButtonDown(1))
        {

        }
        else if (Input.GetMouseButtonDown(2))
        {

        }
        else if (Input.anyKeyDown)
        {
            TextColorChange(Color.red);
        }
    }

    void TextColorChange(Color color)
    {
        textArray[letterNumber - arrangedLetters.Count].color = color;
    }
}
