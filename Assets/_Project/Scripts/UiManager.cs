using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private float countdown = 4;


    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;

    [SerializeField] private Sprite winnerSprite;
    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private GameObject endPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endPanel.SetActive(false);
        if (winnerSprite == null)
        {
            return;
        }
        GameManager.Instance.OnUpdateTime += UpdateTimer;
        GameManager.Instance.OnEndGame += EndGame;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTimer(float time)
    {
        timerText.text = time.ToString();
    }

    void EndGame()
    {
       endPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
