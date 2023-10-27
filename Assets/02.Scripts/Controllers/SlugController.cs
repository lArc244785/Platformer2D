using Platformer.FSM;

namespace Platformer.Controllers
{
	public class SlugController : EnemyController
	{
		protected override void Start()
		{
			base.Start();

			machine = new EnemyMachine(this);
			var machineData = StateMachineDataSheet.GetSlugData(machine);
			machine.Init(machineData);
			onHpMin += () => machine.ChangeState(CharacterStateID.Die);
			onHpDepleted += (x) => machine.ChangeState(CharacterStateID.Hurt);
		}
	}
}