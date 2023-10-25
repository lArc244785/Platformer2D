using Platformer.GameElements;
using UnityEngine;

namespace Platformer.FSM.Character
{
	public class UpLadderClimb : CharacterStateBase
	{
		public override CharacterStateID id => CharacterStateID.UpLadderClimb;

		public override bool canExecute => base.canExecute &&
			(machine.currentStateID == CharacterStateID.Idle ||
			 machine.currentStateID == CharacterStateID.Move ||
			 machine.currentStateID == CharacterStateID.Dash ||
			 machine.currentStateID == CharacterStateID.Jump ||
			 machine.currentStateID == CharacterStateID.Fall ||
			 machine.currentStateID == CharacterStateID.DownJump ||
			 machine.currentStateID == CharacterStateID.DoubleJump) &&
			 controller.isLadderUpDetected;

		private Ladder _ladder;
		private float _vertical;
		private bool _doExit;
		public UpLadderClimb(CharacterMachine machine)
			: base(machine)
		{
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();

			controller.isDirectionChangeable = false;
			controller.isMovable = false;
			controller.hasJumped = false;
			controller.hasDoubleJumped = false;
			controller.Stop();
			rigidbody.bodyType = RigidbodyType2D.Kinematic;
			animator.Play("Ladder");
			animator.speed = 0.0f;

			_ladder = controller.upLadder;
			Vector2 startPos = transform.position.y > _ladder.upEnter.y ? new Vector2(_ladder.upEnter.x,transform.position.y) : _ladder.upEnter; 
			transform.position = startPos;
			_doExit = false;
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();

			if (nextID == CharacterStateID.None)
				return id;


			_vertical = controller.vertical;
			animator.speed =  Mathf.Abs(_vertical);

			controller.hasJumped = Mathf.Abs(controller.horizontal) == 0.0f;

			if (_doExit)
				nextID = CharacterStateID.Idle;


			return nextID;
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();
			transform.position += Vector3.up * _vertical * Time.fixedDeltaTime;
			
			if(transform.position.y >= _ladder.upExit.y)
			{
				transform.position = _ladder.top;
				_doExit = true;
			}
			else if(transform.position.y <= _ladder.downExit.y)
			{
				_doExit = true;
			}
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
			rigidbody.bodyType = RigidbodyType2D.Dynamic;
			animator.speed = 1.0f;

		}

	}
}
