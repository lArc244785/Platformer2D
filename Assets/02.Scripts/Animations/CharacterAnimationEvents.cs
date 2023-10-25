using System;
using UnityEngine;

namespace Platformer.Animations
{
	public class CharacterAnimationEvents : MonoBehaviour
	{
		public Action onHit;
		private void Hit()
		{
			onHit?.Invoke();
		}
	}

}

	
