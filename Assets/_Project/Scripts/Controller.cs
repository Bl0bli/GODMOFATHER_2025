using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Connect();
    }

    void Connect()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyUp(KeyCode.Space) == false)
        {
            Debug.Log("huh");
            float secondsHold = 4;
            secondsHold -= Time.deltaTime;
            Debug.Log(secondsHold);
            if (secondsHold <= 0)
            {
                Debug.Log("TAMERE");
            }
            
        }
    }
}
