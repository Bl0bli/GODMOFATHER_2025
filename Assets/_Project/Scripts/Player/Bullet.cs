using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Range(1,10)] private float _MAXlifeTime = 5f;
    [SerializeField, Range(1, 5)] private float _MAXSize = 5f;
    [SerializeField, Range(1, 100)] private int _MAXScore = 10;
    [SerializeField, Range(1f, 10f)] private float _MAXSpeed = 4f;
    [SerializeField] Rigidbody2D rb;
    
    float _lifeTime = 0f;
    float _size = 0f;
    float _score = 0f;
    float _speed = 0f;

    public void StartLifeTime(float timePressed, Vector2 direction)
    {
        _score = Mathf.Lerp(_score, _MAXScore, timePressed);
        _lifeTime = Mathf.Lerp(_lifeTime, _MAXlifeTime, timePressed);
        _speed = Mathf.Lerp(_speed, _MAXSpeed, timePressed);
        _size = Mathf.Lerp(_size, _MAXSize, timePressed);
        rb.AddForce(direction.normalized * _speed * 100, ForceMode2D.Impulse);
        StartCoroutine(LifeTimeRoutine(timePressed));
    }

    IEnumerator LifeTimeRoutine(float timePressed)
    {
        while (_lifeTime > 0f)
        {
            _size = Mathf.Lerp(_size, _MAXSize, _lifeTime);
            transform.localScale = new Vector3(_size, _size, _size);
            _lifeTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
