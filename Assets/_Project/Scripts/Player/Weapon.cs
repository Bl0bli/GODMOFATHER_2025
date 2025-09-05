using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _projectile;
    [SerializeField] private Transform _anchorFire;
    [SerializeField] private Player _player;

    [Header("Params")] 
    [SerializeField] private float _spreadAngle = 15f;
    
    public UnityEvent OnShoot;

    private bool _tripleShot = false;
    
    public void Fire(float timePressed, Vector2 dir)
    {
        if(timePressed <= 0f) return;
        
        OnShoot?.Invoke();
        
        if (_tripleShot)
        {
            FireBullet(dir, 0f, timePressed);
            FireBullet(dir, _spreadAngle, timePressed);
            FireBullet(dir, -_spreadAngle, timePressed); 
        }
        else
        {
            FireBullet(dir, 0f, timePressed);
        }
    }

    void FireBullet(Vector2 dir, float angle, float timePressed)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle) * Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
        
        GameObject bullet = Instantiate(_projectile, _anchorFire.position, rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.InitShooter(_player);
        
        
        Vector2 newDir = Quaternion.Euler(0, 0, angle) * dir;
        bulletScript.StartLifeTime(timePressed, newDir);
    }

    public void EnableTripleShot() => _tripleShot = true;
    public void DisableTripleShot() => _tripleShot = false;
}
