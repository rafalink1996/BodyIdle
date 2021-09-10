using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TopUI_Manager : MonoBehaviour
{
    [SerializeField] GameObject TransitionImage;
    [SerializeField] float TweenTimeTransition;
    [SerializeField] TextMeshProUGUI PointsText, PointsPerSecondText;
    [SerializeField] GameManager myGameManager;
    [SerializeField] NewPointsManager pointsManager;

    void Start()
    {
        myGameManager = GameManager.gameManager;
        if (myGameManager != null)
            pointsManager = myGameManager.pointsManager;


        Debug.Log("789000000000000 is " + AbbrevationUtility.AbbreviateNumber(789000000000));
    }

    private void Update()
    {
        PointsText.text = AbbrevationUtility.AbbreviateNumber(pointsManager.totalPoints);
        PointsPerSecondText.text = AbbrevationUtility.AbbreviateNumber(pointsManager.PointsPerSecond());
    }
    public static class AbbrevationUtility
    {
        private static readonly SortedDictionary<double, string> abbrevations = new SortedDictionary<double, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" },
         {1000000000000, "t" },
         {1000000000000000, "q" },
         {1000000000000000000, "Q" }
     };

        public static string AbbreviateNumber(double number)
        {
            for (int i = abbrevations.Count - 1; i >= 0; i--)
            {
                KeyValuePair<double, string> pair = abbrevations.ElementAt(i);
                if (number >= pair.Key)
                {
                    float roundedNumber = (float)(number / pair.Key);
                    return roundedNumber.ToString("F2") + " " + pair.Value;
                }
            }
            return number.ToString();
        }

    
    }

    public void TransitionIn()
    {
        EnableTransition();
        LeanTween.cancel(TransitionImage);
        LeanTween.scaleY(TransitionImage, 1, TweenTimeTransition);
        LeanTween.moveLocalY(TransitionImage, 0, TweenTimeTransition);

    }
    public void TransitionOut()
    {
        LeanTween.cancel(TransitionImage);
        LeanTween.moveLocalY(TransitionImage, -3413, TweenTimeTransition).setEase(LeanTweenType.easeInExpo);
        Invoke("EnableTransition", TweenTimeTransition);

    }

    void EnableTransition()
    {
        if (!TransitionImage.activeSelf)
        {
            TransitionImage.SetActive(true);
        }
        else
        {
            TransitionImage.SetActive(false);
        }
    }




}
