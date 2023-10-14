using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    [SerializeField] TextGenerator textGenerator;
    [SerializeField] GameManager gameManager;
    [SerializeField] Text CountDownText;
    [SerializeField] GameObject CountDownTextObject;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CountDown");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountDown()
    {
        gameManager.ShowNameText();
        CountDownText.text = "3";
        yield return new WaitForSeconds(1f);
        CountDownText.text = "2";
        yield return new WaitForSeconds(1f);
        CountDownText.text = "1";
        yield return new WaitForSeconds(1f);
        CountDownTextObject.SetActive(false);
        textGenerator.SwitchGameType();
    }
}
