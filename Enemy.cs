using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Vector3 _target;

    public event Action<Enemy> DeactivateEnemy;

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is BoxCollider || collision.collider is SphereCollider)
        {
            DeactivateEnemy?.Invoke(this);
        }
    }

    public void Activate() =>
        SetActivity(true);

    public void Deactivate() =>
        SetActivity(false);

    internal void SetTarget(Vector3 target)
    {
        _target = target;
        Rotate();
    }

    private void SetActivity(bool status) =>
        gameObject.SetActive(status);

    private void Move() =>
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

    private void Rotate() =>
        transform.rotation = Quaternion.LookRotation(_target - transform.position, Vector3.up);
}
