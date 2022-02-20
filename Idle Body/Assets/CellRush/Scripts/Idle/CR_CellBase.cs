using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_CellBase : MonoBehaviour
    {
        #region Variables
        public enum CellSize { Small, Medium, Big };
        public enum CellType { Platlet, RedBlood, White, Helper, Killer };
        public CellSize cellSize;
        public CellType cellType;
        public int ID;



        //[SerializeField] protected CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo myInfo;
        [SerializeField] Sprite CombineSprite = null;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected ParticleSystem _particles;

        [SerializeField] protected bool _move;
        [SerializeField] protected float _speed = 2f;

        Rigidbody2D _rb;
        #endregion Variables

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        public virtual void InitializeCell(CellSize cellSize, CellType cellType)
        {
            Debug.Log("Initializing");
            this.cellSize = cellSize;
            this.cellType = cellType;
        }

        public virtual void StartCell()
        {
            if (!gameObject.activeSelf) return;
            _move = true;
            if (_particles != null) _particles.Play();
            Invoke("StopParticles", 0.5f);
            StartCoroutine(CellMovementStart());
        }

        void StopParticles()
        {
            if (_particles != null) _particles.Stop();
        }

        protected IEnumerator CellMovementStart()
        {
            while (_move)
            {
                yield return applyForce();
                yield return wait(0, 3);
            }
        }
        IEnumerator wait(float minTime, float maxTime)
        {
            float randomTime = Random.Range(minTime, maxTime);
            float timeTowait = Time.time + randomTime;
            while (timeTowait > Time.time)
            {
                yield return null;
            }
        }

        IEnumerator applyForce()
        {
            Vector2 force = CheckMovemtDirection();
            _rb.AddForce(force);
            while (Vector2.Distance(Vector2.zero, _rb.velocity) > 1)
            {
                yield return null;
            }

        }

        Vector2 CheckMovemtDirection()
        {
            float xForce = 0;
            float yForce = 0;
            switch (cellType)
            {
                case CellType.Platlet:
                    xForce = Random.Range(-1f, 1f);
                    yForce = Random.Range(-1f, 1f);
                    break;
                case CellType.RedBlood:
                    if (transform.localPosition.y < 0)
                    {
                        xForce = Random.Range(-1f, 1f);
                        yForce = Random.Range(0f, 1f);
                    }
                    else
                    {
                        xForce = Random.Range(-1f, 1f);
                        yForce = Random.Range(-1f, 1f);
                    }

                    break;
                case CellType.White:
                case CellType.Helper:
                    if (transform.localPosition.y > 0)
                    {
                        xForce = Random.Range(-1f, 1f);
                        yForce = Random.Range(-1f, 0f);
                    }
                    else
                    {
                        xForce = Random.Range(-1f, 1f);
                        yForce = Random.Range(-1f, 1f);
                    }
                    break;
                case CellType.Killer:
                    break;
                default:
                    break;
            }

            Vector2 force = new Vector2(xForce, yForce) * _speed;
            return force;
        }


        public virtual IEnumerator Merge(Transform target, float speedMultiplier = 1)
        {
            
            StopCoroutine("applyForce");
            StopCoroutine("wait");
            _move = false;
            _rb.velocity = Vector2.zero;

            while (Vector2.Distance(transform.position, target.position) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, (_speed * speedMultiplier) * Time.deltaTime);
                yield return null;
            }
            gameObject.SetActive(false);
        }





    }
}
