using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/PowerUp")]
public class PowerUp : ScriptableObject
{
    [Header("Classic")]
    public string powerUpName = "KK";
    public Sprite icon;
    
    // public abstract void ApplyEffect(GameObject player);         // Appliquer l'effet du PowerUp au joueur
}

