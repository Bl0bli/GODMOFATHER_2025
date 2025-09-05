    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class Bullet : MonoBehaviour, IInteractable
    {
        [SerializeField] ParticleSystem particleDestroy;
        [SerializeField, Range(1,10)] private float _MAXlifeTime = 5f;
        [SerializeField, Range(1, 10)] private float _MINlifeTime = 1f;
        [SerializeField, Range(1, 5)] private float _MAXSize = 5f;
        [SerializeField, Range(1, 100)] private int _MAXScore = 2;
        [SerializeField, Range(1f, 10f)] private float _MAXSpeed = 4f;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] PhysicsMaterial2D _bouncyMaterial, _defaultMaterial;

        public UnityEvent UnityOnDestroy;
        
        float _lifeTime = 0f;
        float _size = 0f;
        private int _score = 0;
        float _speed = 0f;
	    Player _shooter;
        private Coroutine _lifeTimeRoutine;
        private bool _isBouncy = false;
        private float _defaultSize = 0f;
        private float _initialLifeTime;
        
        public int Score => _score;
        public Player Shooter => _shooter;
        public SpriteRenderer Renderer => _renderer;

        public void InitShooter(Player shooter)
        {
            _defaultSize = _size;
            _shooter = shooter;
            SetBouncy(shooter.HasBouncyBullets);
        }
        
        public void SetBouncy(bool enabled)
        {
            _isBouncy = enabled;
            rb.sharedMaterial = enabled ? _bouncyMaterial : _defaultMaterial;
        }

        public void StartLifeTime(float timePressed, Vector2 direction)
        {
            _lifeTime = Mathf.Lerp(_MINlifeTime, _MAXlifeTime, timePressed);
            _initialLifeTime = _lifeTime;
            _speed = Mathf.Lerp(_speed, _MAXSpeed, timePressed);
            _size = Mathf.Lerp(_size, _MAXSize, timePressed);
            _score = _MAXScore;
            _defaultSize = _size;
            transform.localScale = new Vector3(_size, _size, _size);
            rb.AddForce(direction.normalized * _speed * 50, ForceMode2D.Impulse);
            _lifeTimeRoutine = StartCoroutine(LifeTimeRoutine());
        }

        IEnumerator LifeTimeRoutine()
        {
            while (_lifeTime > 0f)
            {
                float t = 1f - (_lifeTime / _initialLifeTime); // normalisé 0 → 1
                _size = Mathf.Lerp(_defaultSize, _defaultSize / 6f, t);
                transform.localScale = Vector3.one * _size;

                _lifeTime -= Time.deltaTime;
                yield return null;
            }
            EndLifeTime();
        }

        IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(particleDestroy.main.duration * .5f);
            Destroy(gameObject);
        }

        public void Trigger(Player player = null)
        {
            Debug.Log("HIT");
            if(player != null) player.Hit(this);
        }
        
        public void EndLifeTime()
        {
            rb.freezeRotation = true;
            _renderer.enabled = false;
            if (_lifeTimeRoutine != null)
            {
                StopCoroutine(_lifeTimeRoutine);
                _lifeTimeRoutine = null;
            }
            StartCoroutine(DestroyRoutine());
            UnityOnDestroy?.Invoke();
        }
    }
