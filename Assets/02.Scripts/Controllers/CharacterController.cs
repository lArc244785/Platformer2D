using Platformer.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Controllers
{
	public abstract class CharacterController : MonoBehaviour
	{
		public const int DIRACTION_RIGHT = 1;
		public const int DIRACTION_LEFT = -1;

		public abstract float horizontoal { get; }
		public abstract float vertical { get; }

		public int dircation
		{
			get => _dircation;
			set
			{
				if (!isDirectionChangeable)
					return;
				if(value == _dircation) 
					return;

				if (value > 0)
					_dircation = DIRACTION_RIGHT;
				else if (value < 0)
					_dircation = DIRACTION_LEFT;
				else
					throw new System.Exception($"[CharacterController] : Worong direction {_dircation}");

				transform.localScale = new Vector3(1.0f * _dircation, 1.0f, 1.0f);
			}
		}
		private int _dircation;
		public bool isDirectionChangeable;
		
		public bool isMoveable;
		public Vector2 move;
		[SerializeField] private float _moveSpeed;
		protected Rigidbody2D rig2D;

		public bool isGrounded
		{
			get
			{
				ground = Physics2D.OverlapBox(
					rig2D.position + _groundDetectOffset, 
					_groundDetectSize, 
					0f, 
					_groundMask);

				return ground;
			}
		}

		[Header("Ground Detect"), SerializeField] private LayerMask _groundMask;
		[SerializeField] private Vector2 _groundDetectOffset;
		[SerializeField] private Vector2 _groundDetectSize;
		public Collider2D ground;
		[SerializeField] private float _groundBelowDetectDistance;


		public bool hasJumped = false;
		public bool hasDoubleJumped = false;
		protected CharacterMachine machine;


		protected virtual void Awake()
		{
			rig2D = GetComponent<Rigidbody2D>();
		}

		protected virtual void Update()
		{
			machine.UpdateState();

			if (isMoveable)
            {
				move = new Vector2(horizontoal * _moveSpeed, 0.0f);
            }

			if(Mathf.Abs(horizontoal) > 0.0f)
				dircation = horizontoal < 0.0f ? DIRACTION_LEFT : DIRACTION_RIGHT;
        }

		protected virtual void FixedUpdate()
		{
			machine.FixedUpdateState();

			Move();
		}

		protected virtual void LateUpdate()
		{
			machine.LateUpdateState();
		}

		private void Move()
		{
			rig2D.position += move * Time.fixedDeltaTime;
		}


		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset, _groundDetectSize);

			Vector3 castStartPos = transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y;
			RaycastHit2D hit =
			Physics2D.BoxCast(origin: castStartPos,
							  size: _groundDetectSize,
							  angle: 0.0f,
							  direction: Vector2.down,
							  distance: _groundBelowDetectDistance,
							  layerMask: _groundMask);


			if(!hit.collider)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireCube(castStartPos + Vector3.down * hit.distance, _groundDetectSize);
				Gizmos.DrawLine(castStartPos + Vector3.left * _groundDetectSize.x * 0.5f,
								castStartPos + Vector3.left * _groundDetectSize.x * 0.5f + Vector3.down * hit.distance);
			}

			Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y, _groundDetectSize);
			Gizmos.DrawWireCube(transform.position + (Vector3)_groundDetectOffset + Vector3.down * _groundDetectSize.y + Vector3.down * _groundBelowDetectDistance, _groundDetectSize);
		}

		public void Stop()
		{
			move = Vector2.zero;
			rig2D.velocity = Vector2.zero;
		}
	}

}
