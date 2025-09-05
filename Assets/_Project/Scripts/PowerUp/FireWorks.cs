using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/FireWorks")]
public class FireWorks : PowerUp
{
    public override void Apply(Player player)
    {
        player.HasFireWork = true;
    }

    public override void Remove(Player player)
    {
        player.HasFireWork = false;
    }
}
