using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PowerUpPicker : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp[] possibleEffects;
    private PowerUp chosenEffect;
    private SpriteRenderer sr;
    List<PowerUp> _powerUps;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:PowerUp");
        possibleEffects = new PowerUp[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            possibleEffects[i] = AssetDatabase.LoadAssetAtPath<PowerUp>(path);
        }
    }
#endif
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        _powerUps = possibleEffects.ToList();
    }

    public void Trigger(Player player = null)
    {
        if (player != null)
        {
            if (_powerUps != null && _powerUps.Count > 0)
            {
                chosenEffect = _powerUps[Random.Range(0, _powerUps.Count)];
                _powerUps.Remove(chosenEffect);
                //sr.sprite = chosenEffect.Icon;
            }
            player.AddPowerUp(chosenEffect);
            Debug.Log($"ADD POWER UP {chosenEffect.Name}");
        }
    }
}
