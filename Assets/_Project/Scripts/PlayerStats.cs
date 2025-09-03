using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PowerUp effect;
    public int score;

    public PlayerStats(PowerUp effect, int score)
    {
        this.effect = effect;
        this.score = score;
    }
}

