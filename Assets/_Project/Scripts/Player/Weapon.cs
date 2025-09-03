using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent OnShoot;
    [SerializeField] GameObject _projectile;
    [SerializeField] private Transform _anchorFire;
    [SerializeField] private Player _player;
    
    public void Fire(float timePressed, Vector2 dir)
    {
        if (timePressed > 0f)
        {
            OnShoot?.Invoke();
            GameObject bullet = Instantiate(_projectile, _anchorFire.position, _anchorFire.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.InitShooter(_player);
            bulletScript.StartLifeTime(timePressed, dir);
        }
    }

    public void EnableTripleShot()
    {
        Debug.Log("enable triple shot");
    }
}
