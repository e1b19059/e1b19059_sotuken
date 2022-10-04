using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void JavaScriptAlert(string str);

    [DllImport("__Internal")]
    private static extern void doCode();

    [DllImport("__Internal")]
    private static extern void setTargetObject(string str);

    public TMP_InputField inputField;

    public void OnClick()
    {
        Debug.Log(inputField.text);
#if !UNITY_EDITOR && UNITY_WEBGL
        JavaScriptAlert(inputField.text);
#endif
    }

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

    public void SetInputFieldText(string text)
    {
        inputField.text = text;
    }

    public void DoCode()
    {
        Debug.Log("é¿çs");
#if !UNITY_EDITOR && UNITY_WEBGL
        setTargetObject("Circle");
        doCode();
#endif
    }

}