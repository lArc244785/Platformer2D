using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Controllers;
using Platformer.Stats;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

public class MiniStatusUI : MonoBehaviour
{
	[SerializeField] private Slider _slider;

	private void Start()
	{
		var hp = transform.root.GetComponent<IHp>();


		_slider.minValue = hp.hpMin;
		_slider.maxValue = hp.hpMax;
		_slider.value = hp.hpValue;

		hp.onHpChanged += (value)=> _slider.value = value;

		if(hp is CharacterController)
		{
			Vector3 originScale = transform.localScale;
			((CharacterController)hp).onDirectionChanged += (value) =>
			{
				transform.localScale = value < 0 ? 
				new Vector3(-originScale.x, originScale.y, originScale.z) :
				new Vector3(+originScale.x, originScale.y, originScale.z);
			};
		}
	}
}
