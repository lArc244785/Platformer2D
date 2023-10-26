using Platformer.FSM;

namespace Platfomer.Controllers
{
	public class SlugController : EnemyController
	{
		protected override void Start()
		{
			base.Start();

			machine = new EnemyMachine(this);
			var machineData = StateMachineDataSheet.GetSlugData(machine);
			machine.Init(machineData);
			OnHpMin += () => machine.ChangeState(CharacterStateID.Die);
			OnHpDepleted += (x) => machine.ChangeState(CharacterStateID.Hurt);
		}
	}
}