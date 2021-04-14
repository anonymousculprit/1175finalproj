using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnToPlayer
{
    PlayerController player;
    float range;
    float mvtForce;

    public void OnInit(PlayerController _player, float _range, float _mvtForce)
    {
        player = _player;
        range = _range;
        mvtForce = _mvtForce;
    }

    public void RunFixedUpdate(GameObject obj, ref Vector3 force)
    {
        float dist = Vector3.Distance(player.transform.position, obj.transform.position);
        if (dist <= range)
            force += Vector3.Normalize(player.transform.position - obj.transform.position) * mvtForce;
    }
}
