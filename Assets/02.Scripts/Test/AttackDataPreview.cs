using Platformer.Datum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDataPreview : MonoBehaviour
{
	public float direction;
	public SkillCastSetting setting;

	private void OnDrawGizmos()
	{
		if (!setting)
			return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.02f);

		Gizmos.color = Color.red;

		Vector2 origin = (Vector2)transform.position + new Vector2(setting.castCenter.x * direction, setting.castCenter.y);
		Vector2 size = setting.castSize;
		float distance = setting.castDistance;

		Vector2 lt = origin + new Vector2(-size.x / 2.0f * direction, +size.y / 2.0f);
		Vector2 rt = origin + new Vector2(+size.x / 2.0f * direction, +size.y / 2.0f);
		Vector2 lb = origin + new Vector2(-size.x / 2.0f * direction, -size.y / 2.0f);
		Vector2 rb = origin + new Vector2(+size.x / 2.0f * direction, -size.y / 2.0f);


		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(origin, Vector2.one * 0.01f);

		Gizmos.color = Color.green ;
		
		// L-T -> R-T
		Gizmos.DrawLine(lt ,
					    rt + Vector2.right * direction * distance);
		// L-B -> R-B
		Gizmos.DrawLine(lb ,
						rb + Vector2.right * direction * distance);
		
		// L-T -> L-B
		Gizmos.DrawLine(lt ,
						lb);
		
		// R-T -> R-B
		Gizmos.DrawLine(rt + Vector2.right * direction * distance,
						rb + Vector2.right * direction * distance);
	}
}
