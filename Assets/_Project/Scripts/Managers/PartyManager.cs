using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] List<Transform> _playerSpawns = new List<Transform>();
    public static PartyManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
        GameManager.Instance.SpawnPlayers(_playerSpawns);
        
        //wait 3 2 1
        GameManager.Instance.BeginGame();
    }
}
