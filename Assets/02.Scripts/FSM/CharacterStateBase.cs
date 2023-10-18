using Unity.VisualScripting;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;
namespace Platformer.FSM
{
	public enum CharacterStateID
	{
		None,
		Idle,
		Move,
		Jump,
		DownJump,
		DoubleJump,
		Fall,
		Land,
		Crounch,
		Hurt,
		Die,
		Attack,
		WallSlide,
		Edge,
		EdgeClimb,
		Ladder,
	}

	public class CharacterStateBase : IState<CharacterStateID>
	{
		public virtual CharacterStateID id { get;}

		public virtual bool canExecute => true;

		protected StateMachine<CharacterStateID> machine;
		protected Transform transfrom;
		protected Rigidbody2D rigidbody;
		protected Animator animator;
		protected CharacterController controller;

		public CharacterStateBase(StateMachine<CharacterStateID> machine)
		{
			this.machine = machine;
			transfrom = machine.owner.transform;
			rigidbody = machine.owner.GetComponent<Rigidbody2D>();
			animator = machine.owner.GetComponentInChildren<Animator>();
			controller = machine.owner;
		}

		public virtual void OnStateEnter()
		{
		}
		public virtual CharacterStateID OnStateUpdate()
		{
			return id;
		}
		public virtual void OnStateFixedUpdate()
		{
		}
		public virtual void OnStateExit()
		{
		}

	}


}
