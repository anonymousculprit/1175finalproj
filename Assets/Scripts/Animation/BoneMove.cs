using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMove : MonoBehaviour
{
    public GameObject[] target;
    public float strength, timeMultiplier;
    float r = 0;

    void Update()
    {
        for (int i = 0; i < target.Length; i++)
        {
            r += Time.deltaTime * timeMultiplier;
            float x = Mathf.Cos(r) * 0.001f * strength;

            target[i].transform.position += new Vector3(x, 0, 0);
        }
    }

    void MoveVersion1()
    {
        float rotation = 0f;

        rotation += Time.deltaTime;

        Transform b1 = transform.GetChild(0);
        float rotationLim = 5f;

        do
        {
            b1.rotation = Quaternion.Euler(0, 0, 90 + rotationLim * Mathf.Cos(rotation));
            rotationLim += rotationLim * 0.5f;

            if (b1.childCount == 0) break;
            b1 = b1.GetChild(0);
        } while (true);
    }
}
