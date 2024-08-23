using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Target _target;

    public event Action<Enemy> Deactivating;

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Target target)|| collision.collider.TryGetComponent(out Wall wall))
        {
            Deactivating?.Invoke(this);
        }
    }

    public void Activate() =>
        SetActivity(true);

    public void Deactivate() =>
        SetActivity(false);

    internal void SetTarget(Target target)
    {
        _target = target;
        Rotate();
    }

    private void SetActivity(bool status) =>
        gameObject.SetActive(status);

    private void Move() =>
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

    private void Rotate() =>
        transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position, Vector3.up);
}
