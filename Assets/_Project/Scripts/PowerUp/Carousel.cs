using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Carousel : MonoBehaviour
{
    [SerializeField] private GameObject _imgPrefab;
    [SerializeField] private PowerUp[] _powerUps;
    [SerializeField] private RectTransform _viewport, _content;
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private bool _isLocked = false;
    
    [SerializeField] private float baseScrollSpeed = 500f;
    [SerializeField] private int minSpins = 3; 
    [SerializeField] private int maxSpins = 6; 

    [Header("Debug")]
    [SerializeField] private List<RectTransform> _itemList = new List<RectTransform>();
    
    private float _itemHeight;
    private int _totalItems;
    private Coroutine _spinRoutine;

#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:PowerUp");
        _powerUps = new PowerUp[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            _powerUps[i] = AssetDatabase.LoadAssetAtPath<PowerUp>(path);
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
            GameObject go = Instantiate(_imgPrefab);
            go.GetComponent<Image>().sprite = powerUp.Icon;
            go.transform.SetParent(_content);
            _itemList.Add(go.transform as RectTransform);
        }
    }
    
    [ContextMenu("TrySpin")]
    public void StartSpin()
    {
        if (_spinRoutine != null)
        {
            StopCoroutine(_spinRoutine);
            _spinRoutine = null;
        }
        _spinRoutine = StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        if (_itemList.Count == 0) yield break;
        
        int targetIndex = Random.Range(0, _itemList.Count);
        
        int totalSpins = Random.Range(minSpins, maxSpins);

        float speed = baseScrollSpeed;

        int currentIndex = 0;
        int spinsDone = 0;

        while (true)
        {
            foreach (RectTransform item in _itemList)
            {
                item.localPosition += Vector3.down * speed * Time.deltaTime;
            }
            
            for (int i = 0; i < _itemList.Count; i++)
            {
                RectTransform item = _itemList[i];
                if (item.localPosition.y < -_viewport.rect.height / 2f - _itemHeight / 2f)
                {
                    float maxY = float.MinValue;
                    foreach (RectTransform other in _itemList)
                        if (other.localPosition.y > maxY) maxY = other.localPosition.y;

                    item.localPosition = new Vector3(item.localPosition.x, maxY + _itemHeight, item.localPosition.z);
                    
                    currentIndex = (currentIndex + 1) % _totalItems;

                    if (currentIndex == 0)
                        spinsDone++;
                }
            }
            
            if (spinsDone >= totalSpins)
            {
                speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 2f);
                
                if (speed < 10f && currentIndex == targetIndex)
                {
                    Debug.Log($" PowerUp gagné: {_powerUps[targetIndex].name}");
                    yield break;
                }
            }

            yield return null;
        }
    }
    
}
