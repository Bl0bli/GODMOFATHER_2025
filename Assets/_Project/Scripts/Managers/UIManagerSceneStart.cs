using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManagerSceneStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerSprites;
    private int index = 0;

    public UnityEvent UnityOnEnableSprite;
    
    public void EnablePlayerSprites()
    {
        UnityOnEnableSprite?.Invoke();
        _playerSprites[index].SetActive(true);
        if (index < 1)
        {
            index++;
        }
    }
}
