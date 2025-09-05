using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private List<Sprite> _playerMovingSprites = new List<Sprite>();
    private Sprite _spriteOuch;
    private Sprite _DefaultSprite;
    private Coroutine _movingRoutine, _ouchRoutine;
    private bool _ouch = false;
    public List<Sprite> PlayerMovingSprites
    {
        get{return _playerMovingSprites;}
        set{_playerMovingSprites = value;}
    }

    public Sprite SpriteOuch
    {
        get{return _spriteOuch;}
        set{_spriteOuch = value;}
    }

    public Sprite DefaultSprite
    {
        get{return _DefaultSprite;}
        set{_DefaultSprite = value;}
    }

    public void PlayMovingAnimation()
    {
        if (_movingRoutine == null && _ouch == false)
        {
            _movingRoutine = StartCoroutine(MovingRoutine());
        }
    }

    public void StopMovingAnimation()
    {
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
            _renderer.sprite = _DefaultSprite;
        }
    }

    public void PlayOuchAnimation()
    {
        if (_ouchRoutine == null)
        {
            if (_movingRoutine != null)
            {
                StopMovingAnimation();
                _ouch = true;
            }
            _ouchRoutine = StartCoroutine(OuchRoutine());
        }
    }

    IEnumerator MovingRoutine()
    {
        while (true)
        {
            for (int i = 0; i < _playerMovingSprites.Count; i++)
            {
                _renderer.sprite = _playerMovingSprites[i];
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator OuchRoutine()
    {
        _renderer.sprite = _spriteOuch;
        yield return new WaitForSeconds(0.75f);
        _renderer.sprite = _DefaultSprite;
        _ouchRoutine = null;
        _ouch = false;
    }
}
