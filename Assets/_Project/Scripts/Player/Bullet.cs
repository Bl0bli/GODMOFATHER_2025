using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour, IInteractable
{
    [SerializeField, Range(1,10)] private float _MAXlifeTime = 5f;
    [SerializeField, Range(1, 10)] private float _MINlifeTime = 1f;
    [SerializeField, Range(1, 5)] private float _MAXSize = 5f;
    [SerializeField, Range(1, 100)] private int _MAXScore = 10;
    [SerializeField, Range(1f, 10f)] private float _MAXSpeed = 4f;
    [SerializeField] Rigidbody2D rb;

    public UnityEvent UnityOnDestroy;
    
    float _lifeTime = 0f;
    float _size = 0f;
    private int _score = 0;
    float _speed = 0f;
	Player _shooter;
    private Coroutine _lifeTimeRoutine;
    
    public int Score => _score;
    public Player Shooter => _shooter;
    
    public void InitShooter(Player shooter) => _shooter = shooter;
   

    public void StartLifeTime(float timePressed, Vector2 direction)
    {
        _score = (int)Mathf.Lerp(_score, _MAXScore, timePressed);
        _lifeTime = Mathf.Lerp(_MINlifeTime, _MAXlifeTime, timePressed);
        _speed = Mathf.Lerp(_speed, _MAXSpeed, timePressed);
        _size = Mathf.Lerp(_size, _MAXSize, timePressed);
        transform.localScale = new Vector3(_size, _size, _size);
        rb.AddForce(direction.normalized * _speed * 50, ForceMode2D.Impulse);
        _lifeTimeRoutine = StartCoroutine(LifeTimeRoutine(timePressed));
    }

    IEnumerator LifeTimeRoutine(float timePressed)
    {
        while (_lifeTime > 0f)
        {
            _size = Mathf.Lerp(_size / 4, _size, _lifeTime);
            transform.localScale = new Vector3(_size, _size, _size);
            _lifeTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Trigger(Player player = null)
    {
        if(player != null) player.Hit(this);
    }

    public void EndLifeTime()
    {
        StopCoroutine(_lifeTimeRoutine);
        _lifeTimeRoutine = null;
        UnityOnDestroy?.Invoke();
        Destroy(gameObject);
    }
}
