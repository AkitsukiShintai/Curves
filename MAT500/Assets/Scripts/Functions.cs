using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{

    const int ARRAYNUMBER = 22;
    const int LINE_SEGMENT_COUNT = 50;

    //program 1
    public float[] pointValues;
    private int[] PT;//B(d,i)list
    private int d;

    //program 2
    public float tP; //t parameter, used for shell line
    //public List<Vector2> linePoints;
    //public List<Vector2> clickPoints;

    //program3
 
    void Awake()
    {
        pointValues = new float[ARRAYNUMBER];
        PT = new int[ARRAYNUMBER];
        d = 1;
        tP = 0.5f;
    }
    //public void in

    public int D
    {
        get { return d; }
        set
        {
            d = value;
            Recalculate_PT();
        }
    }


    private void Recalculate_PT()
    {
        for (int i = 0; i < d + 1; i++)
        {
            PT[i] = B(d, i);
        }
    }

    //program algorithm
    public void functionsOfProgram1(List<Vector3> linePoints, int typeOfAlg, Matrix4x4 scaleMat)
    {
        float y = 0;
        float t = 0.0f;
        linePoints.Clear();
        List<Vector3> a = new List<Vector3>();
        //for (int i = 0; i < clickPoints.Count; i++)
        //{
        //    a.Add(Camera.main.ScreenToWorldPoint(clickPoints[i].position));
        //}
        switch (typeOfAlg)
        {
            case 0: //BBF        
                //int d = points.Count - 1;
                for (int i = 0; i < LINE_SEGMENT_COUNT+1; ++i)
                {
                    for (int j = 0; j < d + 1; ++j)
                    {
                        y += pointValues[j] * PT[j] * Mathf.Pow((1 - t), d - j) * Mathf.Pow(t, j);
                    }
                    linePoints.Add(new Vector3(t, y, 0));
                    y = 0;
                    t += 1.0f / LINE_SEGMENT_COUNT;
                }

               
                break;
            case 1: //NLI
                //int d = points.Count - 1;
                for (int k = 0; k < LINE_SEGMENT_COUNT + 1; ++k)
                {
                    float[] tempArray1 = new float[ARRAYNUMBER];
                    float[] tempArray2 = new float[ARRAYNUMBER];
                    for (int i = 0; i < ARRAYNUMBER; i++)
                    {
                        tempArray1[i] = pointValues[i];
                    }
                    for (int i = 0; i < d; ++i)
                    {
                        for (int j = 0; j < d - i; ++j)
                        {
                            tempArray2[j] = tempArray1[j] * (1 - t) + tempArray1[j + 1] * t;
                        }
                        for (int j = 0; j < d - i; j++)
                        {
                            tempArray1[j] = tempArray2[j];
                        }
                    }
                    linePoints.Add(new Vector3(t, tempArray1[0], 0));
                    t += 1.0f / LINE_SEGMENT_COUNT;
                }
                break;         
            default:
                break;
        }

        for (int i = 0; i < linePoints.Count; ++i)
        {
            linePoints[i] = scaleMat * new Vector4(linePoints[i].x, linePoints[i].y, linePoints[i].z, 1);
        }

    }

    public void functionsOFProgram2(List<Transform> clickPoints, List<Vector3> linePoints, List<Vector3> shellPoints, int typeOfAlg)
    {
        float t = 0.0f;
        linePoints.Clear();
        shellPoints.Clear();
        List<Vector3> a = new List<Vector3>();
        for (int i = 0; i < clickPoints.Count; i++)
        {
            a.Add(Camera.main.ScreenToWorldPoint(clickPoints[i].position));
        }
        Vector3 y = new Vector3();
        if (clickPoints.Count < 2)
        {
            return;
        }
        switch (typeOfAlg)
        {
            case 0: //DC
                t = 0;

                for (int i = a.Count-1; i >=0 ; --i)
                {
                    shellPoints.Add(a[i]);
                }
               
                DCShell(shellPoints, a,tP);


                for (int i = 0; i < LINE_SEGMENT_COUNT+1; ++i)
                {
                    DC(linePoints, a, t);
                    t += 1.0f / LINE_SEGMENT_COUNT;
                }

                return;

            case 1: //BBF
                t = 0;
                D = clickPoints.Count - 1;
                for (int i = 0; i < LINE_SEGMENT_COUNT + 1; i++)
                {
                    for (int j = 0; j < d + 1; j++)
                    {
                        y += PT[j] * Mathf.Pow((1 - t), d - j) * Mathf.Pow(t, j) * a[j];
                    }
                    linePoints.Add(y);
                    y = new Vector3();
                    t += 1.0f / LINE_SEGMENT_COUNT;
                }
                return;

            case 2://mid point
                List<Vector3> up = new List<Vector3>();
                List<Vector3> down = new List<Vector3>();
               // List<Vector3> c = new List<Vector3>();
                while (a.Count < 1000) {
                    up.Clear();
                    down.Clear();
                    Midpoint(linePoints, a, up, down);
                    a.Clear();
                    List<Vector3> tee = a;
                    a = linePoints;
                    linePoints = tee;
                }
                for (int i = 0; i < a.Count; i++)
                {
                    linePoints.Add(a[i]);
                }

                return;

            default:
                return;
        }


    }

    private void DCShell(List<Vector3> shellPoints, List<Vector3> clickPoints, float t) {
        if (clickPoints.Count == 2)
        {
            return;
        }

        List<Vector3> tempClick = new List<Vector3>();
        for (int i = 0; i < clickPoints.Count-1; ++i)
        {
            Vector3 temp = t * clickPoints[i] + (1 - t) * clickPoints[i + 1];
            tempClick.Add(temp);
        }
        for (int i = 0; i < tempClick.Count; ++i)
        {
            shellPoints.Add(tempClick[i]);
        }
        tempClick.Reverse();
        DCShell(shellPoints, tempClick, 1-t);
    }

    private void DC( List<Vector3> linePoints, List<Vector3> clickPoints, float t)
    {
        if (clickPoints.Count ==1 )
        {
            linePoints.Add(clickPoints[0]);
            return;
        }
        List<Vector3> tempClick = new List<Vector3>();
        for (int i = 0; i < clickPoints.Count - 1; ++i)
        {
            Vector3 temp = t * clickPoints[i] + (1 - t) * clickPoints[i + 1];
            tempClick.Add(temp);
        }
        DC(linePoints,tempClick,t);
    }

    private void Midpoint(List<Vector3> linePoints, List<Vector3> clickPoints, List<Vector3> upPoints, List<Vector3> downPoints)
    {

        if (clickPoints.Count==1)
        {
            for (int i = 0; i < upPoints.Count; ++i)
            {
                linePoints.Add(upPoints[i]);
            }

            linePoints.Add(clickPoints[0]);

            for (int i = downPoints.Count - 1; i >=0; --i)
            {
                linePoints.Add(downPoints[i]);              
            }
            return;
        }

        List<Vector3> tempClick = new List<Vector3>();
        for (int i = 0; i < clickPoints.Count-1; ++i)
        {
            tempClick.Add((clickPoints[i + 1] + clickPoints[i]) / 2.0f);
        }
        upPoints.Add(clickPoints[0]);
        downPoints.Add(clickPoints[clickPoints.Count-1]);
        Midpoint(linePoints,tempClick,upPoints,downPoints);

    }

    public void functionsOFProgram3(List<Transform> clickPoints, List<Vector3> linePoints) {
        double t = 0.0;
        linePoints.Clear();
        if (clickPoints.Count<2)
        {
            return;
        }
        List<Vector3> a = new List<Vector3>();
        List<double> tArray = new List<double>();
        List<double> xList = new List<double>();
        List<double> yList = new List<double>();
        for (int i = 0; i < clickPoints.Count; i++)
        {
            a.Add(Camera.main.ScreenToWorldPoint(clickPoints[i].position));
            tArray.Add(i);
            xList.Add(a[i].x);
            yList.Add(a[i].y);
        }
       
        
        List<double> resultX = new List<double>();
        List<double> resultY = new List<double>();
        InterpolationPolynomial(tArray, xList, 1, resultX);
        InterpolationPolynomial(tArray, yList, 1, resultY);
        double D = tArray[tArray.Count-1];
        double step = D/1000.0f;
        for (int i = 0; i < 1000+1; ++i)
        {
            t = i * step;
            double y = 0.0f;
            double x = 0.0f;
            for (int j = 0; j < resultX.Count; ++j)
            {
                if (j == 0)
                {
                    x += resultX[0];
                    y += resultY[0];
                }
                else
                {
                    double tXti = 1.0f;
                    for (int k = 0; k < j; ++k)
                    {
                        tXti *= t - tArray[j - k-1];
                    }
                    x += resultX[j] * tXti;

                    y += resultY[j] * tXti;
                }
            }
            linePoints.Add(new Vector3( (float)x, (float)y,0.0f));
        }


    }

    private void InterpolationPolynomial(List<double> origin, List<double> current, int n, List<double> result) {
        if (n== origin.Count)
        {
            return;
        }
        if (n == 1)
        {
            result.Add(current[0]);
        }
        List<double> temp = new List<double>();
        for (int i = 0; i < current.Count-1; ++i)
        {
            double y = current[i + 1] - current[i];
            double x = origin[i + n] -origin[i];
            temp.Add(y/x);
        }
        result.Add(temp[0]);
        InterpolationPolynomial(origin, temp, n + 1, result);
    }

    private int B(int d, int i)
    {
        if (d < i)
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
