using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public void FocusCanvas(string p_focus)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        if (p_focus == "0")
        {
            WebGLInput.captureAllKeyboardInput = false;
        }
        else
        {
            WebGLInput.captureAllKeyboardInput = true;
        }
#endif
    }
}
