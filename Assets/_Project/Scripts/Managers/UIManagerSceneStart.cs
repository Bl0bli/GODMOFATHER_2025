using System.Collections.Generic;
using UnityEngine;

public class UIManagerSceneStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerSprites;
    private int index = 0;
    
    public void EnablePlayerSprites()
    {
        _playerSprites[index].SetActive(true);
        if (index < 1)
        {
            index++;
        }
    }
}
