using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //[SerializeField] private int _score;
    [SerializeField] private int _MAXLife;
    [SerializeField] private float _autoHealCooldown = 5f;
    [SerializeField] private float _autoHealSpeedinSeconds = 3f;

    private Coroutine _autoHealRoutine;
    private int _life;
    
    //public int Score { get { return _score; } }
    public int Life { get { return _life; } }
    
   //public void AddScore(int score) =>_score += score;

   private void Start()
   {
       _life = _MAXLife;
   }

   public void UpdateLife(int deltaLife)
   {
       _life += deltaLife;
       if (_life <= 0f)
       {
           GameManager.Instance.EndGame();
       }
       else if (deltaLife < 0)
       {
           if (_autoHealRoutine != null)
           {
               StopCoroutine(_autoHealRoutine);
               _autoHealRoutine = null;
           }

           _autoHealRoutine = StartCoroutine(HealRoutine());
       }
   }

   IEnumerator HealRoutine()
   {
       yield return new WaitForSeconds(_autoHealCooldown);
       float timeToHeal = _autoHealSpeedinSeconds;
       while (timeToHeal > 0f)
       {
           timeToHeal -= Time.deltaTime;
           _life = (int)Mathf.Lerp(_life, _MAXLife, timeToHeal);
           yield return null;
       }
   }
}

