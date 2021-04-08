using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite 
{
    SpriteRenderer sprite;

    public void OnInit(SpriteRenderer _sprite)
    {
        sprite = _sprite;
    }

    public void RunUpdate(float flipState, ref int flip)
    {
        if (flipState < 0)
            sprite.flipX = false;
        if (flipState > 0)
            sprite.flipX = true;

        if (flipState != 0)
            flip = (int)flipState;
    }
}
