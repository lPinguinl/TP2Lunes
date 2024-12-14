using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPrototype<T>
{
    private Queue<T> poolQueue;
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, Transform parent, int initialSize = 5)
    {
        this.prefab = prefab;
        this.parent = parent;
        poolQueue = new Queue<T>();

        // Inicialización del pool
        for (int i = 0; i < initialSize; i++)
        {
            T newObject = Object.Instantiate(prefab, parent);
            newObject.gameObject.SetActive(false);
            poolQueue.Enqueue(newObject);
        }
    }

    public T Get()
    {
        if (poolQueue.Count > 0)
        {
            T pooledObject = poolQueue.Dequeue();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
        else
        {
            // Clonar a partir del Prototype si no hay más disponibles
            T clonedObject = prefab.Clone();
            clonedObject.transform.SetParent(parent);
            clonedObject.gameObject.SetActive(true);
            return clonedObject;
        }
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}