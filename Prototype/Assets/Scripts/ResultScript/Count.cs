using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Count : MonoBehaviour
{
	[SerializeField]
	private List<Number> number_ = new List<Number>();

	private float counter = 0;

	private float scoreTime = 0;

	private bool[] one_SE_ = new bool[(int)Item.Max];

	private bool play_SE_ = false;
	public bool LastCountSE
	{
		get { return one_SE_[(int)Item.RabbitBad]; }
	}

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
		for(int i = 0; i < (int)Item.Max; i++)
		{
			one_SE_[i] = false;
		}
		play_SE_ = false;
	}

	void Update()
	{
		counter += Time.deltaTime;

		//一つ目
		if (counter >= scoreTime)
		{
			number_[(int)Item.GiftGood].SetNum(Score.GetGiftGood());
			//number_[(int)Item.GiftGood].SetNum(1);

			//SE
			if (!one_SE_[(int)Item.GiftGood])
			{
				one_SE_[(int)Item.GiftGood] = true;
				play_SE_ = true;

				// ドラムロール
				SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Drumroll_SE);
			}
		}
		else
		{
			number_[(int)Item.GiftGood].SetNum(Random.Range(10, 100));
		}

		//二つ目
		if (counter >= scoreTime + 1)
		{
			number_[(int)Item.GiftBad].SetNum(Score.GetGiftBad());
			//number_[(int)Item.GiftBad].SetNum(2);

			//SE
			if (!one_SE_[(int)Item.GiftBad])
			{
				one_SE_[(int)Item.GiftBad] = true;
				play_SE_ = true;

				// ドラムロール
				SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Drumroll_SE);
			}
		}
		else
		{
			number_[(int)Item.GiftBad].SetNum(Random.Range(10, 100));		
		}

		//三つ目
		if (counter >= scoreTime + 2)
		{
			number_[(int)Item.RabbitGood].SetNum(Score.GetRabbitGood());
			//number_[(int)Item.RabbitGood].SetNum(3);

			//SE
			if (!one_SE_[(int)Item.RabbitGood])
			{
				one_SE_[(int)Item.RabbitGood] = true;
				play_SE_ = true;

				// ドラムロール
				SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Drumroll_SE);
			}
		}
		else
		{
			number_[(int)Item.RabbitGood].SetNum(Random.Range(10, 100));
		}

		//四つ目
		if (counter >= scoreTime + 3)
		{
			number_[(int)Item.RabbitBad].SetNum(Score.GetRabbitBad());
			//number_[(int)Item.RabbitBad].SetNum(9);

			//SE
			if (!one_SE_[(int)Item.RabbitBad])
			{
				one_SE_[(int)Item.RabbitBad] = true;
				play_SE_ = true;				
			}
		}
		else
		{
			number_[(int)Item.RabbitBad].SetNum(Random.Range(10, 100));
		}

		if(play_SE_)
		{
			// リザルトでギフトのドラムロール終わった音
			SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.GiftCountDecision_SE);
			SoundManager.SingletonInstance.AllStopSE();
			play_SE_ = false;
		}
	}
}
