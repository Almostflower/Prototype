using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
	/// <summary>
	/// UIの表示
	/// </summary>
	//[SerializeField]
	//private List<ImageNo> image_ = new List<ImageNo>();

	[SerializeField]
	private List<Number> number_ = new List<Number>();

	private float counter = 0;

	private float scoreTime = 0;

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
		//image_[(int)Item.GiftGood].SetNo(Score.GetGiftGood());
		//image_[(int)Item.GiftBad].SetNo(Score.GetGiftBad());
		//image_[(int)Item.RabbitGood].SetNo(Score.GetRabbitGood());
		//image_[(int)Item.RabbitBad].SetNo(Score.GetRabbitBad());

		//number_[(int)Item.GiftGood].SetNum(Score.GetGiftGood());
		//number_[(int)Item.GiftBad].SetNum(Score.GetGiftBad());
		//number_[(int)Item.RabbitGood].SetNum(Score.GetRabbitGood());
		//number_[(int)Item.RabbitBad].SetNum(Score.GetRabbitBad());
	}

	void Update()
	{
		counter += Time.deltaTime;

		//一つ目
		if (counter >= scoreTime)
		{
			number_[(int)Item.GiftGood].SetNum(Score.GetGiftGood());
		}
		else
		{
			number_[(int)Item.GiftGood].SetNum(Random.Range(10, 100));
		}

		//二つ目
		if (counter >= scoreTime + 1)
		{
			number_[(int)Item.GiftBad].SetNum(Score.GetGiftBad());
		}
		else
		{
			number_[(int)Item.GiftBad].SetNum(Random.Range(10, 100));
		}

		//三つ目
		if (counter >= scoreTime + 2)
		{

			number_[(int)Item.RabbitGood].SetNum(Score.GetRabbitGood());
		}
		else
		{
			number_[(int)Item.RabbitGood].SetNum(Random.Range(10, 100));
		}

		//四つ目
		if (counter >= scoreTime + 3)
		{
			number_[(int)Item.RabbitBad].SetNum(Score.GetRabbitBad());
		}
		else
		{
			number_[(int)Item.RabbitBad].SetNum(Random.Range(10, 100));
		}
	}
}
