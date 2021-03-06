﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static float PositionHorizontal(int pNumber)
    {
        float pH = 0;
        pH = Input.GetAxisRaw($"P{pNumber} Position Horizontal");
        return pH;
    }

    public static float PositionVertical(int pNumber)
    {
        float pV = 0;
        pV = Input.GetAxisRaw($"P{pNumber} Position Vertical");
        return pV;
    }

    public static float RotationHorizontal(int pNumber)
    {
        float rH = 0;
        rH = Input.GetAxisRaw($"P{pNumber} Rotation Horizontal");
        return rH;
    }

    public static float RotationVertical(int pNumber)
    {
        float rV = 0;
        rV = Input.GetAxisRaw($"P{pNumber} Rotation Vertical");
        return rV;
    }

    public static bool GetButtonDown(int pNumber, string buttonName)
    {
        bool returnVar = false;
        returnVar = (Input.GetButtonDown($"P{pNumber} {buttonName}")) ? true : false;

        return returnVar;
    }
}
