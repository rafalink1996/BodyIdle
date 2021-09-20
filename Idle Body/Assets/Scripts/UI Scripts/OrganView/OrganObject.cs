using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganObject : MonoBehaviour
{
    [SerializeField] Organ myOrgan;
    [SerializeField] int ID;
    GameManager gameManager;
    OrganManager organManager;

    public void Start()
    {
        gameManager = GameManager.gameManager;
        if (gameManager != null)
            organManager = gameManager.organManager;
    }

    public void CustomStart()
    {

    }

    public void OnClickOrgan()
    {
        if (gameManager == null)
            gameManager = GameManager.gameManager;

        gameManager.playerInput.OnClickOrgan(ID);
    }


    public void BuyOrgan()
    {

    }

    public void ShowOrgan()
    {

    }
}
