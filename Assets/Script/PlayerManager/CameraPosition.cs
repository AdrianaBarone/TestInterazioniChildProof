using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public string cameraPosition;

    public string GivePosition()
    {
        Debug.Log("posizione fixed: "+ cameraPosition);
        return cameraPosition;
    }
}