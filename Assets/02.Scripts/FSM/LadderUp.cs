using Platformer.GameElements;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class LadderUp : CharacterStateBase
    {
        public override bool canExecute => base.canExecute &&
            machine.currentStateID != CharacterStateID.LadderUp;

		public override CharacterStateID id => CharacterStateID.LadderUp;
        private float _speed;
  
		private Vector3 _renderOriginLocalPos;
        private Vector3 _renderLadderLocalPos = new Vector3(0.03f, 0.16f, 0f);

        private Ladder _ladder;

		public LadderUp(CharacterMachine machine, float speed = 0.3f)
            : base(machine)
        {
            _speed = speed;
            _renderOriginLocalPos = animator.transform.localPosition;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

			rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _ladder = controller.upLadder;
			animator.transform.localPosition = _renderLadderLocalPos;

            if (machine.prevStateID == CharacterStateID.LadderDown)
                return;

			controller.hasDoubleJumped = false;
            controller.isMovable = false;
            controller.isDirectionChangeable = false;
            controller.Stop();

            animator.Play("Ladder");


            if (controller.isGrounded)
                transform.position = _ladder.groundEnter;
            else if (transform.position.y < _ladder.upEnter.y)
                transform.position = _ladder.upEnter;
            else
                transform.position = new Vector2(_ladder.centerX, transform.position.y);


		}

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            animator.speed = controller.vertical > 0f ? 1.0f : 0.0f;

            if (!controller.isLadderUpDetected)
			{
                transform.position = _ladder.upExit;
               nextID = CharacterStateID.Idle;
			}

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
