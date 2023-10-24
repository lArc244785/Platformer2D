using Platformer.GameElements;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class LadderUp : CharacterStateBase
    {
		public override bool canExecute => base.canExecute &&
            !(machine.currentStateID == CharacterStateID.LadderUp ||
              machine.currentStateID == CharacterStateID.LadderDown);

		public override CharacterStateID id => CharacterStateID.LadderUp;
        private float _speed;
        private Ladder _ladder;
		private Vector3 _renderLocalPos = new Vector3(0.0799999982f, 0.209999993f, 0);
		public LadderUp(CharacterMachine machine, float speed = 0.3f)
            : base(machine)
        {
            _speed = speed;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
			controller.hasDoubleJumped = false;
            controller.isMovable = false;
            controller.isDirectionChangeable = false;
            controller.Stop();

            animator.Play("Ladder");

            _ladder = controller.upLadder;

			if (controller.isGrounded)
                transform.position = _ladder.groundEnter;
            else if (transform.position.y < _ladder.upEnter.y)
                transform.position = _ladder.upEnter;
            else
                transform.position = new Vector2(_ladder.centerX, transform.position.y);

			rigidbody.bodyType = RigidbodyType2D.Kinematic;

			animator.transform.localPosition = new Vector3(0.03f, 0.16f, 0f);
		}

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (Mathf.Abs(controller.vertical) > 0f)
                animator.speed = 1.0f;
            else
                animator.speed = 0.0f;

            if (controller.isGrounded)
                nextID = CharacterStateID.Idle;
            else if(transform.position.y > _ladder.upExit.y ||
                transform.position.y < _ladder.upEnter.y)
                nextID = CharacterStateID.Idle;

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
            animator.transform.localPosition = _renderLocalPos;
		}
	}
}
