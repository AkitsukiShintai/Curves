using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InputD : MonoBehaviour
{
    public Draw drawManager;

    private void Start()
    {
        GetComponent<InputField>().text = "1";
    }
    public void InputDDD(string text)
    {
        int tempD = int.Parse(text);
        if (tempD <1)
        {
            //EditorUtility.DisplayDialog("Warning", "d must be more than 0", "ok", "ok");
            GetComponent<InputField>().text = "1";
            drawManager.D = 1;
            return;
        }

        if (tempD >20)
        {
            //UnityEditor.EditorUtility.DisplayDialog("Warning", "d must be less than 21", "ok", "ok");
            GetComponent<InputField>().text = "20";
            drawManager.D = 20;
            return;
        }
        drawManager.D = tempD;
    }

}
