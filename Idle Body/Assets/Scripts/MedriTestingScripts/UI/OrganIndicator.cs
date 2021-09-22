using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganIndicator : MonoBehaviour
{
    public int Pos;
    [SerializeField] Image myImage;
    public void setImage(Sprite sprite)
    {
        myImage.sprite = sprite;
    }
}
