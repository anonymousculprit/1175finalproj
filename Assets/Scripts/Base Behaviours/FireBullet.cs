using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet
{
    ObjectPool objPool;
    int maxBullets;
    float currCD, firingCD;

    public void OnInit(int _maxBullets, float _firingCD, ObjectPool _oPool)
    {
        objPool = _oPool;
        maxBullets = _maxBullets;
        firingCD = _firingCD;
    }

    public void RunUpdate(Vector3 spawnPosition)
    {
        if (currCD > 0)
            currCD -= Time.deltaTime;

        SpawnBullet(spawnPosition);
    }

    void SpawnBullet(Vector3 spawnPosition)
    {
        if (Input.GetButtonDown("Fire1") && currCD <= 0)
            if (objPool.GetActiveCount() < maxBullets)
            {
                currCD = firingCD;
                GameObject go = objPool.GetObject();
                go.transform.position = spawnPosition;
                go.SetActive(true);
            }
    }
}
