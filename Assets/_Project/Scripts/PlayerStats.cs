using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //[SerializeField] private int _score;
    [SerializeField] private int _MAXLife;
    [SerializeField] private float _autoHealCooldown = 5f;
    [SerializeField] private float _autoHealSpeedinSeconds = 3f;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _spriteOuch;
    [SerializeField] private Sprite _spriteDead;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private bool _debug = false;

    private Coroutine _autoHealRoutine;
    private Coroutine _shakeRoutine;
    [SerializeField, ShowIf("_debug")] private int _life;
    private Vector3 _basePos;
    private bool _cantHeal = false;
    

    
    //public int Score { get { return _score; } }
    public int Life { get { return _life; } }
    public Sprite SpriteOuch { get { return _spriteOuch; }  set { _spriteOuch = value; } }
    public Sprite SpriteDead { get { return _spriteDead; }  set { _spriteDead = value; } }
    public Sprite DefaultSprite { get { return _defaultSprite; }  set { _defaultSprite = value; } }
    
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
       if (_life <= 0f)
       {
           _cantHeal = true;
           _renderer.sprite = _spriteDead;
           GameManager.Instance.EndGame();
       }

       if (_cantHeal) return;
       else if (_life < _MAXLife / 2)
       {
           Debug.Log("Half");
           _renderer.sprite = _spriteOuch;
           if (_shakeRoutine == null)
               _shakeRoutine = StartCoroutine(ShakeRoutine());
       }
       else if (_life > _MAXLife / 2)
       {
           if(_defaultSprite != null) _renderer.sprite = _defaultSprite;
           if (_shakeRoutine != null)
           {
               StopCoroutine(_shakeRoutine);
               _shakeRoutine = null;
           }

       }if (deltaLife < 0)
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

       int startLife = _life; 
       float elapsed = 0f;

       while (elapsed < _autoHealSpeedinSeconds)
       {
           elapsed += Time.deltaTime;
           float t = Mathf.Clamp01(elapsed / _autoHealSpeedinSeconds);
           
           int targetLife = Mathf.RoundToInt(Mathf.Lerp(startLife, _MAXLife, t));
           
           int delta = targetLife - _life;
           if (delta != 0)
               UpdateLife(delta);  

           yield return null;
       }
       
       if (_life != _MAXLife)
           UpdateLife(_MAXLife - _life);
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

