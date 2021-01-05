using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberPopUp : MonoBehaviour
{
    [SerializeField] private Sprite DamageImage;
    [SerializeField] private Sprite PointImage;
    [SerializeField] private Sprite BuildImage;
    private TextMeshPro textMesh;
    private Image BGPopUp;


    // Start is called before the first frame update

    private void Awake()
    {
        textMesh = transform.GetComponentInChildren<TextMeshPro>();
        BGPopUp = transform.GetComponentInChildren<Image>();
    }


    public static NumberPopUp Create(Vector3 position, int NumberAmount, int PopUpType, GameObject parentObject)
    {
     
        GameObject numberPopUpTransform = Instantiate(Resources.Load("Prefabs/NumberPopUp")as GameObject);
        NumberPopUp numberPopUp = numberPopUpTransform.GetComponent<NumberPopUp>();
        numberPopUp.setup(NumberAmount, PopUpType);
       
        numberPopUp.transform.position = position;
        numberPopUp.transform.SetParent(parentObject.transform);



       Destroy(numberPopUpTransform, 1f);

        return numberPopUp;
    }

    public void setup (int numberAmount, int popUpType )
    {
        
        switch (popUpType)
        {
            case 1:
                textMesh.SetText("+" + numberAmount.ToString());
                BGPopUp.sprite = PointImage;
                textMesh.color = new Color(0.8f, 0.6f, 0.6f, 1);
                break;
            case 2:
                textMesh.SetText(numberAmount.ToString());
                BGPopUp.sprite = DamageImage;
                textMesh.color = new Color(1, 1, 1, 1);
                break;
            case 3:
                textMesh.SetText(numberAmount.ToString());
                BGPopUp.sprite = BuildImage;
                textMesh.color = new Color(.2f, .6f, .8f, 1);
                break;


        }
    }
}
