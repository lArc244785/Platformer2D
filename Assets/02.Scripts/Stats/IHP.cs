using System;

namespace Platformer.Stats
{
	public interface IHP
	{
		bool invincible { get; set; }
		float hpValue { get; set; }
		float hpMax { get; }
		float hpMin { get; }

		public event Action<float> OnHpChanged;
		public event Action<float> OnHpRecovered;
		public event Action<float> OnHpDepleted;
		public event Action OnHpMax;
		public event Action OnHpMin;

		public void RecoverHP(object subject, float amount);
		public void DepleteHp(object subject, float amount);
	}

}
