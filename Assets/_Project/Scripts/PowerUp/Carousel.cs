using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Carousel : MonoBehaviour
{
public UnityEvent UnityOnPowerUpSelected; 
[SerializeField] private float _pauseTime = 0.5f; 
[SerializeField] private GameObject _imgPrefab; 
[SerializeField] private PowerUp[] _powerUps; 
[SerializeField] private RectTransform _viewport, _content, _globalLayout; 
[SerializeField] private float _spinDuration = 3f; 
[SerializeField] private VerticalLayoutGroup _layoutGroup; 
[SerializeField] private float scrollSpeed = 100f; 
[SerializeField] private bool _isLocked = false; 
[SerializeField] private float _offset = 1f; 
[SerializeField] private float baseScrollSpeed = 500f; 
[Header("Debug")] 
[SerializeField] private List<RectTransform> _itemList = new List<RectTransform>();
private float _itemHeight; private int _totalItems; 
private Coroutine _spinRoutine; 
private RectTransform _currentTarget; 
#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:PowerUp"); 
        _powerUps = new PowerUp[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]); _powerUps[i] = AssetDatabase.LoadAssetAtPath<PowerUp>(path);
        }
    } 
#endif
    private void Start()
    {
        InitItems();
    }

    void InitItems()
    {
        foreach (PowerUp powerUp in _powerUps)
        {
            _globalLayout.localScale = Vector3.zero; 
            GameObject go = Instantiate(_imgPrefab, _content); 
            go.GetComponent<Image>().sprite = powerUp.Icon; 
            RectTransform rect = go.transform as RectTransform; 
            rect.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
            _itemList.Add(rect);
        } 
        _totalItems = _itemList.Count;
        if (_totalItems > 0)
        {
            _itemHeight = _itemList[0].rect.height + _layoutGroup.spacing;
        }
    }

    [ContextMenu("TrySpin")]
    public void StartSpin(PowerUp effect, Player player)
    {
        if (_spinRoutine != null)
        {
            StopCoroutine(_spinRoutine); _spinRoutine = null;
        } 
        _spinRoutine = StartCoroutine(SpinRoutine(effect, _spinDuration, player));
    } 
    IEnumerator SpinRoutine(PowerUp target, float spinDuration, Player player) 
    { 
        if (_itemList.Count == 0) yield break; 
        int targetIndex = System.Array.IndexOf(_powerUps, target); Debug.Log(targetIndex);
        if (targetIndex < 0)
        {
            Debug.LogError("❌ Target PowerUp non trouvé dans la liste !"); yield break;
        } 
        float speed = baseScrollSpeed; 
        float elapsed = 0f; 
        float targetY = -200f; // centre local
        RectTransform targetItem = _itemList[targetIndex]; 
        _currentTarget = targetItem; // 🔹 Animation d'apparition (scale 0 -> 1.5)
        float appearDuration = spinDuration * 0.25f; 
        float appearElapsed = 0f;
        while (appearElapsed < appearDuration)
        {
            float t = appearElapsed / appearDuration; 
            float scale = Mathf.Lerp(0f, 0.005f, t); 
            _globalLayout.localScale = Vector3.one * scale; 
            appearElapsed += Time.deltaTime; 
            yield return null;
        } 
        _globalLayout.localScale = Vector3.one * 0.005f; // 🔹 Phase de spin principal
        while (elapsed < spinDuration)
        {
            ScrollItems(ref speed); 
            elapsed += Time.deltaTime; 
            yield return null;
        } // 🔹 Décélération & alignement du target
        bool stopped = false;
        while (!stopped)
        {
            ScrollItems(ref speed); 
            speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 1.5f); 
            float distance = targetY - targetItem.localPosition.y;
            if (speed < 30f)
            {
                foreach (var item in _itemList) item.localPosition += new Vector3(0f, distance * 0.1f, 0f);
            }

            if (Mathf.Abs(distance) < 2f && speed < 5f)
            {
                float snapDelta = distance; 
                foreach (var item in _itemList) item.localPosition += new Vector3(0f, snapDelta, 0f); 
                Debug.Log($"🎉 PowerUp gagné : {target.name}"); 
                stopped = true;
            } 
            yield return null;
        } 
        UnityOnPowerUpSelected?.Invoke(); 
        player.ActivatePowerUp(target); // 🔹 Attendre 1 seconde avant de disparaitre
        yield return new WaitForSeconds(_pauseTime); // 🔹 Animation disparition (scale 1.5 -> 0)
        float disappearDuration = 0.5f; 
        float disappearElapsed = 0f;
        while (disappearElapsed < disappearDuration)
        {
            float t = disappearElapsed / disappearDuration; 
            float scale = Mathf.Lerp(0.005f, 0f, t); 
            _globalLayout.localScale = Vector3.one * scale; 
            disappearElapsed += Time.deltaTime; yield return null;
        }
        _globalLayout.localScale = Vector3.zero; 
    }

    private void ScrollItems(ref float speed)
    {
        foreach (RectTransform item in _itemList)
        {
            item.localPosition += Vector3.down * speed * Time.deltaTime;
        }

        for (int i = 0; i < _itemList.Count; i++)
        {
            RectTransform item = _itemList[i]; 
            if (item == _currentTarget) continue;
            if (item.localPosition.y < (-_viewport.rect.height / 2f - _itemHeight / 2f) + _offset)
            {
                float maxY = float.MinValue; 
                foreach (RectTransform other in _itemList) 
                    if (other.localPosition.y > maxY) maxY = other.localPosition.y; 
                item.localPosition = new Vector3(item.localPosition.x, maxY + _itemHeight, item.localPosition.z);
            }
        }
    }
}
