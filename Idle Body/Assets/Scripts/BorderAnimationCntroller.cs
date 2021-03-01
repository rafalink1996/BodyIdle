using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderAnimationCntroller : MonoBehaviour
{
    public void animationLeftAcrive()
    {
        GetComponent<Animator>().SetTrigger("IZ");
    }
}
