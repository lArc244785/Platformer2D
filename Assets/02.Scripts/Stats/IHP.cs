using System;

namespace Platformer.Stats
{
	public interface IHP
	{
		bool invincible { get; set; }
		float hpValue { get; set; }
		float hpMax { get; }
		float hpMin { get; }

		public event Action<float> onHpChanged;
		public event Action<float> onHpRecovered;
		public event Action<float> onHpDepleted;
		public event Action onHpMax;
		public event Action onHpMin;

		public void RecoverHP(object subject, float amount);
		public void DepleteHp(object subject, float amount);
	}

}
