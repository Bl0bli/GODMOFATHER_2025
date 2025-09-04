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
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _spriteOuch;

    private Coroutine _autoHealRoutine;
    private Coroutine _shakeRoutine;
    private int _life;
    private Vector3 _basePos;
    
    //public int Score { get { return _score; } }
    public int Life { get { return _life; } }
    public Sprite SpriteOuch { get { return _spriteOuch; }  set { _spriteOuch = value; } }
    
   //public void AddScore(int score) =>_score += score;

   private void Start()
   {
       _life = _MAXLife;
       _basePos = _renderer.transform.localPosition;
   }

   public void UpdateLife(int deltaLife)
   {
       _life += deltaLife;
       float delta = Mathf.InverseLerp(_MAXLife, 0, _life);
       float deltaScale = Mathf.Lerp(.5f, .8f, delta);
       transform.localScale = new Vector3(deltaScale, deltaScale, deltaScale);
       if (_life < _MAXLife / 2)
       {
           Debug.Log("Half");
           _renderer.sprite = _spriteOuch;
           if (_shakeRoutine == null)
               _shakeRoutine = StartCoroutine(ShakeRoutine());
       }
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

   IEnumerator ShakeRoutine()
   {
       while (true)
       {
           Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * 0.1f; 
           _renderer.transform.localPosition = _basePos + randomOffset;
           yield return new WaitForSeconds(0.05f);
       }
   }
}

