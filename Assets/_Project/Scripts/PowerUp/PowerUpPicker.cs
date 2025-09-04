using UnityEditor;
using UnityEngine;

public class PowerUpPicker : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp[] possibleEffects;
    private PowerUp chosenEffect;
    private SpriteRenderer sr;
    
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
    }

    public void Trigger(Player player = null)
    {
        if (player != null)
        {
            if (possibleEffects != null && possibleEffects.Length > 0)
            {
                chosenEffect = possibleEffects[Random.Range(0, possibleEffects.Length)];
                //sr.sprite = chosenEffect.Icon;
            }
            player.AddPowerUp(chosenEffect);
            Debug.Log($"ADD POWER UP {chosenEffect.Name}");
        }
    }
}
