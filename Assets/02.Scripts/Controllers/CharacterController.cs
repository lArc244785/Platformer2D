using Platformer.FSM;
using Platformer.GameElements;
using Platformer.Stats;
using System;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
	public abstract class CharacterController : MonoBehaviour, IHP
    {
        public const int DIRECTION_RIGHT = 1;
        public const int DIRECTION_LEFT = -1;
        public int direction
        {
            get => _direction;
            set
            {
                if (isDirectionChangeable == false)
                    return;

                if (value == _direction)
                    return;

                if (value > 0)
                {
                    _direction = DIRECTION_RIGHT;
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (value < 0)
                {
                    _direction = DIRECTION_LEFT;
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                    throw new System.Exception("[CharacterController] : Wrong direction.");
            }
        }
        private int _direction;
        public bool isDirectionChangeable;

        public abstract float horizontal { get; }
        public abstract float vertical { get; }

        public bool isMovable;
        public Vector2 move;
        [SerializeField] private float _moveSpeed;
        public float moveSpeed => _moveSpeed;
        protected Rigidbody2D rigidbody;

		#region Ground Detection
		//Ground Detection
		public bool isGrounded
        {
            get
            {
                ground = Physics2D.OverlapBox(rigidbody.position + _groundDetectOffset,
                                              _groundDetectSize,
                                              0.0f,
                                              _groundMask);
                return ground;
            }
        }

        public bool isGroundBelowExist
        {
            get
            {
                Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * 0.01f;
                RaycastHit2D[] hits =
                    Physics2D.BoxCastAll(origin: castStartPos,
                                         size: _groundDetectSize,
                                         angle: 0.0f,
                                         direction: Vector2.down,
                                         distance: _groundBelowDetectDistance,
                                         layerMask: _groundMask);

                RaycastHit2D hit = default;
                if (hits.Length > 0)
                    hit = hits.FirstOrDefault(x => ground ?? x != ground);

                groundBelow = hit.collider;
                return groundBelow;
            }
        }

        public Collider2D ground;
        public Collider2D groundBelow;
        [SerializeField] private Vector2 _groundDetectOffset;
        [SerializeField] private Vector2 _groundDetectSize;
        [SerializeField] private float _groundBelowDetectDistance;

        [SerializeField] private LayerMask _groundMask;
		#endregion
		#region Wall Detection
		//Wall Detection
		public bool isWallDetected
        {
            get
            {
                RaycastHit2D topHit = Physics2D.Raycast(_wallTopCastCenter, Vector2.right * _direction, _wallDetectDistance, _wallMask);
                RaycastHit2D bottomHit = Physics2D.Raycast(_wallBottomCastCenter, Vector2.right * _direction, _wallDetectDistance, _wallMask);
				return topHit.collider && bottomHit.collider;
            }
        }

        private Vector2 _wallTopCastCenter =>
			rigidbody.position + Vector2.up * _col.size;
        private Vector2 _wallBottomCastCenter => rigidbody.position;


		[SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _wallDetectDistance;
        #endregion

        #region Ladder Detection
        public bool isLadderUpDetected
        {
            get
            {
                Collider2D col = Physics2D.OverlapCircle(ladderUpDetectCenterPosition,
                                                         _ladderDetectRadius,
                                                         _ladderMask);
                if (col)
                {
                    upLadder = col.GetComponent<Ladder>();
                    return true;
                }
                return false;
            }
        }
        public bool isLadderDownDetected
        {
            get
            {
                Collider2D col = Physics2D.OverlapCircle(ladderDownDetectCenterPosition,
                                                         _ladderDetectRadius,
                                                         _ladderMask);
                if (col)
                {
                    downLadder = col.GetComponent<Ladder>();
                    return true;
                }
                return false;
            }
        }
        public float ladderExitY => transform.position.y + _ladderExitOffsetY;
        private Vector2 ladderUpDetectCenterPosition => (Vector2)transform.position + Vector2.up * _ladderUpDetectionOffset;
        private Vector2 ladderDownDetectCenterPosition => (Vector2)transform.position + Vector2.up * _ladderDownDetectionOffset;
		public Ladder upLadder;
        public Ladder downLadder;

        [SerializeField] private float _ladderUpDetectionOffset;
        [SerializeField] private float _ladderDownDetectionOffset;
        [SerializeField] private float _ladderDetectRadius;
        [SerializeField] private LayerMask _ladderMask;

        [SerializeField] private float _ladderExitOffsetY;
        #endregion

		private CapsuleCollider2D _col;

        public bool hasJumped;
        public bool hasDoubleJumped;

        protected CharacterMachine machine;

		public float hpValue
        {
            get => _hp;
            set
            {
                if (value == _hp)
                    return;

                value = Mathf.Clamp(value, hpMin, hpMax);
                _hp = value;

                OnHpChanged?.Invoke(value);

                if (value == hpMax)
                    OnHpMax?.Invoke();
                else if(value == hpMin)
                    OnHpMin?.Invoke();

            }
        }

		public float hpMax => _hpMax;

		public float hpMin => 0.0f;

		public bool invincible { get => _invincible; set => _invincible = value; }

        private bool _invincible = false;
		private float _hp;
        [SerializeField] private float _hpMax;
		public event Action<float> OnHpChanged;
		public event Action<float> OnHpRecovered;
		public event Action<float> OnHpDepleted;
		public event Action OnHpMax;
		public event Action OnHpMin;

		public void Stop()
        {
            move = Vector2.zero; // 입력 0
            rigidbody.velocity = Vector2.zero; // 속도 0
        }

        protected virtual void Awake()
        {
            _hp = _hpMax;
			rigidbody = GetComponent<Rigidbody2D>();
			_col = GetComponent<CapsuleCollider2D>();
		}

        protected virtual void Update()
        {
            machine.UpdateState();

            if (isMovable)
            {
                move = new Vector2(horizontal * _moveSpeed, 0.0f);
            }

            if (Mathf.Abs(horizontal) > 0.0f)
            {
                direction = horizontal < 0.0f ? DIRECTION_LEFT : DIRECTION_RIGHT;
            }
        }

        protected virtual void LateUpdate()
        {
            machine.LateUpdateState();
        }

        protected virtual void FixedUpdate()
        {
            machine.FixedUpdateState();

            Move();
        }

        private void Move()
        {
            rigidbody.position += move * Time.fixedDeltaTime;
        }

        private void OnDrawGizmosSelected()
		{
			DrawGroundDetectGizmos();
			DrawGoundBelowDetectGizmos();
            DrawWallDetectGizmos();
            DrawLadderDetectGizmos();
            DrawLadderExitGizmos();
		}

		private void DrawLadderExitGizmos()
		{
            Gizmos.color = Color.red;
            Vector3 pos = transform.position;
            pos.y = ladderExitY;
            Gizmos.DrawSphere(pos, 0.008f);
		}

		private void DrawLadderDetectGizmos()
		{
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(ladderUpDetectCenterPosition, _ladderDetectRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(ladderDownDetectCenterPosition, _ladderDetectRadius);
		}

		private void DrawGoundBelowDetectGizmos()
		{
			Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * 0.01f;
			RaycastHit2D[] hits =
				Physics2D.BoxCastAll(origin: castStartPos,
									 size: _groundDetectSize,
									 angle: 0.0f,
									 direction: Vector2.down,
									 distance: _groundBelowDetectDistance,
									 layerMask: _groundMask);

			RaycastHit2D hit = default;
			if (hits.Length > 0)
				hit = hits.FirstOrDefault(x => ground ?? x != ground);


			Gizmos.color = Color.gray;
			Gizmos.DrawWireCube(castStartPos, _groundDetectSize);
			Gizmos.DrawWireCube(castStartPos + Vector3.down * _groundBelowDetectDistance, _groundDetectSize);
			Gizmos.DrawLine(castStartPos + Vector3.left * _groundDetectSize.x / 2.0f,
							castStartPos + Vector3.left * _groundDetectSize.x / 2.0f + Vector3.down * _groundBelowDetectDistance);
			Gizmos.DrawLine(castStartPos + Vector3.right * _groundDetectSize.x / 2.0f,
							castStartPos + Vector3.right * _groundDetectSize.x / 2.0f + Vector3.down * _groundBelowDetectDistance);

			if (hit.collider != null)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireCube(castStartPos, _groundDetectSize);
				Gizmos.DrawWireCube(castStartPos + Vector3.down * hit.distance, _groundDetectSize);
				Gizmos.DrawLine(castStartPos + Vector3.left * _groundDetectSize.x / 2.0f,
								castStartPos + Vector3.left * _groundDetectSize.x / 2.0f + Vector3.down * hit.distance);
				Gizmos.DrawLine(castStartPos + Vector3.right * _groundDetectSize.x / 2.0f,
								castStartPos + Vector3.right * _groundDetectSize.x / 2.0f + Vector3.down * hit.distance);
			}
		}

		private void DrawGroundDetectGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset, _groundDetectSize);
		}

        private void DrawWallDetectGizmos()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _direction = (int)Mathf.Sign(transform.localScale.x); 

			Gizmos.color = Color.red;
            Gizmos.DrawLine(_wallTopCastCenter, _wallTopCastCenter + Vector2.right * _wallDetectDistance * _direction);
            Gizmos.DrawLine(_wallBottomCastCenter, _wallBottomCastCenter + Vector2.right * _wallDetectDistance * _direction);
        }

		public void RecoverHP(object subject, float amount)
		{
            hpValue += amount;
            OnHpRecovered?.Invoke(amount);
		}

		public void DepleteHp(object subject, float amount)
		{
			hpValue -= amount;
            OnHpDepleted?.Invoke(amount);
        }

    }
}