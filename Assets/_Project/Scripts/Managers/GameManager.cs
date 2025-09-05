using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _gameTime = 120f;
    [SerializeField] private Sprite[] _playerSprites;
    [SerializeField] private Sprite[] _playerSpritesOuch;
    [SerializeField] private Sprite[] _playerDead;
    [SerializeField] private List<Sprite> _playerMovingSprites1 = new List<Sprite>();
    [SerializeField] private List<Sprite> _playerMovingSprites2 = new List<Sprite>();
    private List<Sprite>[] _playerMovingSprites = new List<Sprite>[2];
    
    [SerializeField] List<PlayerInput> _playerInputs = new List<PlayerInput>();
    
    public event Action<float> OnUpdateTime;
    public event Action<int> OnEndGame;

    public UnityEvent UnityOnPlayerJoined;
    public UnityEvent UnityOnShowUIEndGame;
    
    public static GameManager Instance;
    
    private int _playerAmount = -1;
    List<Player> _players = new List<Player>();
    private Coroutine _startGameRoutine;
    
    public List<Player> Players => _players;
    private Coroutine _timerRoutine;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
        
        DontDestroyOnLoad(gameObject);

        _playerMovingSprites[0] = _playerMovingSprites1;
        _playerMovingSprites[1] = _playerMovingSprites2;
        
    }

    public int GetPlayerID(Player player)
    {
        _playerAmount++;
        if(player != null)
        {
            _players.Add(player);
            player.Renderer.sprite = _playerSprites[_playerAmount];
            player.Stats.DefaultSprite = _playerSprites[_playerAmount];
            player.Stats.SpriteOuch = _playerSpritesOuch[_playerAmount];
            player.Stats.SpriteDead = _playerDead[_playerAmount];
            player.PlayerAnimator.PlayerMovingSprites = _playerMovingSprites[_playerAmount];
            player.PlayerAnimator.DefaultSprite = _playerSprites[_playerAmount];
            player.PlayerAnimator.SpriteOuch = _playerSpritesOuch[_playerAmount];
        }   
        return _playerAmount;
    }
    

    void SwitchScene()
    {
        ScenesManager.Instance.LoadScene("TristanScene");
    }

    public void SpawnPlayers(List<Transform> playerSpawns)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].gameObject.SetActive(true);
            _players[i].transform.position = playerSpawns[i].position;
            _players[i].transform.rotation = playerSpawns[i].rotation;
        }
    }
    public void BeginGame()
    {
        _timerRoutine = StartCoroutine(TimerRoutine(_gameTime));
        foreach (Player player in _players)
        {
            player.BlockInputs = false;
        }
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

    Player GetWinner()
    {
        Player winner = null;
        int life = 0;
        foreach (Player player in _players)
        {
            if (player.Stats.Life > life)
            {
                winner = player;
                life = player.Stats.Life;
            }
        }
        
        return winner;
    }

    public void EndGame()
    {
        Player winner = GetWinner();
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }
        UnityOnShowUIEndGame?.Invoke();
        OnEndGame?.Invoke(winner.PlayerID);
        foreach (Player player in _players)
        {
            player.BlockInputs = true;
        }
    }
    
    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
        UnityOnPlayerJoined?.Invoke();
        _playerInputs.Add(playerInput);
        Player player = playerInput.GetComponent<Player>();
        if(player != null) player.BlockInputs = true;
        
        //Afficher potentiellement l'image du perso associé
        
        if (_playerInputs.Count >= 2 && _startGameRoutine == null)
        {
            _startGameRoutine = StartCoroutine(WaitBeforeStartGame());
        }
    }

    IEnumerator WaitBeforeStartGame()
    {
        yield return new WaitForSeconds(1.5f);
        _startGameRoutine = null;
        SwitchScene();
    }

    public void Clear()
    {
        foreach (Player player in _players)
        {
            Destroy(player.gameObject);
        }
        
        Destroy(this.gameObject);
    }
}
