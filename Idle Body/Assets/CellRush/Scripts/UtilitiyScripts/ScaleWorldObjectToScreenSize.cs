using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWorldObjectToScreenSize : MonoBehaviour
{
    float _ratio;
    [SerializeField] bool _square;
    void Start()
    {
        _ratio = (float)Screen.height / Screen.width;
        float WidthAdjustment = 1920 / _ratio;
        float newWidth = (WidthAdjustment * transform.localScale.x) / 1080;
        if (_square)
        {
            transform.localScale = new Vector3(newWidth, newWidth, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);
        }
        


       
    }


}
