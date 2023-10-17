using UnityEngine;

namespace Platformer.Controllers
{
	public class PlayerController : CharacterController
	{
		public override float horizontoal => Input.GetAxisRaw("Horizontal");

		public override float vertical => Input.GetAxisRaw("Vertical");
	}
}