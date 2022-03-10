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
        _offlineHolder.sizeDelta = new Vector2(0, -_offlineHolder.rect.height);
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
    }



    public void CheckOfflineProgress(System.DateTime CurrenTime, System.DateTime LastTime)
    {
        //check offline time in seconds
       var utcCurrentTime = CurrenTime.ToUniversalTime();
        float offlineSecondsLimit = offlineHourLimit * 3600;
        var interval = utcCurrentTime - LastTime;
        Debug.Log("Timespan: " + interval);
        Debug.Log("Current Time: " + utcCurrentTime);
        Debug.Log("Last Time: " + LastTime);
        float timeOffline = (float)interval.TotalSeconds;
        if (timeOffline > offlineSecondsLimit) timeOffline = offlineSecondsLimit;
        if (timeOffline < 0) timeOffline = 0;
        System.TimeSpan offlineInterval = System.TimeSpan.FromSeconds(timeOffline);
        _offlineSeconds = offlineInterval.Seconds;
        _offlineMinutes = offlineInterval.Minutes;
        _offlineHours = offlineInterval.Hours;

        //multiply time in seconds by _energyPerSecond
        var EPS = CR_Data.data.GetEnergyPerSecond();
        _offlineEnergy = EPS * timeOffline;
        SetOfflineProduction();
        ShowOfflineProduction();
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
    void ShowOfflineProduction()
    {
        Debug.Log("Show OfflineProduction");
        _offlineHolder.gameObject.SetActive(true);
        LeanTween.size(_offlineHolder, Vector2.zero, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
        {
            _closeButton.gameObject.SetActive(true);
            _closeButton.transform.localScale = Vector3.zero;
            LeanTween.scale(_closeButton.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        });
    }

    public void OfflineProgressCheckError()
    {
        Debug.Log("There was an error loading offline progress");
    }

}
