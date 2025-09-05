using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletBounce")]
public class BulletBounce : PowerUp
{
    
    public override void Apply(Player player)
    {
        player.HasBouncyBullets = true;
    }

    public override void Remove(Player player)
    {
        player.HasBouncyBullets = true;
    }
}
