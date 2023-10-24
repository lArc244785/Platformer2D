using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Dash : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Dash;
        //Idle, Move, Jump, DownJump, DoubleJump, Fall
        public override bool canExecute => base.canExecute && 
            (machine.currentStateID == CharacterStateID.Idle ||
             machine.currentStateID == CharacterStateID.Move || 
             machine.currentStateID == CharacterStateID.Jump ||
             machine.currentStateID == CharacterStateID.DoubleJump ||
             machine.currentStateID == CharacterStateID.Fall) &&
            machine.currentStateID != CharacterStateID.Dash;


        private float _distance;
        private float _time;

        private Vector2 _startPosition;
        private Vector2 _targetPosition;

		// 기반타입이 생성자 오버로드를 가지면,
		// 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
		// (최소 한개)
		public Dash(CharacterMachine machine , float distance = 1.0f)
            : base(machine)
        {
            _distance = distance;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;

            controller.Stop();

            animator.Play("Dash");
            _startPosition = transform.position;
			_targetPosition = _startPosition + (Vector2.right * (controller.direction * _distance));


			rigidbody.bodyType = RigidbodyType2D.Kinematic;

        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();
            if (nextID == CharacterStateID.None)
                return id;


			_time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            _time = Mathf.Log10(10 + _time * 90) - 1;
            Vector2 expecfed= Vector2.Lerp(_startPosition, _targetPosition, _time);
            bool isValid = true; 
			//transform.position = Vector2.Lerp(_startPosition, _targetPosition, _time);

            //if (_time >= 1.0f || controller.isWallDetected)
            //    nextID = CharacterStateID.Idle;


            if(Physics2D.OverlapCapsule(
                (Vector2)expecfed + trigger.offset, 
                trigger.size, 
                trigger.direction,
                0.0f,
                1 << LayerMask.NameToLayer("Wall")))
            {
                _targetPosition = transform.position;
                isValid = false;
			}

            if (isValid)
                transform.position = expecfed;
            
            if (!isValid || _time >= 1.0f)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

		public override void OnStateExit()
		{
			base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}

	}
}
