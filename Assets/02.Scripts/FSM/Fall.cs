namespace Platformer.FSM.Character
{
	class Fall : CharacterStateBase
	{
		private float _fallStartY;
		private float _landingDistance;
		public Fall(CharacterMachine machine, float landingDistance) : base(machine)
		{
			_landingDistance = landingDistance;
		}

		public override CharacterStateID id => CharacterStateID.Fall;
		public override bool canExecute => base.canExecute &&
					(machine.currentStateID == CharacterStateID.Idle ||
					 machine.currentStateID == CharacterStateID.Move ||
					 machine.currentStateID == CharacterStateID.Jump || 
					 machine.currentStateID == CharacterStateID.DoubleJump);

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isMoveable = false;
			controller.isDirectionChangeable = true;
 			animator.Play("Fall");
			_fallStartY = controller.transform.position.y;
		}

		public override CharacterStateID OnStateUpdate()
		{
			CharacterStateID nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				return id;

			if (controller.isGrounded)
			{
				var fallDistacne = _fallStartY - controller.transform.position.y;
				if (fallDistacne >= _landingDistance)
					nextID = CharacterStateID.Land;
				else
					nextID = CharacterStateID.Idle;
			}


			return nextID;
		}
	}
}


