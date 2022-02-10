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

        public virtual void InitializeCell(CellSize cellSize)
        {
            Debug.Log("Initializing");
            this.cellSize = cellSize;

        }

        public virtual void StartCell()
        {
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
            float xForce = Random.Range(-1f, 1f);
            float yForce = Random.Range(-1f, 1f);
            Vector2 force = new Vector2(xForce, yForce) * _speed;
            _rb.AddForce(force);
            while (Vector2.Distance(Vector2.zero, _rb.velocity) > 1)
            {
                yield return null;
            }

        }


        public virtual IEnumerator Merge(Transform target)
        {
            Debug.Log("Merge detected");
            StopCoroutine("applyForce");
            StopCoroutine("wait");
            _move = false;
            _rb.velocity = Vector2.zero;
            Debug.Log("Distance: " + Vector2.Distance(transform.position, target.position));
            while (Vector2.Distance(transform.position, target.position) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, (_speed/4) * Time.deltaTime);
                Debug.Log("Merging(" + "Current pos: " + transform.position + " Target Pos: " + target.position);
                yield return null;
            }
            gameObject.SetActive(false);
            Debug.Log("End Merge");
        }





    }
}
