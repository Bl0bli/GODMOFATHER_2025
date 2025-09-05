using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<Sprite> _winnerScreen = new List<Sprite>();
    [SerializeField] private Image _imageWinnerScreen;

    [SerializeField] private TextMeshProUGUI _textTimer;
    
    Coroutine _printWinnerCoroutine;
    private void Start()
    {
        GameManager.Instance.OnEndGame += PrintWinnerPanel;
        GameManager.Instance.OnUpdateTime += UpdateTimer;
    }

    private void UpdateTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        _textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    void PrintWinnerPanel(int PlayerID)
    {
        if(_printWinnerCoroutine == null)
            _printWinnerCoroutine = StartCoroutine(PrintWinnerPanelRoutine(PlayerID));
    }

    IEnumerator PrintWinnerPanelRoutine(int playerID)
    {
        yield return new WaitForSeconds(0.5f);
        _imageWinnerScreen.sprite = _winnerScreen[playerID];
        _imageWinnerScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        GameManager.Instance.Clear();
        ScenesManager.Instance.LoadScene("StartScene");
    }
}
