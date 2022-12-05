using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(MosquitoMove))]
[RequireComponent(typeof(MosquitoHealth))]
[RequireComponent(typeof(Rigidbody))]
public class MosquitoBase : MonoBehaviour, IObjectPool
{
    MosquitoMove _move;
    MosquitoHealth _health;
    Rigidbody _rb;
    Collider _col;
    VisualEffect _model;
    bool _isActive;

    public bool IsActive => _isActive;
    public MosquitoMove Move => _move;
    public MosquitoHealth Health => _health;

    private void Awake()
    {
        _move = GetComponent<MosquitoMove>();
        _health = GetComponent<MosquitoHealth>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<SphereCollider>();
        _model = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if(_health.IsDead)
        {
            var fall = _health.Falling();

            if(!fall)
            {
                Destroy();
            }
        }
        else
        {
            _move.Move();
        }
    }

    public void Create()
    {
        _rb.isKinematic = false;
        _col.enabled = true;
        _model.enabled = true;
        _health.IsDead = false;
        _isActive = true;
        _health.IsDead = false;
    }

    public void Destroy()
    {
        _rb.isKinematic = true;
        _col.enabled = false;
        _model.enabled = false;
        _isActive = false;
        _health.IsDead = true;
        _health.OnDestroy?.Invoke();
    }

    public void DisactiveForInstantiate()
    {
        _rb.isKinematic = true;
        _col.enabled = false;
        _model.enabled = false;
        _isActive = false;
        _health.IsDead = true;
    }
}
