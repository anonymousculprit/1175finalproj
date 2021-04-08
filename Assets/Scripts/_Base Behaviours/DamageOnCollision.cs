using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision
{
    PlayerController player;
    int dmg;

    public void OnInit(PlayerController _player, int _dmg)
    {
        player = _player;
        dmg = _dmg;
    }

    public void RunOnCollision(Collision2D col)
    {
        PlayerController p = col.gameObject.GetComponent<PlayerController>();

        if (p == null)
            return;

        p.Damage(dmg);
    }
}
