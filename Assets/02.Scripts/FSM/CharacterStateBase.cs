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

	public abstract class CharacterStateBase : StateBase<CharacterStateID>
	{

		protected CharacterMachine machine;
		protected Transform transfrom;
		protected Rigidbody2D rigidbody;
		protected Animator animator;
		protected CharacterController controller;

		public CharacterStateBase(CharacterMachine machine) : base(machine)
		{
			this.machine = machine;
			transfrom = machine.owner.transform;
			rigidbody = machine.owner.GetComponent<Rigidbody2D>();
			animator = machine.owner.GetComponentInChildren<Animator>();
			controller = machine.owner;
		}
	}


}
