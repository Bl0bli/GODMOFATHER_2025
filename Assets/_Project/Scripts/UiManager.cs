using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private float countdown = 4;
    private int a = 0;
    private int b = 0;


    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            a = 1;
            Debug.Log("P1 OK");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            b = 1;
            Debug.Log("P2 OK");
        }

        if (a == 1 &&  b == 1)
        {
            countdownText.text = "" + (int)countdown;
            countdown -= Time.deltaTime;
            if(countdown <= 1)
            {
                countdownText.text = "FIGHT !";

            }

        }
    }

    void GamePlay()
    {
        Debug.Log("Jeu Lancé");
    }

    void Timer()
    {
        if (remainingTime > 0)
            remainingTime -= Time.deltaTime;
        else
            remainingTime = 0;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void WinnerScreen(Sprite spriteWinner, TextMeshProUGUI textWinner)
    {

    }
}
