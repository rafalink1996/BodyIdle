using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWorldObjectToScreenSize : MonoBehaviour
{
    [SerializeField] float ratio;
    [SerializeField] float hdRatio;
    void Start()
    {
        ratio = (float)Screen.height / Screen.width;
        hdRatio = (float)1920 / 1080;

        float WidthAdjustment = 1920 / ratio;
        float newWidth = (WidthAdjustment * transform.localScale.x) / 1080;
        transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);


       
    }


}
