using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderMove : MonoBehaviour
{
    float t;
    public int ID;
    public Text showText;
    private Functions drawManager;

    void Awake() {
        drawManager = GameObject.Find("Manager").gameObject.GetComponent<Functions>();
    }

    public void Move(float x)
    {
        t = (GetComponent<Slider>().value * 6.0f - 3.0f);
        showText.text = "value = " + t;
        drawManager.pointValues[ID] = t;
    }
}
