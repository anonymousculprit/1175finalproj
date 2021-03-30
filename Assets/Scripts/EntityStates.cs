using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovement
{
    NULL,
    MOVE,
    JUMP,
    MOVEJUMP
}

public enum GroundState
{
    NULL,
    GROUND,
    AIR
}

public enum GrabState
{
    NULL,
    GRAB
}

public enum CollectableTypes
{
    NULL,
    ENERGY,
    LIFE,
    KEY,
}

public enum KeyTypes
{
    KEY1,
    KEY2
}