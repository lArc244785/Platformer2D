using UnityEngine;

namespace Platfomer.Datum
{
	[CreateAssetMenu(fileName = "new SkillCastSetting", menuName = "Platformer/ScriptableObjects/SkillCaseSetting")]
	public class SkillCastSetting : ScriptableObject
	{
		public int targetMax;
		public LayerMask targetMask;
		public float damageGain;
		public Vector2 castCenter;
		public Vector2 castSize;
		public float castDistance;
	}
}

