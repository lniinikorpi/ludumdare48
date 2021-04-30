using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region singelton
    public static ObjectPooler instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }
    }
    #endregion
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Pool pool)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No such tag exists");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }

        else
        {
            objectToSpawn = Instantiate(pool.prefab);
        }

        if (objectToSpawn)
        {
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation; 
        }

        return objectToSpawn;
    }

    public void DestroyObject(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No such tag exists");
            return;
        }

        poolDictionary[tag].Enqueue(obj);
        obj.SetActive(false);
    }
}
