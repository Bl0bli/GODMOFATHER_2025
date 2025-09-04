using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _gameTime = 120f;
    [SerializeField] private Sprite[] _playerSprites;
    
    public event Action<float> OnUpdateTime;
    public event Action OnEndGame;

    public UnityEvent UnityOnShowUIEndGame;
    
    public static GameManager Instance;
    
    private int _playerAmount = -1;
    List<Player> _players = new List<Player>();
    private Coroutine _timerRoutine;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    public int GetPlayerID(Player player)
    {
        _playerAmount++;
        if(player != null)
        {
            _players.Add(player);
            player.Renderer.sprite = _playerSprites[_playerAmount];
        }
        if (_playerAmount >= 2) BeginGame();
        return _playerAmount;
    }
    

    void BeginGame()
    {
        _timerRoutine = StartCoroutine(TimerRoutine(_gameTime));
    }

    IEnumerator TimerRoutine(float time)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            OnUpdateTime?.Invoke(timer);
            yield return null;
        }

        EndGame();
    }

    public void EndGame()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }
        UnityOnShowUIEndGame?.Invoke();
        OnEndGame?.Invoke();
        foreach (Player player in _players)
        {
            player.BlockInputs = true;
        }
    }
}
