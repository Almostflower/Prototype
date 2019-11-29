using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
	/// <summary>
	/// UIの表示
	/// </summary>
	[SerializeField]
	private List<ImageNo> image_ = new List<ImageNo>();

	enum Item
	{
		GiftGood,
		GiftBad,
		RabbitGood,
		RabbitBad,
		Max
	}

	void Start()
    {
		image_[(int)Item.GiftGood].SetNo(Score.GetGiftGood());
		image_[(int)Item.GiftBad].SetNo(Score.GetGiftBad());
		image_[(int)Item.RabbitGood].SetNo(Score.GetRabbitGood());
		image_[(int)Item.RabbitBad].SetNo(Score.GetRabbitBad());
	}

	void Update()
	{
		
	}
}
