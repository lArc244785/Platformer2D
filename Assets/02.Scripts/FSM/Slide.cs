using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Slide : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Slide;
        //Idle, Move, Jump, DownJump, DoubleJump, Fall
        public override bool canExecute => base.canExecute && 
            (machine.currentStateID == CharacterStateID.Idle ||
             machine.currentStateID == CharacterStateID.Move);


        private float _distance;
        private float _time;

        private Vector2 _startPosition;
        private Vector2 _targetPosition;


        private Vector2 _originalColliderOffset;
        private Vector2 _originalColliderSize;
        private Vector2 _crouchedColliderOffset;
        private Vector2 _crouchedColliderSize;

        // 기반타입이 생성자 오버로드를 가지면,
        // 하위타입에서 해당 오버로드에 인자를 전달할 수 있도록 파라미터들을 가지는 오버로드가 필요하다.
        // (최소 한개)
        public Slide(CharacterMachine machine , Vector2 crounchedCollideroffset, Vector2 crouchedColliderSize, float distance = 1.0f)
            : base(machine)
        {
            _distance = distance;
            _originalColliderOffset = trigger.offset;
            _originalColliderSize = trigger.size;
            _crouchedColliderOffset = crounchedCollideroffset;
            _crouchedColliderSize = crouchedColliderSize;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;

            controller.Stop();

            animator.Play("Slide");
            _startPosition = transform.position;
			_targetPosition = _startPosition + (Vector2.right * (controller.direction * _distance));

            trigger.offset = _crouchedColliderOffset;
            trigger.size = _crouchedColliderSize;

            collision.offset = _crouchedColliderOffset;
            collision.size = _crouchedColliderSize;

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
            trigger.offset = _originalColliderOffset;
            trigger.size = _originalColliderSize;

            collision.offset = _originalColliderOffset;
            collision.size = _originalColliderSize;
        }

	}
}
