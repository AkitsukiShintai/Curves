using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class Draw : MonoBehaviour
{
    const int ARRAYNUMBER = 22;
    // public Input inputD;
    public Material mat;
    public int typeOfAlg;
    //public Color color;
    private List<Transform> points;
    public float[] pointValues;
    float yLength;
    float xLength;
    const int FOR_COUNT = 100;
    List<Vector3> a = new List<Vector3>();
    bool isInit = false;
    private int d;
	private int[] PT;//B(d,i)list
    public int D {
        get { return d; }
       
        set{
            d = value;
            ResetPoints();
        }

    }
    
    public void AddNewPointAndReset(Transform point) {

        points.Add(point);
        for (int i = 0; i < points.Count; ++i)
        {
            if (points.Count == 1)
            {
                points[0].localPosition = new Vector3(-500, 100, 0);
                return;
            }
            int step = 1000 / (points.Count-1);
            points[i].localPosition = new Vector3(-500 + i * step,  100, 0);
            
        }
    }
    public void AddNewPoint(Transform point)
    {
        points.Add(point);
    }
    public void ResetPoints() {
        for (int i = 0; i < d + 1; i++)
        {
            PT[i] = B(d, i);
        }
        for (int i = 0; i < d+1; ++i)
        {
           
            int step = 1000 / (d);
            points[i].gameObject.SetActive(true);
            points[i].GetComponent<Point>().mSlider.gameObject.SetActive(true);
            points[i].localPosition = new Vector3(-500 + i * step, 100, 0);

            points[i].GetComponent<Point>().mSlider.value = 4.0f / 6.0f;

        }
        for (int i = d+1; i < points.Count; ++i)
        {
            points[i].GetComponent<Point>().mSlider.gameObject.SetActive(false);
            points[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Screen.SetResolution(1600, 1200, true);

        pointValues = new float[ARRAYNUMBER];
        typeOfAlg = 0;
        PT = new int[ARRAYNUMBER];
        points = new List<Transform>();
        yLength = Camera.main.orthographicSize;
        xLength = Camera.main.orthographicSize * Camera.main.aspect;
        for (int i = 0; i < ARRAYNUMBER-1; i++)
        {
            //GetComponent<CreateNewPoint>().Create(i);
        }
        d = 1;
        ResetPoints();

    }



    private float function(float x) {
        float y = 0;
        switch (typeOfAlg){         
            case 0: //BBF        
                //int d = points.Count - 1;
                for (int i = 0; i < d + 1; ++i)
                { 
                    y += pointValues[i] * PT[i] * Mathf.Pow((1 - x), d - i) * Mathf.Pow(x, i);
                }
                return y;
             
            case 1: //NLI
                //int d = points.Count - 1;
                float[] tempArray1 = new float[ARRAYNUMBER];
                float[] tempArray2 = new float[ARRAYNUMBER];
                for (int i = 0; i < ARRAYNUMBER; i++)
                {
                    tempArray1[i] = pointValues[i];
                }
                for (int i = 0; i < d; ++i)
                {
                    for (int j = 0; j < d -i; ++j)
                    {
                        tempArray2[j] = tempArray1[j] * (1-x) + tempArray1[j+1] * x;
                    }
                    for (int j = 0; j < d - i; j++)
                    {
                        tempArray1[j] = tempArray2[j];
                    }

                }

                return tempArray1[0];
          
            default:
                return 0;        
        }
       

        
    }



    private float func(int x)
    {
        float funcX = (x - (Screen.width / 2 - 500)) / 1000.0f;
        float funcY = function(funcX);
        return funcY;
    }
    


    void Update()
    {
     
    }




    void OnRenderObject()
    {
        a.Clear();
        if (points.Count < 2)
        {
            Debug.Log("points less than 2");
        }
        else
        {
            yLength = Camera.main.orthographicSize;
            xLength = Camera.main.orthographicSize * Camera.main.aspect;
            for (int i = 0; i < FOR_COUNT + 1; ++i)
            {
                int inFuncX = Screen.width / 2 - 500 + i * (1000 / (FOR_COUNT));
                float outFuncY = func(inFuncX);
                float x = (float)inFuncX / Screen.width * (xLength * 2) - xLength;
                Vector3 temp = new Vector3(x, outFuncY, 0);
                a.Add(temp);
            }
            isInit = true;
        }

        if (!isInit)
        {
            return;
        }
        GL.Clear(false,true,Camera.main.backgroundColor);
        GL.PushMatrix();
         mat.SetPass(0);
        //GL.wireframe = true;
        GL.Begin(GL.LINE_STRIP);
        GL.Color(mat.color);
        for (int i = 0; i < a.Count; ++i)
        {
            GL.Vertex3(a[i].x, a[i].y, 0);
        }

        GL.End();
        GL.PopMatrix();

    }


    private int B(int d, int i)
    {
        if (d<i)
        {
            return 0;
        }
        if (d == 1)
        {
            return 1;
        }
        else if (i == 0)
        {
            return 1;
        }

        return B(d - 1, i - 1) + B(d - 1, i);

    }

  
}