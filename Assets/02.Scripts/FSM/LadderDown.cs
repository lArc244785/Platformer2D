using Platformer.GameElements;
using UnityEngine;

namespace Platformer.FSM.Character
{
	public class LadderDown : CharacterStateBase
	{
		public override CharacterStateID id => CharacterStateID.LadderDown;
		public override bool canExecute => base.canExecute &&
			//땅에 있는 경우 또는 이미 사다리를 타고 있는 경우
			(machine.currentStateID == CharacterStateID.Idle ||
			 machine.currentStateID == CharacterStateID.Move || 
			machine.currentStateID == CharacterStateID.Crouch ||
			machine.currentStateID == CharacterStateID.LadderUp);

		private float _speed;

		private Ladder _ladder;

		private Vector3 _renderOriginLocalPos;
		private Vector3 _renderLadderLocalPos = new Vector3(0.033f, 0.16f, 0f);

		public LadderDown(CharacterMachine machine, float speed = 0.3f)
			: base(machine)
		{
			_speed = speed;
			_renderOriginLocalPos = animator.transform.localPosition;
		}

		public override void OnStateEnter()
		{
			base.OnStateEnter();

			_ladder = controller.downLadder;
			rigidbody.bodyType = RigidbodyType2D.Kinematic;
			animator.transform.localPosition = _renderLadderLocalPos;

			if (machine.prevStateID == CharacterStateID.LadderUp)
				return;

			controller.hasDoubleJumped = false;
			controller.isMovable = false;
			controller.isDirectionChangeable = false;
			controller.Stop();
			animator.Play("Ladder");

			if (transform.position.y > _ladder.downEnter.y)
				transform.position = _ladder.downEnter;
			else
				transform.position = new Vector2(_ladder.centerX, transform.position.y);
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();

			if (nextID == CharacterStateID.None)
				return id;

			animator.speed = controller.vertical < 0f ? 1.0f : 0.0f;

			if (controller.isGrounded)
				nextID = CharacterStateID.Idle;
			else if (controller.ladderExitY <= _ladder.downExit.y)
			{
				transform.position = _ladder.downExit;
				nextID = CharacterStateID.Idle;
			}

			Debug.Log(controller.isLadderDownDetected);

			return nextID;
		}

		public override void OnStateFixedUpdate()
		{
			base.OnStateFixedUpdate();

			transform.position += Vector3.up * controller.vertical * _speed * Time.fixedDeltaTime;
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
			animator.speed = 1.0f;
			rigidbody.bodyType = RigidbodyType2D.Dynamic;
			animator.transform.localPosition = _renderOriginLocalPos;
		}
	}
}
