using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

namespace Idle
{
    public class CR_Options_UI : MonoBehaviour
    {
        [Header("GENERAL")]
        [SerializeField] CanvasGroup _optionsHolder;
        [SerializeField] Button _closeOptionsButton;
        [SerializeField] RectTransform _optionsBody;
        [Header("SLIDERS")]
        [SerializeField] Slider _musicSlider;
        [SerializeField] Slider _soundEffectsSlider;
        [Header("TOGGLE NOTIFICATIONS")]
        [SerializeField] Transform _toggleNotifications;
        [SerializeField] Transform _toggleOff;
        [SerializeField] Transform _toggleOn;
        [Header("SELECT LANGUAGE")]
        [SerializeField] Button _languageButton;
        [SerializeField] RectTransform _languageOptionsObject;
        [SerializeField] RectTransform _languageOptionsReference;
        [SerializeField] TextMeshProUGUI _languageText;
        [SerializeField] Image _languageIcon;


        [SerializeField] Sprite[] _languageIconSprites;
        [SerializeField] string[] _languageTexts = new string[] {"English", "Español" };
 
        [SerializeField] AudioMixer _mixer;
        [SerializeField] SetGridLayoutGroup _setGridLayoutGroup;


        bool _notificationsOn;
        bool _languageOptionsShown;

      

        private void Start()
        {
            _musicSlider.value = CR_Data.data._musicVolume;
            _soundEffectsSlider.value = CR_Data.data._SFXVolume;

            _languageOptionsObject.gameObject.SetActive(false);
            if (_notificationsOn)
            {
                _toggleNotifications.localPosition = _toggleOn.localPosition;
            }
            else
            {
                _toggleNotifications.localPosition = _toggleOff.localPosition;
            }
            _setGridLayoutGroup.Set();

        }
     

        public void SetMusic(float volume)
        {
            if (volume <= _musicSlider.minValue) volume = -80;
            _mixer.SetFloat("_musicVolume", volume);
            CR_Data.data.SetMusicVolume(volume);
        }
        public void SetSoundEffects(float volume)
        {
            if (volume <= _soundEffectsSlider.minValue) volume = -80;
            _mixer.SetFloat("_soundVolume", volume);
            CR_Data.data.SetSFXVolume(volume);
        }
        public void ToggleNotifications()
        {
            LeanTween.cancel(_toggleNotifications.gameObject);
            if (_notificationsOn)
            {
                _notificationsOn = false;
                CR_Data.data.SetNotifications(false);
                LeanTween.moveLocalX(_toggleNotifications.gameObject, _toggleOff.localPosition.x, 0.5f).setEase(LeanTweenType.easeOutExpo);
            }
            else
            {
                _notificationsOn = true;
                CR_Data.data.SetNotifications(true);
                LeanTween.moveLocalX(_toggleNotifications.gameObject, _toggleOn.localPosition.x, 0.5f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        public void OnClickSelectLanguage()
        {
            if (_languageOptionsShown) return;
            _languageOptionsShown = true;
            _languageOptionsObject.gameObject.SetActive(true);
            Debug.Log(_languageOptionsObject.rect.width);
            _languageOptionsObject.sizeDelta = new Vector2(-_languageOptionsReference.rect.width, 0);
            Vector2 toVector = new Vector2(0, 0);
            LeanTween.size(_languageOptionsObject, toVector, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }

        public void OnClickSelectNewLanguage(int languageID)
        {
            if (!_languageOptionsShown) return;
            Vector2 toVector = new Vector2(-_languageOptionsObject.rect.width, 0);
            LeanTween.size(_languageOptionsObject, toVector, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
            {
                _languageOptionsShown = false;
                _languageOptionsObject.gameObject.SetActive(false);
            });
            CR_Data.data.SetLanguage((CR_Data.Languages)languageID);
            _languageIcon.sprite = _languageIconSprites[languageID];
            _languageText.text = _languageTexts[languageID];
        }


        public void ShowOptionsMenu()
        {
            _optionsHolder.gameObject.SetActive(true);
            _optionsHolder.alpha = 0;
            _optionsBody.transform.localScale = Vector3.zero;
            _closeOptionsButton.transform.localScale = Vector3.zero;
            LeanTween.alphaCanvas(_optionsHolder, 1, 0.5f).setOnComplete(done =>
            {
                LeanTween.scale(_closeOptionsButton.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                {
                    LeanTween.scale(_optionsBody.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                    {

                    });
                });
            });
        }

        public void HideOptionsMenu()
        {
            LeanTween.scale(_optionsBody.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
            {
                LeanTween.scale(_closeOptionsButton.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
                {
                    LeanTween.alphaCanvas(_optionsHolder, 0, 0.5f).setOnComplete(done =>
                    {
                        _optionsHolder.gameObject.SetActive(false);
                    });
                });
            });
        }
    }
}
