using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenScr : MonoBehaviour
{
    public Toggle checkbox;

    public void SetFullscreen()
    {
        if (checkbox.isOn)
        {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log("FullScreen is on");
        };       
    }
}
