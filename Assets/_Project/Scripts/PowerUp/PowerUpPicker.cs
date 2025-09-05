using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpPicker : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp[] possibleEffects;
    [SerializeField] private float _timeToDisappear = .75f;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    private PowerUp chosenEffect;
    [SerializeField] SpriteRenderer sr;
    List<PowerUp> _powerUps;
    private Vector3 _defaultScale;
    Coroutine _disappearRoutine;
    private Coroutine _coolDownBeforeReAppear;
    private Coroutine _appearRoutine;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:PowerUp");
        possibleEffects = new PowerUp[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            possibleEffects[i] = AssetDatabase.LoadAssetAtPath<PowerUp>(path);
        }
    }
#endif
    private void Awake()
    {
        _powerUps = possibleEffects.ToList();
        _defaultScale = sr.gameObject.transform.parent.localScale;
    }

    private void Start()
    {
        if(_disappearRoutine == null)
            _disappearRoutine = StartCoroutine(DisapearRoutine());
    }

    public void Trigger(Player player = null)
    {
        if (player != null)
        {
            _collider.enabled = false;
            if (_powerUps != null && _powerUps.Count > 0)
            {
                chosenEffect = _powerUps[Random.Range(0, _powerUps.Count)];
                _powerUps.Remove(chosenEffect);
                //sr.sprite = chosenEffect.Icon;
            }
            player.AddPowerUp(chosenEffect);
            Debug.Log($"ADD POWER UP {chosenEffect.Name}");
            if(_disappearRoutine == null)
                _disappearRoutine = StartCoroutine(DisapearRoutine());
        }
    }

    void StartCooldown()
    {
        StopAllCoroutines();
        if (_disappearRoutine != null)
        {
            _disappearRoutine = null;
        }
        
        if(_coolDownBeforeReAppear == null)
            _coolDownBeforeReAppear = StartCoroutine(CoolDown());
    }

    void Appear()
    {
        StopAllCoroutines();
        if(_appearRoutine == null)
            _appearRoutine = StartCoroutine(AppearRoutine());
        
    }
    IEnumerator DisapearRoutine()
    {
        _animator.SetBool("idle", false);
        float elapsedTime = 0f;
        while (elapsedTime < _timeToDisappear)
        {
            elapsedTime += Time.deltaTime;
            sr.gameObject.transform.parent.localScale = Vector3.Lerp(_defaultScale, Vector3.zero, elapsedTime / _timeToDisappear);
            sr.transform.Rotate(0,0,720*Time.deltaTime*2);
            yield return null;
        }
        sr.transform.eulerAngles = new Vector3(0,0,0);
        _disappearRoutine = null;
        StartCooldown();
    }

    IEnumerator CoolDown()
    {
        float timer = Random.Range(8, 15);
        Debug.Log($"COOLDOWN {timer}");
        yield return new WaitForSeconds(timer);
        _coolDownBeforeReAppear = null; 
        Appear();
    }

    IEnumerator AppearRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _timeToDisappear)
        {
            elapsedTime += Time.deltaTime;
            sr.gameObject.transform.parent.localScale = Vector3.Lerp(Vector3.zero, _defaultScale, elapsedTime / _timeToDisappear);
            sr.transform.Rotate(0,0,720*Time.deltaTime*2);
            yield return null;
        }
        sr.transform.eulerAngles = new Vector3(0,0,0);
        _appearRoutine = null;
        _collider.enabled = true;
        _animator.SetBool("idle", true);
    }
}
