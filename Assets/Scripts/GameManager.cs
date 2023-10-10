using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject finishText;

    private void Start()
    {
        GameDataManager.Instance.SceneNumber++;
        HideCursor();
        HideFinishText();
    }

    private void Update()
    {
        OnClick();
    }

    public IEnumerator LoadScene()
    {
        ShowFinishText();
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
}
