using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    [SerializeField] TextGenerator textGenerator;
    [SerializeField] GameManager gameManager;
    [SerializeField] Text playerCountDownText;
    [SerializeField] Text enemyCountDownText;
    [SerializeField] GameObject playerCountDownTextObject;
    [SerializeField] GameObject enemyCountDownTextObject;


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
        playerCountDownText.text = "3";
        enemyCountDownText.text = "3";
        yield return new WaitForSeconds(1f);
        playerCountDownText.text = "2";
        enemyCountDownText.text = "2";
        yield return new WaitForSeconds(1f);
        playerCountDownText.text = "1";
        enemyCountDownText.text = "1";
        yield return new WaitForSeconds(1f);
        playerCountDownTextObject.SetActive(false);
        enemyCountDownTextObject.SetActive(false);
        textGenerator.SwitchGameType();
        gameManager.ShowNameText();
    }
}
