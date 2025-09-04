using NaughtyAttributes;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] bool _doesHaveADuration = false;
    [SerializeField, ShowIf("_doesHaveADuration")] private float duration = -1f;
    [SerializeField] private Sprite _icon;
    public float Duration => duration;
    public Sprite Icon => _icon;
    public string Name => _name;

    public abstract void Apply(Player player);
    public abstract void Remove(Player player);
}

