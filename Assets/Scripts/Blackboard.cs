using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Blackboard
{
    public static List<KeyTypes> keys = new List<KeyTypes>();


    public static bool HasKey(KeyTypes type)
    {
        for (int i = 0; i < keys.Count; i++)
            if (keys[i] == type)
                return true;
        return false;
    }

    public static void RemoveKey(KeyTypes type) => keys.Remove(type);
    public static void SetKey(KeyTypes type) => keys.Add(type);
}
