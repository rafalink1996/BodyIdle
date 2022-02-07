using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CR_OrganismView_Anim : MonoBehaviour
{

    [SerializeField] RectTransform BuyUI;
    [SerializeField] RectTransform SeeOrganButton;
    [SerializeField] RectTransform OrganBuy;


    [SerializeField] Vector2 BuyUiStartingPos;
    [SerializeField] Vector2 seeOrganButtonStartingPos;
    [SerializeField] Vector2 OrganBuyUiStartingPos;

    bool PosGotten;
    private void Start()
    {
        StartCoroutine(WaitForEndFarme());
    }
    IEnumerator WaitForEndFarme()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();
        //GetPos();
        //HideUI();
    }
    public void GetPos()
    {
        if (!PosGotten)
        {
            BuyUiStartingPos = BuyUI.localPosition;
            seeOrganButtonStartingPos = SeeOrganButton.localPosition;
        }
    }
    public void HideUI()
    {
        LeanTween.cancel(BuyUI.gameObject);
        LeanTween.moveLocalY(BuyUI.gameObject, BuyUiStartingPos.y - BuyUI.rect.height, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
        {

        });
    }

    public void ShowUI()
    {
        LeanTween.cancel(BuyUI.gameObject);
        LeanTween.moveLocalY(BuyUI.gameObject, BuyUiStartingPos.y, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
        {

        });
    }

    public void toggleSeeOrganObjects(bool show)
    {
        if (show)
        {
            SeeOrganButton.GetComponent<Button>().interactable = true;
            LeanTween.moveLocalX(OrganBuy.gameObject, OrganBuyUiStartingPos.x - 180, 0.5f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.moveLocalX(SeeOrganButton.gameObject, seeOrganButtonStartingPos.x, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
            {

            });
        }
        else
        {
            SeeOrganButton.GetComponent<Button>().interactable = false;
            LeanTween.moveLocalX(OrganBuy.gameObject, OrganBuyUiStartingPos.x, 0.5f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.moveLocalX(SeeOrganButton.gameObject, seeOrganButtonStartingPos.x - SeeOrganButton.rect.width, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
            {

            });
        }

    }

    float GetCanvasHeight()
    {
        float S_Width = Screen.width;
        float S_Height = Screen.height;

        float Ratio = 0;
        if (S_Height < S_Width)
        {
            Ratio = S_Width / S_Height;
        }
        else
        {
            Ratio = S_Height / S_Width;
        }
        return Ratio * 1080;

    }
}
