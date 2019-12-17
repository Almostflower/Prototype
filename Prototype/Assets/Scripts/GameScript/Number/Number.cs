using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
	//色
	[SerializeField]
	private Color color_ = Color.white;

	//位置
	[SerializeField]
	private Vector3 pos_ = new Vector3(0, 0, 0);

	//間隔
	[SerializeField]
	private float span_ = 0;

	//大きさ
	[SerializeField]
	private Vector3 scale_ = new Vector3(1, 1, 1);

	// Start is called before the first frame update
	void Start()
	{
	}

	public void SetNum(int point)
	{
		foreach (Transform child in this.transform)
		{
			if (child.name == "Numbers(Clone)")
			{
				GameObject.Destroy(child.gameObject);
			}
		}

		//表示用のダメージを作る
		CreateNum(point);
	}

	//描画用の数字を作る
	private void CreateNum(int point)
	{

		//桁を割り出す
		int digit = ChkDigit(point);

		//数字プレハブを読み込む、テスト用のフォルダとファイル名
		GameObject obj = LoadGObject("Pref", "Numbers");

		//桁の分だけオブジェクトを作り登録していく
		for (int i = 0; i < digit; i++)
		{
			GameObject numObj = Instantiate(obj) as GameObject;

			Vector3 position = Vector3.zero;

			//子供として登録
			numObj.transform.parent = transform;

			//現在チェックしている桁の数字を割り出す
			int digNum = GetPointDigit(point, i + 1);

			//ポイントから数字を切り替える
			numObj.GetComponent<NumCtrl>().ChangeSprite(digNum);

			//位置をずらす
			position.x = i * span_ + pos_.x;
			position.y = pos_.y;

			numObj.transform.localPosition = position;

			numObj.transform.localScale = scale_;

			numObj.GetComponent<Image>().color = color_;

			numObj = null;
		}

	}


	// Update is called once per frame
	void Update()
	{
	}

	/**-----------------------------------------------------------------------------------
     * 以下の関数はテスト用にここに記載しているけど、別のスクリプトファイルとしたほうが使い勝手がいいかも
     * ----------------------------------------------------------------------------------/

    /**
    * 整数の桁数を返す
    * */
	public static int ChkDigit(int num)
	{

		//0の場合1桁として返す
		if (num == 0) return 1;

		//対数とやらを使って返す
		return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);

	}
	/**
    * 数値の中から指定した桁の値をかえす
    * */
	public static int GetPointDigit(int num, int digit)
	{

		int res = 0;
		int pow_dig = (int)Mathf.Pow(10, digit);
		if (digit == 1)
		{
			res = num - (num / pow_dig) * pow_dig;
		}
		else
		{
			res = (num - (num / pow_dig) * pow_dig) / (int)Mathf.Pow(10, (digit - 1));
		}

		return res;
	}
	/**
    * オブジェクトを読み込む
    * リソースフォルダから読み込む
    * */
	public static GameObject LoadGObject(string dir_name, string filename)
	{

		GameObject obj;

		//リソースから読み込むパターン
		obj = (GameObject)Resources.Load(dir_name + "/" + filename);

		return obj;

	}
}
