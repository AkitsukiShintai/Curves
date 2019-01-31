using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawLine : MonoBehaviour
{

    public Dropdown programs;
    public List<Transform> clickPoints;
    public List<Vector3>linePoints;
    public Functions function;
    public Material mat;
    public Toggle curveLine;
    public GameObject clickDrawPoint;

    //program 1
    public GameObject program1Objects;
    public InputField dInput;
    public List<GameObject> program1Sliders;
    public Transform startPos;
    public Transform endPos;
    public Transform highPos;
    public GameObject program1SliderPrefab;
    public Transform sliderParent;
    public Matrix4x4 scalemat;



    //program 2
    public GameObject program2Objects;
    public Toggle shellLine;
    public List<Vector3> shellPoints;
    public GameObject render ;
    public Dropdown alg;
    public Slider tSlider;
    public Text tSliderText;

    //initialize data
    private void Start()
    {
        programs.onValueChanged.AddListener(ProgramSwitch);
        tSlider.onValueChanged.AddListener(Program2TValueChange);
        dInput.onEndEdit.AddListener(Program1DChange);
        //program1 init
        for (int i = 0; i < 21; ++i)
        {
            GameObject go = GameObject.Instantiate(program1SliderPrefab);
            go.transform.parent = sliderParent;
            go.GetComponent<SliderMove>().ID = i;
            program1Sliders.Add(go);
        }
        Program1DChange("1");

        Vector3 start = Camera.main.ScreenToWorldPoint(endPos.position);
        Vector3 high = Camera.main.ScreenToWorldPoint(highPos.position);
         scalemat = Matrix4x4.Translate(new Vector3(-start.x,0, 0)) * Matrix4x4.Scale(new Vector3( start.x*2, high.y /3, 1)) ;

        SetMainObjects(false);
        SetProgram2(false);
        SetProgram1(false);

    }


    //UI event
    void ProgramSwitch(int par) {
        switch (par)
        {
            case 0:
                SetMainObjects(false);
                SetProgram2(false);
                SetProgram1(false);
                break;

            case 1:
                
                SetMainObjects(true);
                SetProgram2(false);
                SetProgram1(true);
                Program1DChange("1");
                break;
            case 2:
                SetMainObjects(true);
                SetProgram1(false);

                SetProgram2(true);
                break;
            default:
                break;
        }

    }

    void Program1DChange(string ds) {
        int d = int.Parse(ds);
        //active sliders and reset position
        if (d<1)
        {
            d = 1;
            
        }
        else if (d>20)
        {
            d = 20;
            
        }
        //set Function d
        function.D = d;

        for (int i = program1Sliders.Count-1; i >= d + 1; --i)
        {
            program1Sliders[i].SetActive(false);
        }

        Vector3 step = (endPos.localPosition - startPos.localPosition) / d;

        for (int i = 0; i < d+1; ++i)
        {
            program1Sliders[i].SetActive(true);
            program1Sliders[i].transform.localPosition = startPos.localPosition + i * step;
            program1Sliders[i].GetComponent<Slider>().value = 4f / 6;

        }
        //reset position end

        dInput.text = d + "";

    }

    void Program2TValueChange(float val) {
        function.tP = val;
        tSliderText.text = "t = " + val;
    }


    /// <summary>
    /// update
    /// </summary>
    private void Update()
    {
        switch (programs.value){
            case 1:
                function.functionsOfProgram1(linePoints, alg.value,scalemat);
                break;
            case 2:
                function.functionsOFProgram2(clickPoints, linePoints, shellPoints, alg.value);
                break;
            default:
                break;
        }

    }

    void OnRenderObject() {
        if (programs.value == 1)
        {

        }
        else if(programs.value == 2)
        {
            if (clickPoints.Count < 2)
            {
                //Debug.Log("points less than 2");
                return;
            }
        }
      

        GL.Clear(false, true, Camera.main.backgroundColor);
        GL.PushMatrix();
        mat.SetPass(0);
        //GL.wireframe = true;
        GL.Color(mat.color);

        GL.Begin(GL.LINE_STRIP);
        if (curveLine.isOn)
        {
            for (int i = 0; i < linePoints.Count; ++i)
            {
                GL.Vertex3(linePoints[i].x, linePoints[i].y, 0);
            }
        }
        
        GL.End();
        GL.Begin(GL.LINE_STRIP);
        if (shellLine.isOn)
        {
            for (int i = 0; i < shellPoints.Count; ++i)
            {
                GL.Vertex3(shellPoints[i].x, shellPoints[i].y, 0);
            }
        }
        GL.End();
        GL.PopMatrix();

    }

    void SetMainObjects(bool state) {
        if (state == false)
        {
            alg.gameObject.SetActive(false);
            curveLine.gameObject.SetActive(false);
            return;
        }
        alg.gameObject.SetActive(true);
        curveLine.gameObject.SetActive(true);
    }


    void SetProgram1(bool state) {
        if (state == true)
        {
            program1Objects.SetActive(true);
            curveLine.isOn = true;
            //clickDrawPoint.SetActive(true);
            alg.options.Clear();
            alg.AddOptions(new List<string> { "BBF", "NIL" });
            return;
        }
        program1Objects.SetActive(false);
        program2Objects.SetActive(false);
        curveLine.isOn = true;
        shellLine.isOn = false;
        clickDrawPoint.SetActive(false);



    }



    void SetProgram2(bool state) {
        if (state == true)
        {
            program2Objects.SetActive(true);
            tSlider.value = 0.5f;
            curveLine.isOn = true;
            shellLine.isOn = true;
            clickDrawPoint.SetActive(true);
            alg.options.Clear();
            alg.AddOptions(new List<string> { "DC", "Be", "Midpoint" });
            Program1DChange("1");
            return;
        }
        for (int i = 0; i < clickPoints.Count; i++)
        {
            Destroy(clickPoints[i].gameObject);
        }
        clickPoints.Clear();
        tSlider.value = 0.5f;
        shellLine.isOn = false;
        program2Objects.SetActive(false);
        clickDrawPoint.SetActive(false);

    }


}
