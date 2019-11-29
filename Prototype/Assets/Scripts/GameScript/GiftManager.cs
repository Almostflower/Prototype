using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftManager : BaseMonoBehaviour
{
	///////////////////////////////////////////////////////////////////
	/// プロトを見て変える
	/// <summary>
	/// 1本のゲージで見せるモード
	/// </summary>
	[SerializeField]
	private bool debug_one_time_ = false;
	///////////////////////////////////////////////////////////////////


	/// <summary>
	/// ギフトのprefab
	/// </summary>
	[SerializeField] private GameObject giftData;

    /// <summary>
    /// ステージのデータ
    /// </summary>
    [SerializeField] private GameObject stageManager;

    /// <summary>
    /// ギフト最大値
    /// </summary>
    [SerializeField, Tooltip("ギフトの最大値（1-10）")] private int giftMax;

    /// <summary>
    /// ギフトの生成オブジェクト
    /// </summary>
    private GameObject[] giftManager;
    private bool[] isExistence;
    private bool[] isPlayerHave;

    /// <summary>
    /// 空白ステージの座標
    /// </summary>
    private List<Vector3> giftArea;

	/// <summary>
	/// 最大値のスコア
	/// </summary>
	[SerializeField]
	private int badminscore_ = 0;

	/// <summary>
	/// 最低値のスコア
	/// </summary>
	[SerializeField]
	private int badmaxscore_ = 0;

	[SerializeField]
	private float badminlimittime_ = 0;

	[SerializeField]
	private float badmaxlimittime_ = 0;

	[SerializeField]
	private float dustminlimittime_ = 0;

	[SerializeField]
	private float dustmaxlimittime_ = 0;

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ギフトマネージャー初期化
    /// </summary>
    private void Start()
    {
		// ギフトのメモリ確保
		giftManager = new GameObject[giftMax];
        isExistence = new bool[giftMax];

        // ギフト生成可能座標のメモリ確保
        giftArea = new List<Vector3>();

        // ステージ空白の配列の生成
        for (int i = 0; i < stageManager.GetComponent<StageManager>().StageHeight; i++)
        {
            for (int j = 0; j < stageManager.GetComponent<StageManager>().StageWidth; j++)
            {
                if (!stageManager.GetComponent<StageManager>().StageArea(i, j).isExistence)
                {
                    //変更
                    //Giftと建物の支点が違った為、y座標調整しました。
                    Vector3 pos = stageManager.GetComponent<StageManager>().StageArea(i, j).position;
                    pos.y = 0.5f;
                    giftArea.Add(pos);
                }

            }
        }

        // ギフトの生成
        for (int i= 0; i < giftMax; i++)
        {
            Birth(i);
        }

    }

    /// <summary>
    /// ギフトマネージャー更新
    /// </summary>
    public override void UpdateNormal()
    {
        // ギフトの生存チェック
        for (int i = 0; i < giftMax; i++)
        {
            if (isExistence[i])
            {
                // 自然消滅
                if (giftManager[i].GetComponent<Gift>().GetDeathFlag())
                {
                    // ステージゲージを下げる

                    // ギフトの削除と生成
                    Delete(i);
                    Birth(i);

                }

                // プレイヤーが回収
                if(giftManager[i].GetComponent<Gift>().PlayerAbsorbFlag)
                {
                    // ステージゲージを上げる
                
                    // ギフトの削除と生成
                    Delete(i);
                    Birth(i);
                }

            }
        }
    }

    /// <summary>
    /// リストから一つランダムに取得する
    /// </summary>
    /// <returns>座標を取得する</returns>
    public Vector3 GetPositionFromList()
    {
        int index = UnityEngine.Random.Range(0, giftArea.Count);
        Vector3 target = giftArea[index];
        giftArea.RemoveAt(index);

        return target;
    }

    /// <summary>
    /// ギフト生成可能エリアを追加する
    /// </summary>
    /// <param name="pos"></param>
    public void AddListToGiftArea(Vector3 pos)
    {
        giftArea.Add(pos);
    }

    /// <summary>
    /// ギフトの削除
    /// </summary>
    /// <param name="index"></param>
    private void Delete(int index)
    {
        // 削除
        AddListToGiftArea(giftManager[index].transform.position);
        Destroy(giftManager[index]);
        isExistence[index] = false;
    }

    /// <summary>
    /// ギフトの生成
    /// </summary>
    /// <param name="index"></param>
    private void Birth(int index)
    {
		int badscore = Random.Range(badminscore_, badmaxscore_);
		float badlimittime = Random.Range(badminlimittime_, badmaxlimittime_);
		float dustlimittime = Random.Range(dustminlimittime_, dustmaxlimittime_);

        giftManager[index] = Instantiate(giftData, GetPositionFromList(), Quaternion.identity);
        giftManager[index].transform.parent = this.transform;
		giftManager[index].GetComponent<Gift>().SetGift(badscore, badlimittime, dustlimittime);
		giftManager[index].GetComponent<Gift>().DebugOneTime = debug_one_time_;
        isExistence[index] = true;
    }
}
