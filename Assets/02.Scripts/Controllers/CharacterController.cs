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
		protected Rigidbody2D rigidbody;



		protected virtual void Awake()
		{
			rigidbody = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
            if (isMoveable)
            {
				move = new Vector2(horizontoal * _moveSpeed, 0.0f);
            }

			if(Mathf.Abs(horizontoal) > 0.0f)
				dircation = horizontoal < 0.0f ? DIRACTION_LEFT : DIRACTION_RIGHT;
        }

		protected virtual void FixedUpdate()
		{
			Move();
		}

		private void Move()
		{
			rigidbody.position += move * Time.fixedDeltaTime;
		}

	}

}
