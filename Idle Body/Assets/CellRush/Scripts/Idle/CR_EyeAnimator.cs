using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Idle
{
    public class CR_EyeAnimator : MonoBehaviour
    {
        [SerializeField] Vector2 OriginalPos;
        public bool Debugeye;
        RectTransform rectTransform;
        RectTransform pupilsRectTransofrm;
        Coroutine delay;

        public Transform Pupils;

        private void Awake()
        {
            gameObject.TryGetComponent(out RectTransform rectT);
            if (rectT != null)
            {
                rectTransform = rectT;
            }
            Pupils.gameObject.TryGetComponent(out RectTransform rectP);
            if (rectP != null)
            {
                pupilsRectTransofrm = rectP;
            }
        }
        private void Start()
        {

        }
        void OnEnable()
        {
            delay = StartCoroutine(waitDelay());
        }
        private void OnDisable()
        {
            rectTransform.localPosition = OriginalPos;
            pupilsRectTransofrm.localPosition = Vector3.zero;
            StopCoroutine(delay);
        }
        IEnumerator waitDelay()
        {
            yield return new WaitForEndOfFrame();
            OriginalPos = rectTransform.localPosition;
            RandomAnimation();

        }


        async void RandomAnimation()
        {
            await WaitForAnimation();
            int random = Random.Range(0, 6);
            Animation(random);
        }



        void Animation(int animation)
        {
            switch (animation)
            {
                case 0:
                case 1:
                    LeanTween.scaleY(gameObject, 0.2f, 0.2f).setEase(LeanTweenType.easeInExpo).setOnComplete(complete =>
                    {
                        LeanTween.scaleY(gameObject, 1f, 0.2f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
                        {
                            RandomAnimation();
                        });
                    });
                    break;
                case 2: // mira derecha

                    LeanTween.moveLocalX(gameObject, OriginalPos.x + 15, 0.5f).setEase(LeanTweenType.easeInOutExpo);
                    LeanTween.moveLocalX(Pupils.gameObject, 15, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                    {
                        LeanTween.moveLocalX(gameObject, OriginalPos.x, 0.5f).setEase(LeanTweenType.easeInOutExpo);
                        LeanTween.moveLocalX(Pupils.gameObject, 0, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                        {
                            RandomAnimation();
                        });
                    });


                    break;
                case 3: // mira izquierda
                    LeanTween.moveLocalX(gameObject, OriginalPos.x - 15, 0.5f).setEase(LeanTweenType.easeInOutExpo);
                    LeanTween.moveLocalX(Pupils.gameObject, -15, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                    {
                        LeanTween.moveLocalX(gameObject, OriginalPos.x, 0.5f).setEase(LeanTweenType.easeInOutExpo);
                        LeanTween.moveLocalX(Pupils.gameObject, 0, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                        {
                            RandomAnimation();
                        });
                    });
                    break;

                case 4:
                    LeanTween.scaleY(gameObject, 0.2f, 0.2f).setEase(LeanTweenType.easeInExpo).setOnComplete(complete =>
                    {
                        LeanTween.scaleY(gameObject, 1f, 0.2f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
                        {
                            LeanTween.scaleY(gameObject, 0.2f, 0.2f).setEase(LeanTweenType.easeInExpo).setOnComplete(complete =>
                            {
                                LeanTween.scaleY(gameObject, 1f, 0.2f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
                                {
                                    RandomAnimation();
                                });
                            });
                        });
                    });
                    break;

                case 5:
                    LeanTween.scaleY(gameObject, 0.5f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(complete =>
                    {
                        LeanTween.moveLocalX(Pupils.gameObject, -15, 0.5f).setEase(LeanTweenType.easeInOutExpo).setLoopOnce().setOnComplete(complete =>
                        {
                            LeanTween.moveLocalX(Pupils.gameObject, 15, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                            {
                                LeanTween.moveLocalX(Pupils.gameObject, 0, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(complete =>
                                {
                                    LeanTween.scaleY(gameObject, 1f, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
                                    {
                                        RandomAnimation();
                                    });
                                });
                            });

                        });
                    });
                    break;
                default:
                    RandomAnimation();
                    break;

            }
        }

        async Task WaitForAnimation()
        {
            float RandomTime = Random.Range(3, 6);
            var end = Time.time + RandomTime;
            while (Time.time < end)
            {
                await Task.Yield();
            }
        }
    }

}
