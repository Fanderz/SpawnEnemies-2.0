using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private Target _currentTarget;
    [SerializeField] private List<Transform> _allTargets;

    private ObjectPool<Enemy> _pool;

    private Coroutine _coroutine;
    private bool _isRunning = false;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefab, transform),
            actionOnGet: (enemy) => SpawnEnemy(enemy),
            actionOnRelease: (enemy) => enemy.Deactivate(),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        SetTargetsForCurrentTarget();

        _isRunning = true;
        _coroutine = StartCoroutine(Spawning());
    }

    private void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void SpawnEnemy(Enemy enemy)
    {
        Vector3 startPosition = transform.position;
        enemy.transform.position = startPosition;
        enemy.Activate();
        enemy.SetTarget(_currentTarget);
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
        enemy.Deactivating -= OnReleaseEnemy;
    }

    private IEnumerator Spawning()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (_isRunning)
        {
            if (_pool.CountAll < _poolMaxSize || _pool.CountInactive > 0)
            {
                var enemy = _pool.Get();
                enemy.Deactivating += OnReleaseEnemy;
            }

            yield return wait;
        }
    }

    private void SetTargetsForCurrentTarget()
    {
        foreach (Transform target in _allTargets)
            _currentTarget.SetNextTargets(target.position);
    }
}
