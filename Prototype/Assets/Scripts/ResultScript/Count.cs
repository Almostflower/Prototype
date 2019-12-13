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

    /// <summary>
    /// ギフトとウサギの数の表示までの時間
    /// </summary>
    [SerializeField]private int[] scoreTime;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float counter;

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
        counter = 0.0f;

    }

	void Update()
	{
        counter += Time.deltaTime;

        // 一つ目
        if (counter >= scoreTime[0])
        {
            image_[(int)Item.GiftGood].SetNo(Score.GetGiftGood());
        }
        else
        {
            image_[(int)Item.GiftGood].SetNo(Random.Range(0, 100));
        }

        // 二つ目
        if (counter >= scoreTime[1])
        {
            image_[(int)Item.GiftBad].SetNo(Score.GetGiftBad());
        }
        else
        {
            image_[(int)Item.GiftBad].SetNo(Random.Range(0, 100));
        }

        // 三つ目
        if (counter >= scoreTime[2])
        {

            image_[(int)Item.RabbitGood].SetNo(Score.GetRabbitGood());
        }
        else
        {
            image_[(int)Item.RabbitGood].SetNo(Random.Range(0, 100));
        }

        // 四つ目
        if (counter >= scoreTime[3])
        {
            image_[(int)Item.RabbitBad].SetNo(Score.GetRabbitBad());
        }
        else
        {
            image_[(int)Item.RabbitBad].SetNo(Random.Range(0, 100));
        }

        
    }

    
}
