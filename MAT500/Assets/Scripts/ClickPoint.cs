using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickPoint : MonoBehaviour
{
    public DrawLine drawLine;
    //public GameObject clickPoint;
    private bool isComplete;
    public GameObject clickPointPrefab;
    public Transform pointParent;
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
          
            isComplete = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //clickPoint = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                GameObject go = GameObject.Instantiate(clickPointPrefab);
                go.transform.parent = pointParent;
                go.GetComponent<Transform>().position = Input.mousePosition;
                drawLine.clickPoints.Add(go.transform);
            }
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    isComplete = true;
        //}

    }
}
