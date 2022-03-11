using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using UnityEngine.UI;
using TMPro;
using Idle;

public class CR_OfflineProgress : MonoBehaviour
{
    WorldTimeAPI _worldTimeAPI;
    [Range(8f, 12f)]
    [SerializeField] float offlineHourLimit = 8f;


    BigDouble _offlineEnergy;
    int _offlineSeconds;
    int _offlineMinutes;
    int _offlineHours;

    [Header("OFFLINE UI")]
    [SerializeField] RectTransform _offlineHolder;
    [SerializeField] Button _closeButton;
    [SerializeField] TextMeshProUGUI _workedHardText;
    [SerializeField] TextMeshProUGUI _hoursText;
    [SerializeField] TextMeshProUGUI _minutesText;
    [SerializeField] TextMeshProUGUI _secondsText;
    [SerializeField] TextMeshProUGUI _hibernationText;
    [SerializeField] TextMeshProUGUI _madeText;
    [SerializeField] TextMeshProUGUI _energyText;


    private void Awake()
    {
        if (_worldTimeAPI == null) FindObjectOfType<WorldTimeAPI>();
    }

    private void Start()
    {
        _closeButton.gameObject.SetActive(false);
        _closeButton.transform.localScale = Vector3.zero;
        _offlineHolder.localScale = Vector3.zero;
        _offlineHolder.gameObject.SetActive(false);
    }
    public IEnumerator CheckLoadFiles()
    {
        // GET UTC TIME

        bool waitForTime = false;
        float TimeTilFailure = 5f;
        bool timeFailed = false;
        var worldTimeS = WorldTimeAPI.Instance;
        System.DateTime CurrentTime = System.DateTime.Now;
        while (!waitForTime)
        {
            if (worldTimeS.IsTimeLodaed)
            {
                waitForTime = true;
                CurrentTime = worldTimeS.GetCurrentDateTime();
            }
            else
            {
                TimeTilFailure -= Time.deltaTime;
                if (TimeTilFailure <= 0)
                {
                    waitForTime = true;
                    timeFailed = true;
                }
            }
            yield return null;
        }


        // GET PROGRESS 
        if (!timeFailed)
        {
            CheckOfflineProgress(CurrentTime, CR_Data.data._lastSesionTime);
        }
        else
        {
            CR_Idle_Manager.instance.GameStart();
        }
    }



    public void CheckOfflineProgress(System.DateTime CurrenTime, System.DateTime LastTime)
    {
        //check offline time in seconds
        var utcCurrentTime = CurrenTime.ToUniversalTime();
        float offlineSecondsLimit = offlineHourLimit * 3600;
        var interval = utcCurrentTime - LastTime;
        float timeOffline = (float)interval.TotalSeconds;


        //Debug.Log("Current Time: " + utcCurrentTime);
        //Debug.Log("Last Time: " + LastTime);

        if (timeOffline > offlineSecondsLimit) timeOffline = offlineSecondsLimit;
        if (timeOffline < 0) timeOffline = 0;


        System.TimeSpan offlineInterval = System.TimeSpan.FromSeconds(timeOffline);
        _offlineSeconds = offlineInterval.Seconds;
        _offlineMinutes = offlineInterval.Minutes;
        _offlineHours = offlineInterval.Hours;

        //multiply time in seconds by _energyPerSecond
        var EPS = CR_Data.data.GetEnergyPerSecond();
        _offlineEnergy = EPS * timeOffline;

        if (timeOffline < 30)
        {
            CR_Data.data._offlineProgressCollected = true;
            if (timeOffline > 10)
            {
                CR_Data.data.SetEnergy(CR_Data.data._energy + _offlineEnergy);
            }
            if (!CR_Data.data.gameStarted)
            {
                CR_Idle_Manager.instance.GameStart();
            }
        }
        else
        {
            CR_Data.data._offlineProgressCollected = false;
            SetOfflineProduction();
            StartCoroutine(ShowOfflineProduction());
        }


    }
    void SetOfflineProduction()
    {

        var language = (int)CR_Data.data._language;
        _workedHardText.text = LanguageManager.instance.offlineProgressTexts.workedHard[language];
        _hoursText.text = _offlineHours + "H";
        _minutesText.text = _offlineMinutes + "M";
        _secondsText.text = _offlineSeconds + "S";
        _hibernationText.text = LanguageManager.instance.offlineProgressTexts.hibernation[language];
        _madeText.text = LanguageManager.instance.offlineProgressTexts.made[language];
        _energyText.text = AbbreviationUtility.AbbreviateBigDoubleNumber(_offlineEnergy);
    }
    IEnumerator ShowOfflineProduction()
    {
        yield return CR_Idle_Manager.instance._TransitionAnimation.TransitionIn();
        _offlineHolder.gameObject.SetActive(true);
        LeanTween.scale(_offlineHolder, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
        {
            _closeButton.gameObject.SetActive(true);
            LeanTween.scale(_closeButton.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        });
    }
    void HideOfflineProduction()
    {
        LeanTween.scale(_closeButton.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
        {
            _closeButton.gameObject.SetActive(false);
            LeanTween.scale(_offlineHolder, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
            {
                _offlineHolder.gameObject.SetActive(false);
                if (!CR_Data.data.gameStarted)
                {
                    CR_Idle_Manager.instance.GameStart();
                    CR_Data.data.gameStarted = true;
                }
                else
                {
                    StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.instance.gameState));
                }


            });
        });
    }

    public void OfflineProgressCheckError()
    {
        Debug.Log("There was an error loading offline progress");
        CR_Idle_Manager.instance.GameStart();
    }

    public void OnClickCloseOfflineProgress()
    {
        CR_Data.data.SetEnergy(CR_Data.data._energy + _offlineEnergy);
        CR_Data.data._offlineProgressCollected = true;
        HideOfflineProduction();
    }

}
