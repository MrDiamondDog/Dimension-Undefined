using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void OnLand();
    public static event OnLand onLand;

    public static void TriggerEvent(string e)
    {
        if (e == "onLand")
        {
            onLand();
        }
    }
}
