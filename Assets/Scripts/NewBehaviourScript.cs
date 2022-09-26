using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;

public class NewBehaviourScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void JavaScriptAlert(string str);

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
        Debug.Log("実行");
        const string scriptText =
@"using UnityEngine;
Debug.Log(""doCode() が実行されました。"");
";

        try
        {
            var scriptOptions = ScriptOptions.Default.WithReferences(
                Assembly.Load("netstandard"),
                typeof(Debug).Assembly
            );

            CSharpScript.RunAsync(scriptText, scriptOptions).Wait();
        }
        catch (CompilationErrorException)
        {
            Debug.LogError("コンパイルエラー");
            throw;
        }
        catch
        {
            Debug.LogError("その他のエラー");
            throw;
        }
    }

}