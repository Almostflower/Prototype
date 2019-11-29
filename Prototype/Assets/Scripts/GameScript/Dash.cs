using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : BaseMonoBehaviour
{
	/// <summary>
	/// プレイヤー
	/// </summary>
	[SerializeField]
	private Player player_;

	/// <summary>
	/// 画像
	/// </summary>
	[SerializeField]
	private List<Image> background_ = new List<Image>();

	private enum DASH
	{
		Now,
		Max
	}

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	private void awake()
	{
		base.Awake();
	}

	void Start()
    {
		background_[(int)DASH.Now].fillAmount = 0;
		background_[(int)DASH.Max].enabled = false;
	}

	public override void UpdateNormal()
	{
		background_[(int)DASH.Now].fillAmount = player_.Stamina / player_.StaminaMax;

		if (background_[(int)DASH.Now].fillAmount >= 1)
		{
			background_[(int)DASH.Max].enabled = true;
		}
		else
		{
			background_[(int)DASH.Max].enabled = false;
		}
	}
}
