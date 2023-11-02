using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameElements
{
	public class Projectile : MonoBehaviour
	{
		[HideInInspector]public Transform owner;
		[HideInInspector] public Vector3 velocity;
		[HideInInspector] public LayerMask targetMask;
		private LayerMask _boundMask;

		private void Start()
		{
			_boundMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ground");
		}


		private void FixedUpdate()
		{
			Vector3 expected = transform.position + velocity * Time.fixedDeltaTime;

			RaycastHit2D hit = Physics2D.Raycast(transform.position,
												 expected - transform.position, Vector3.Distance(transform.position, expected),
												_boundMask | targetMask);

			if(hit.collider)
			{
				int layerFlag = 1 << hit.collider.gameObject.layer;
				if ((_boundMask & layerFlag) > 0)
					OnHitBound(hit);
				else if ((targetMask & layerFlag) > 0)
					OnHitTarget(hit);
			}

			transform.position += velocity * Time.fixedDeltaTime;
		}
		protected virtual void OnHitBound(RaycastHit2D hit)
		{
			DestoryParticle(hit);
		}

		private void DestoryParticle(RaycastHit2D hit)
		{
			var ps = PoolExample.Instance.Pool.Get();
			ps.transform.position = transform.position;
			Vector2 direction = hit.point - (Vector2)owner.position;
			ps.transform.rotation = direction.x > 0 ? Quaternion.EulerAngles(0f, -90f, 0f) : Quaternion.EulerAngles(0f, 90f, 0f);
		}

		protected virtual void OnHitTarget(RaycastHit2D hit) { DestoryParticle(hit); }
	}



}

