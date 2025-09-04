using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBoost")]
public class PlayerSpeedBoost : PowerUp
{
    [SerializeField] private float multiplier = 2f;
    private float originalSpeed;

    public override void Apply(Player player)
    {
        originalSpeed = player.MoveSpeed;
        player.MoveSpeed *= multiplier;
    }

    public override void Remove(Player player)
    {
        player.MoveSpeed = originalSpeed;
    }
}
