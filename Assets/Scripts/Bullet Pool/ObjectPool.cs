using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject obj;
    public int poolSize;
    List<GameObject> objList = new List<GameObject>();

    private void Start()
    {
        InstantiateObjects();
    }

    public GameObject GetObject()
    {
        GameObject o = TryGetObject();

        if (o != null)
            return GetObject();
        else
            InstantiateObjects();
        return GetObject();
    }

    GameObject TryGetObject()
    {
        for (int i = 0; i < objList.Count; i++)
            if (!objList[i].activeInHierarchy)
                return objList[i];
        return null;
    }

    void InstantiateObjects()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject o = Instantiate(obj, transform);
            o.name = obj.name + " #" + i;
            objList.Add(o);
            o.SetActive(false);
        }
    }
}
