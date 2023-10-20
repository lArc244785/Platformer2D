using Platformer.FSM.Character;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Platformer.FSM
{
	public static class StateMachineDataSheet
	{
		public static IDictionary<CharacterStateID, IState<CharacterStateID>>
			GetPlayerData(CharacterMachine machine)
		{
			return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
			{
				{CharacterStateID.Idle, new Idle(machine)},
				{CharacterStateID.Move, new Move(machine)},
				{CharacterStateID.Fall, new Fall(machine, 1.0f)},
				{CharacterStateID.Jump, new Jump(machine, 3.7f)},
				{CharacterStateID.Land, new Land(machine)},
				{CharacterStateID.DoubleJump, new DoubleJump(machine, 2.7f)},
			};
		}
	}
}