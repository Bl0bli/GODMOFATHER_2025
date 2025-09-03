using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleShot")]
public class TripleShot: PowerUp
{
    public override void Apply(Player player)
    {
        player.Weapon.EnableTripleShot();
    }

    public override void Remove(Player player)
    {
        player.Weapon.EnableTripleShot();
    }
}
