using Platformer.FSM.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.FSM
{
    public static class StateMachineDataSheet
    {
        public static IDictionary<CharacterStateID, IState<CharacterStateID>> GetPlayerData(CharacterMachine machine)
        {
            return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
            {
                { CharacterStateID.Idle, new Idle(machine) },
                { CharacterStateID.Move, new Move(machine) },
                { CharacterStateID.Fall, new Fall(machine, 0.8f) },
                { CharacterStateID.Jump, new Jump(machine, 3.2f) },
                { CharacterStateID.DoubleJump, new DoubleJump(machine, 3.2f) },
                { CharacterStateID.DownJump, new DownJump(machine) },
                { CharacterStateID.Land, new Land(machine) },
                { CharacterStateID.Crouch, new Crouch(machine, new Vector2(0f,0.054f), new Vector2(0.1f,0.1f)) },
				{ CharacterStateID.WallSlide, new WallSlide(machine) },
                { CharacterStateID.Hurt, new Hurt(machine)},
                { CharacterStateID.Die, new Die(machine)},
                { CharacterStateID.Dash, new Dash(machine)},
                { CharacterStateID.LadderUp, new LadderUp(machine)},
                { CharacterStateID.LadderDown, new LadderDown(machine)},
			};
        }
    }
}
