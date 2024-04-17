using CustomPool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _radius = 40;
    [SerializeField] private int _objCount;
    [SerializeField] private int _enemyCount;
    [SerializeField] private GameObject[] _objs;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private ObjectPool _1CoinPool;
    [SerializeField] private ObjectPool _2CoinPool;
    [SerializeField] private ObjectPool _3CoinPool;
    [SerializeField] private ObjectPool _particlePool;
    [SerializeField] private ObjectPool _particlePoolSword;

    public void Spawn()
    {
        for (int i = 0; i < _objCount; i++)
            SpawnObj();
        for (int i = 0; i < _enemyCount; i++)
            SpawnEnemy();
    }

    public void SpawnObj()
    {
        Object obj = Instantiate(_objs[Random.Range(0, _objs.Length)],
                transform.position + new Vector3(Random.Range(-_radius, _radius), Random.Range(-_radius, _radius)), Quaternion.identity).GetComponent<Object>();
        obj.Spawner = this;
        switch (obj.coinID)
        {
            case 1:
                obj.coinPool = _1CoinPool; break;
            case 2:
                obj.coinPool = _2CoinPool; break;
            case 3:
                obj.coinPool = _3CoinPool; break;
        }
        obj.particlePool = _particlePool;
    }

    public void SpawnEnemy()
    {
        Enemy obj = Instantiate(_enemy,
                transform.position + new Vector3(Random.Range(-_radius, _radius), Random.Range(-_radius, _radius)), Quaternion.identity);
        obj.Spawner = this;
        obj.coinPool3 = _3CoinPool;
        obj.particlePool = _particlePoolSword;
    }
}
