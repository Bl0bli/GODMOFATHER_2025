using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _MAXTimePressed = 3f;
    [SerializeField, Range(0.1f, 1f)] private float _moveSpeed = 5f;
    [SerializeField] private float _weaponRadius = 1;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Weapon _weapon;
    
    private float _timePressed = 0f;
    private Vector2 _aimDir;
    private Coroutine chargeShot;
    public void HandleMovement(InputAction.CallbackContext context)
    {
        //Debug.Log($"move: {context.ReadValue<Vector2>()}");
        Vector2 dir = context.ReadValue<Vector2>();
        rb.MovePosition((Vector2)transform.position + (context.ReadValue<Vector2>() * _moveSpeed));

    }

    public void HandleAim(InputAction.CallbackContext context)
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

    public void HandleShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            chargeShot = StartCoroutine(ChargeShot());
        }
        else if (context.canceled)
        {
            StopCoroutine(chargeShot);
            chargeShot = null;
            _weapon.Fire(_timePressed, _aimDir);
            _timePressed = 0f;
            //Debug.Log("false");
        }
    }

    IEnumerator ChargeShot()
    {
        while (_timePressed < _MAXTimePressed)
        {
            _timePressed += Time.deltaTime;
            yield return null;
        }
    }
}
