using Platformer.Stats;
using UnityEngine;

namespace Platformer.GameElements
{
	public class ProjectileWithDamage : Projectile 
	{
		[HideInInspector] public float damage;

		protected override void OnHitTarget(RaycastHit2D hit)
		{
			base.OnHitTarget(hit);
			if(hit.collider.TryGetComponent<IHp>(out var hp))
			{
				hp.DepleteHp(owner, damage);
			}
				gameObject.SetActive(false);
		}

		protected override void OnHitBound(RaycastHit2D hit)
		{
			base.OnHitBound(hit);
			gameObject.SetActive(false);
			
		}

	}

}
