using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass 
{
    public static Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
