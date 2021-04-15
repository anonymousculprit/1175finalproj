using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    Animator anim;
    bool move, jump;

    public void OnInit(Animator _anim, bool _move = false, bool _jump = false)
    {
        anim = _anim;
        move = _move;
        jump = _jump;
    }

    public void RunUpdate_Player(GroundState state, float hInput)
    {
        switch (state)
        {
            case GroundState.AIR:
                if (!jump)
                    return;
                anim.SetBool("Jumping", true);
                break;
            case GroundState.GROUND:
                if (jump)
                    anim.SetBool("Jumping", false);
                if (!move)
                    return;

                if (hInput != 0)
                    anim.SetBool("Running", true);
                else
                    anim.SetBool("Running", false);
                break;
            default: break;
        }
    }
}
