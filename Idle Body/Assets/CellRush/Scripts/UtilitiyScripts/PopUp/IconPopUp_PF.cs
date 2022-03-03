using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iconPopUp
{
    public class IconPopUp_PF : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 5f;
        [SerializeField] float _frequency = 1f;
        [SerializeField] float _magnitude = 1f;
        [SerializeField] float _lifetime = 1f;

        Vector3 _pos;
        float randomOffset;
        bool _active;

        private void Start()
        {
            _pos = transform.position;
        }

        public void InitializePopUp()
        {
            transform.localScale = Vector3.one;
            _pos = transform.position;
            _active = true;
            randomOffset = Random.Range(0f, 1f);
            LeanTween.cancel(gameObject);
            Invoke("LifetimeEnd", _lifetime);

        }
        private void Update()
        {
            if (!_active) return;
            _pos += transform.up * Time.deltaTime * _moveSpeed;
            transform.position = _pos + transform.right * Mathf.Sin((Time.time + randomOffset) * _frequency) * _magnitude;
           
        }

        void LifetimeEnd()
        {
            LeanTween.scale(gameObject, Vector3.zero, 1).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
            {
                _active = false;
                _pos = Vector3.zero;
                gameObject.SetActive(false);
            });

        }

    }
}
