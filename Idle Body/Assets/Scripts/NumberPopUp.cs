using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPopUp : MonoBehaviour
{
    private TextMeshPro textMesh;


    // Start is called before the first frame update

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static NumberPopUp Create(Vector3 position, int NumberAmount)
    {
        GameObject numberPopUpTransform = Instantiate(Resources.Load("Prefabs/NumberPopUp")as GameObject);
        NumberPopUp numberPopUp = numberPopUpTransform.GetComponent<NumberPopUp>();
        numberPopUp.setup(300);

        return numberPopUp;
    }

    public void setup (int numberAmount)
    {
        textMesh.SetText(numberAmount.ToString());
    }
}
