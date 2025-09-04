using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private string NameSceneLoad ;
    private float countdown = 4;
    

    private int a = 0;
    private int b = 0;

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

        if (a == 1 && b == 1)
        {
            countdownText.text = "" + (int)countdown;
            countdown -= Time.deltaTime;
            if (countdown <= 1)
            {
                countdownText.text = "FIGHT !";
                SceneManager.LoadScene(NameSceneLoad);
            }

        }
    }
}
