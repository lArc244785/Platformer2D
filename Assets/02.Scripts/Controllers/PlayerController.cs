using Platformer.FSM;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
	public class PlayerController : CharacterController
	{
		public override float horizontoal => Input.GetAxis("Horizontal");

		public override float vertical => Input.GetAxis("Vertical");

		private void Start()
		{
			machine = new PlayerMachine(this);
			var machineData = StateMachineDataSheet.GetPlayerData(machine);
			machine.Init(machineData);
		}

		protected override void Update()
		{
			base.Update();

			if (Input.GetKey(KeyCode.Space))
			{
				if(!machine.ChangeState(CharacterStateID.Jump)&&
					Input.GetKeyDown(KeyCode.Space))
				{
					machine.ChangeState(CharacterStateID.DoubleJump);
				}
			}
		}
	}
}