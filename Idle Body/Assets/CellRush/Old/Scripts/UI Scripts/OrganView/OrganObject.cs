using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganObject : MonoBehaviour
{
    [SerializeField] Organ myOrgan;
    [SerializeField] int ID;

    [SerializeField] Image OrganImage;
    [SerializeField] GameObject Eyes;
    [SerializeField] GameObject SecondLungEyes;

    GameManager gameManager;
    OrganManager organManager;

    public void Start()
    {
        gameManager = GameManager.gameManager;
        if (gameManager != null)
            organManager = gameManager.organManager;
    }

    public void CustomStart(Sprite OrganSprite, Vector2 eyePos, bool Lungs = false)
    {
        gameManager = GameManager.gameManager;
        if (gameManager != null)
            organManager = gameManager.organManager;

        UpdateOrgan(OrganSprite, eyePos, Lungs);

    }

    public void OnClickOrgan()
    {
        if (gameManager == null)
            gameManager = GameManager.gameManager;

        gameManager.playerInput.OnClickOrgan(ID);
    }


    public void OnLongClickOrgan()
    {
        Debug.Log("Show info of  " + organManager.organTypes[organManager.activeOrganType].Name + " number " + ID);
        gameManager.OrganViewUI.ShowOrganInfo(organManager.activeOrganType, ID);
    }

    void UpdateOrgan(Sprite OrganSprite, Vector2 eyePos, bool Lungs)
    {
        OrganImage.sprite = OrganSprite;
        Eyes.transform.localPosition = eyePos;
        if (Lungs)
        {
            SecondLungEyes.SetActive(true);
        }
        else
        {
            SecondLungEyes.SetActive(false);
        }
    }
}
