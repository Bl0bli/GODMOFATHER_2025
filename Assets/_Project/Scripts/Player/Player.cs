using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Paramètres atomiques")]
    [SerializeField] private float _offsetRot;
    [SerializeField] private float _MAXTimePressed = 3f;

    [SerializeField] private float _MAXSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 15f;
    [SerializeField, Range(0.1f, 1f)] private float _moveSpeedMultiplier = 1f;
    [SerializeField, Range(0.1f, 5f)] private float _weaponRadius = 1;
    [SerializeField] bool _pushBack = false;
    [SerializeField, ShowIf("_pushBack")] private float _pushBackForce = 1f;
    [SerializeField] private float _invulnerabilityDuration = 1f;
    
    [Header("Event pour les fx")]
    public UnityEvent UnityOnHit; 
    public UnityEvent UnityOnShoot;
    public UnityEvent UnityOnCharge;
    
    [Header("References")]
    [SerializeField] private Carousel _carousel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform _weaponAnchor;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Slider _slider;
    
    private List<PowerUp> activeEffects = new List<PowerUp>();
    private float _timePressed = 0f;
    private Vector2 _aimDir;
    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Coroutine chargeShot;
    private Coroutine _invulnerabilityCooldown;
    private int _playerID = 0;

    public float MoveSpeed
    {
        get{return _moveSpeedMultiplier;}
        set{_moveSpeedMultiplier = value;}
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

    private void OnValidate()
    {
        if (_weapon == null) return;
        
        if (_aimDir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;
            _weapon.transform.rotation = Quaternion.Euler(0, 0, angle + _offsetRot);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = _moveInput.normalized * _MAXSpeed;
        
        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, (_moveInput.sqrMagnitude > 0.1f ? _acceleration : _deceleration) * Time.fixedDeltaTime);

        rb.linearVelocity = _currentVelocity * _moveSpeedMultiplier;
    }

    #region Inputs
    public void HandleMovement(InputAction.CallbackContext context)
    {
        if (!BlockInputs)
        {
            //Debug.Log($"move: {context.ReadValue<Vector2>()}");
            _moveInput = context.ReadValue<Vector2>();
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
                _weaponAnchor.transform.rotation = Quaternion.Euler(0, 0, angle + _offsetRot);
            }
        }
    }

    public void HandleShoot(InputAction.CallbackContext context)
    {
        if (!BlockInputs)
        {
            if (context.performed)
            {
                _slider.value = 0f;
                if(chargeShot != null) StopCoroutine(chargeShot);
                _timePressed = 0f;
                chargeShot = StartCoroutine(ChargeShot());
            }
            else if (context.canceled)
            {
                if (chargeShot != null)
                {
                    _slider.value = 0f;
                    StopCoroutine(chargeShot);
                    chargeShot = null;
                    UnityOnShoot?.Invoke();
                    if (_aimDir.sqrMagnitude < 0.01f) _aimDir = Vector2.right;
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
            _slider.value = Mathf.InverseLerp(0, _MAXTimePressed, _timePressed);
            UnityOnCharge?.Invoke();
            yield return null;
        }
    }

    #endregion
    
    #region Collisions
    public void Hit(Bullet bullet = null)
    {
        /*if(bullet != null && bullet.Shooter != null)
        {
            UnityOnHit?.Invoke();
            
            if (_pushBack && rb != null)
            {
                Vector2 dir = ((Vector2)transform.position - (Vector2)bullet.transform.position).normalized;
                rb.AddForce(dir * _pushBackForce, ForceMode2D.Impulse);
            }

            if (bullet.Shooter.PlayerID != _playerID)
            {
                bullet.Shooter.HandleScoreHit(bullet.Score);
                bullet.EndLifeTime();
            }
            else
            {
                bullet.Shooter.HandleScoreHit(- bullet.Score);
                bullet.EndLifeTime();
            }
        }*/
        if (bullet != null)
        {
            _stats.UpdateLife(-bullet.Score);
            bullet.EndLifeTime();
            if (_invulnerabilityCooldown != null)
            {
                StopCoroutine(_invulnerabilityCooldown);
                _invulnerabilityCooldown = null;
                _invulnerabilityCooldown = StartCoroutine(InvulnerabilityCooldown());
            }
        }
    }

    /*public void HandleScoreHit(int score)
    {
        _stats.AddScore(score);
        Debug.Log($"Player {_playerID} score: {_stats.Score}");
    }*/

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
    
    public void AddPowerUp(PowerUp effect) => _carousel.StartSpin(effect, this);

    public void ActivatePowerUp(PowerUp effect)
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

    IEnumerator InvulnerabilityCooldown()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(_invulnerabilityDuration);
        boxCollider.enabled = true;
    }
    
    #endregion
}
