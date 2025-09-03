using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent OnShoot;
    [SerializeField] GameObject _projectile;
    [SerializeField] private Transform _anchorFire;
    public void Fire(float timePressed, Vector2 dir)
    {
        if (timePressed > 0f)
        {
            OnShoot?.Invoke();
            GameObject bullet = Instantiate(_projectile, _anchorFire.position, _anchorFire.rotation);
            bullet.GetComponent<Bullet>().StartLifeTime(timePressed, dir);
        }
    }
}
