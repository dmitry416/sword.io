using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace CustomPool
{
    public class ObjectPool : MonoBehaviour
    {
        [Description("Если в пуле не хватает объектов, то автоматически пул расширится, иначе будет задействован активный объект (Рекомендую включать во избежании ошибок)")]
        [SerializeField] private bool _spawnIfEmpty = true;
        [Space]

        [Description("Количество объектов в пуле")]
        [SerializeField] [Min(0)] private int _poolSize = 0;

        [Description("Объект, который будет использоваться при инициализации пула")]
        [SerializeField] private GameObject _poolObject;

        private Queue<GameObject> _poolObjects = new Queue<GameObject>();

        private void Awake()
        {
            if (_poolSize > 0 && !_poolObject)
                throw new NullReferenceException("Нет ссылки на объект для инициализации пула");
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject obj = Instantiate(_poolObject);
                obj.SetActive(false);
                _poolObjects.Enqueue(obj);
            }
        }

        public void Spawn(GameObject original)
        {
            if (_poolObjects.TryPeek(out GameObject obj) && !(obj.activeSelf && _spawnIfEmpty))
            {
                obj = _poolObjects.Dequeue();
                obj.SetActive(true);
                _poolObjects.Enqueue(obj);
            }
            else
            {
                GameObject newObj = Instantiate(original);
                _poolObjects.Enqueue(newObj);
            }
        }

        public void Spawn(GameObject original, Transform parent)
        {
            if (_poolObjects.TryPeek(out GameObject obj) && !(obj.activeSelf && _spawnIfEmpty))
            {
                obj = _poolObjects.Dequeue();
                obj.transform.parent = parent;
                obj.SetActive(true);
                _poolObjects.Enqueue(obj);
            }
            else
            {
                GameObject newObj = Instantiate(original, parent);
                _poolObjects.Enqueue(newObj);
            }
        }

        public void Spawn(GameObject original, Vector3 position, Quaternion rotation)
        {
            if (_poolObjects.TryPeek(out GameObject obj) && !(obj.activeSelf && _spawnIfEmpty))
            {
                obj = _poolObjects.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                _poolObjects.Enqueue(obj);
            }
            else
            {
                GameObject newObj = Instantiate(original, position, rotation);
                _poolObjects.Enqueue(newObj);
            }
        }

        public void Spawn(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (_poolObjects.TryPeek(out GameObject obj) && !(obj.activeSelf && _spawnIfEmpty))
            {
                obj = _poolObjects.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.parent = parent;
                obj.SetActive(true);
                _poolObjects.Enqueue(obj);
            }
            else
            {
                GameObject newObj = Instantiate(original, position, rotation, parent);
                _poolObjects.Enqueue(newObj);
            }
        }

        public void Despawn(GameObject original) 
        { 
            original.SetActive(false);
        }

        public void Despawn(GameObject original, float time)
        {
            StartCoroutine(WaitBeforeDespawn(original, time));
        }

        IEnumerator WaitBeforeDespawn(GameObject original, float time)
        {
            yield return new WaitForSeconds(time);
            Despawn(original);
        }
    }
}