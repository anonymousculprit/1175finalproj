using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Behaviour Variables")]
    public float minTime;
    public float maxTime;
    public EnemyMovement[] behaviours;

    protected virtual IEnumerator EnemyBehaviour() { yield return null; }
    protected virtual float ControlEnemy(EnemyMovement state) { return 0f; }
    protected int LeftOrRight() => (int)Time.unscaledTime % 2 == 0 ? 1 : -1;
}
