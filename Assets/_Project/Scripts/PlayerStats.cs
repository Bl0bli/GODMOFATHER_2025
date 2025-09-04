using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _score;
    
    public int Score { get { return _score; } }
    
    public void AddScore(int score) =>_score += score;
}

