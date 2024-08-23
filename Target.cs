using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{ 
    [SerializeField] private float _speed;

    private List<Vector3> _nextTargets = new List<Vector3>();
    private int _nextTargetIndex = 0;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position != _nextTargets[_nextTargetIndex])
            transform.position = Vector3.MoveTowards(transform.position, _nextTargets[_nextTargetIndex], _speed * Time.deltaTime);
        else if (_nextTargetIndex != _nextTargets.Count - 1)
            _nextTargetIndex++;
        else
            _nextTargetIndex = 0;
    }

    internal void SetNextTargets(Vector3 nextTarget)
    {
        _nextTargets.Add(nextTarget);
    }
}
