using UnityEngine;

namespace Platformer.FSM.Character
{
	class Jump : CharacterStateBase
	{
		private float _jumpForce;

		public override bool canExecute => base.canExecute &&
			(machine.currentStateID == CharacterStateID.Idle ||
			machine.currentStateID == CharacterStateID.Move) &&
			controller.isGrounded;

		public Jump(CharacterMachine machine, float jumpForce) : base(machine)
		{
			_jumpForce = jumpForce;
		}

		public override CharacterStateID id => CharacterStateID.Jump;


		public override void OnStateEnter()
		{
			base.OnStateEnter();
			animator.Play("Jump");
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
			rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
			controller.isDirectionChangeable = true;
			controller.isMoveable = false;
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (rigidbody.velocity.y <= 0.0f)
				nextID = CharacterStateID.Fall;

			return nextID;
		}
	}
}


