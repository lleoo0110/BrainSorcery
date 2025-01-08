namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private int _poolSize = 10;

        public int _generatedObjectCount { get { return _parentTransform.childCount; } }
        private Queue<GameObject> _availableObjects = new Queue<GameObject>();

        private void Start()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                try
                {
                    GameObject obj = Instantiate(_objectPrefab, _parentTransform);
                    obj.SetActive(false);
                    _availableObjects.Enqueue(obj);
                }
                catch
                {
                    Debug.LogError("プレファブがアタッチされていません");
                }
            }
        }

        public GameObject GetObject(bool active)
        {
            var gam = _availableObjects.Dequeue();
            gam.SetActive(active);

            if (_availableObjects.Count == 0)
            {
                GameObject obj = Instantiate(_objectPrefab, _parentTransform);
                obj.SetActive(false);
                _availableObjects.Enqueue(obj);
            }

            return gam;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            if (_generatedObjectCount > _poolSize) Destroy(obj);
            else _availableObjects.Enqueue(obj);
        }
    }
}