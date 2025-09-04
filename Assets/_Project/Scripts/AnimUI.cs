using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class AnimUI : MonoBehaviour
{

    [SerializeField] private float countdown = 20;
    [SerializeField] private TextMeshProUGUI display;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("Countdown");

    }
    
    IEnumerator Countdown()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            display.text = "" + (int)countdown;
        }

        else
        {
            display.text = "GO !";
            yield return new WaitForSeconds(1f);
        }
            

        
    }
}
