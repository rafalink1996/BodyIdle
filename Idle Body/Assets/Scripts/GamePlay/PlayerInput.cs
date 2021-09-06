using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public void OnClickBuyCell()
    {
        GameManager.gameManager.organManager.cellSpawner.BuySmallRedBloodCell();
        
    }

}
