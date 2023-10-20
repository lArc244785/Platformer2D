using UnityEngine;

namespace Platformer.FSM.Character
{
	class DoubleJump : CharacterStateBase
	{
		private float _jumpForce;

		public override bool canExecute => base.canExecute &&
			(machine.currentStateID == CharacterStateID.Jump || machine.currentStateID == CharacterStateID.Fall)&&
			!controller.hasDoubleJumped;

		public DoubleJump(CharacterMachine machine, float jumpForce) : base(machine)
		{
			_jumpForce = jumpForce;
		}

		public override CharacterStateID id => CharacterStateID.DoubleJump;


		public override void OnStateEnter()
		{
			base.OnStateEnter();

			controller.isDirectionChangeable = true;
			controller.isMoveable = false;

			controller.hasJumped = true;
			controller.hasDoubleJumped = true;

			animator.Play("Jump");
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
			rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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


