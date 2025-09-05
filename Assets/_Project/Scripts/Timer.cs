using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;


    void Update()
    {
        if (remainingTime > 0)
            remainingTime -= Time.deltaTime;
        else
            remainingTime = 0;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }



    //IEnumerator Countdown()
    //{
    //    if (countdown > 0)
    //    {
    //        countdown -= Time.deltaTime;
    //        display.text = "" + (int)countdown;
    //    }

    //    else
    //    {
    //        display.text = "GO !";
    //        yield return new WaitForSeconds(1f);
    //    }
            

        
    //}
}
