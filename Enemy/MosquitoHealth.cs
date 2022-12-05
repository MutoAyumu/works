using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// 蚊の体力、煙に当たった時の挙動
/// </summary>
public class MosquitoHealth : MonoBehaviour
{
    [Header("蚊の体力")]
    [SerializeField, Tooltip("蚊の体力")] int _health = 3;
    [Header("落下速度")]
    [SerializeField] float _fallingSpeed = 5f;

    VisualEffect _model;
    Transform _thisTransform;
    Vector3 _landingPosition;
    Rigidbody _rb;

    float _elapsed;

    const float _fallDuration = 2f;
    const float _rayDistance = 100f;
    const float _destroyDistance = 1f;

    public delegate void DestroyDelegate();
    public DestroyDelegate OnDestroy;

    bool _isDead;

    public bool IsDead { get => _isDead; set => _isDead = value; }

    private void Awake()
    {
        _model = GetComponent<VisualEffect>();
        _rb = GetComponent<Rigidbody>();
        _thisTransform = this.transform;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"蚊がダメージを受けた(受けたダメージ：{damage})");

        if (_health >= 0)
        {
            Debug.Log($"蚊が倒された");
            _isDead = true;
            _landingPosition = PositionCalculation();
            GameManager.Instance.AddScore(1);
        }
    }

    public void  Init()
    {
        _elapsed = 0;
    }

    public bool Falling()
    {
        //ここで落下の処理

        if(Vector3.Distance(_thisTransform.position, _landingPosition) < _destroyDistance)
        {
            return false;
        }

        _elapsed += Time.deltaTime;
        float speed = 0;

        if (_elapsed < _fallDuration)
        {
            speed = Mathf.Lerp(0, _fallingSpeed, _elapsed / _fallDuration);
        }

        var dir = _landingPosition - _thisTransform.position;
        dir.Normalize();
        _rb.velocity = dir * speed;

        return true;
    }

    Vector3 PositionCalculation()
    {
        RaycastHit info = default;
        var hit = Physics.Raycast(_thisTransform.position, Vector3.down, out info, _rayDistance);
        return hit ? info.point : _thisTransform.position;
    }
}
