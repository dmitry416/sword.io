using CustomPool;
using System;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Action onDestroy;
    public int coinID;
    [HideInInspector] public ObjectPool coinPool;
    [HideInInspector] public ObjectPool particlePool;
    [HideInInspector] public Spawner Spawner;
    [SerializeField] private GameObject _coin;
    [SerializeField] private int _coinsCount;
    [SerializeField] private GameObject _bang;

    private void Start()
    {
        onDestroy += Spawner.SpawnObj;
    }

    private void OnDisable()
    {
        onDestroy -= Spawner.SpawnObj;
    }

    public void DestroyThis()
    {
        onDestroy?.Invoke();
        for (int i = 0; i < _coinsCount; i++)
            coinPool.Spawn(_coin, transform.position, Quaternion.identity);
        particlePool.Spawn(_bang, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
