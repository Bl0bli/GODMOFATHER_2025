using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    [Header("Paramètres atomiques")]
    [SerializeField] private float _MAXTimePressed = 3f;
    [SerializeField, Range(0.1f, 1f)] private float _moveSpeed = 5f;
    [SerializeField] private float _weaponRadius = 1;
    
    [Header("Event pour les fx")]
    public UnityEvent UnityOnHit; 
    public UnityEvent UnityOnShoot;
    public UnityEvent UnityOnCharge;
    
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private SpriteRenderer _renderer;
    
    private List<PowerUp> activeEffects = new List<PowerUp>();
    private float _timePressed = 0f;
    private Vector2 _aimDir;
    private Coroutine chargeShot;
    private int _playerID = 0;

    public float MoveSpeed
    {
        get{return _moveSpeed;}
        set{_moveSpeed = value;}
    }
    public SpriteRenderer Renderer
    {
        get{return _renderer;}
        set{_renderer = value;}
    }

    public Weapon Weapon => _weapon;
    public int PlayerID => _playerID;

    public bool BlockInputs;

    private void Start()
    {
        _playerID = GameManager.Instance.GetPlayerID(this);
    }

    #region Inputs
    public void HandleMovement(InputAction.CallbackContext context)
    {
        if (!BlockInputs)
        {
            //Debug.Log($"move: {context.ReadValue<Vector2>()}");
            Vector2 dir = context.ReadValue<Vector2>();
            rb.MovePosition((Vector2)transform.position + (context.ReadValue<Vector2>() * _moveSpeed));

        }
    }

    public void HandleAim(InputAction.CallbackContext context)
    {
        if (!BlockInputs)
        {
            _aimDir = context.ReadValue<Vector2>();

            if (_aimDir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;
            
                Vector3 offset = new Vector3(_aimDir.x, _aimDir.y, 0).normalized * _weaponRadius;
                _weapon.transform.position = transform.position + offset;
            
                _weapon.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }

            //Debug.Log($"aim: {context.ReadValue<Vector2>()}");
        }
    }

    public void HandleShoot(InputAction.CallbackContext context)
    {
        if (!BlockInputs)
        {
            if (context.performed)
            {
                chargeShot = StartCoroutine(ChargeShot());
            }
            else if (context.canceled)
            {
                if (chargeShot != null)
                {
                    StopCoroutine(chargeShot);
                    chargeShot = null;
                    UnityOnShoot?.Invoke();
                    _weapon.Fire(_timePressed, _aimDir);
                    _timePressed = 0f;
                }

                //Debug.Log("false");
            }
        }
    }
    IEnumerator ChargeShot()
    {
        while (_timePressed < _MAXTimePressed)
        {
            _timePressed += Time.deltaTime;
            UnityOnCharge?.Invoke();
            yield return null;
        }
    }

    #endregion
    
    #region Collisions
    public void Hit(Bullet bullet = null)
    {
        if(bullet != null)
        {
            UnityOnHit?.Invoke();
            bullet.Shooter.HandleBulletHit(bullet);
        }
    }

    public void HandleBulletHit(Bullet bullet)
    {
        if(bullet.Shooter.PlayerID != _playerID)
        _stats.AddScore(bullet.Score);
        else _stats.AddScore(-bullet.Score);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IInteractable interactable = other.collider.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Trigger(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Trigger(this);
        }
    }

    #endregion
    
    #region PowerUps
    
    public void AddPowerUp(PowerUp effect)
    {
        if (effect.Duration <= 0f)
        {
            effect.Apply(this);
            activeEffects.Add(effect);
        }
        else
        {
            StartCoroutine(HandlePowerUp(effect));
        }
    }

    IEnumerator HandlePowerUp(PowerUp effect)
    {
        effect.Apply(this);
        activeEffects.Add(effect);

        yield return new WaitForSeconds(effect.Duration);

        effect.Remove(this);
        activeEffects.Remove(effect);
    }
    
    #endregion
}
