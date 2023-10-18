using Platformer.FSM.Character;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Platformer.FSM
{
	public static class StateMachineDataSheet
	{
		public static IDictionary<CharacterStateID, IState<CharacterStateID>>
			GetPlayerData(StateMachine<CharacterStateID> machine)
		{
			return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
			{
				{CharacterStateID.Idle, new Idle(machine)},
				{CharacterStateID.Move, new Idle(machine)},
			};
		}
	}
}