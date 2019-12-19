using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReady : BaseMonoBehaviour
{
	/// <summary>
	/// アニメーション
	/// </summary>
	[SerializeField]
	private List<Animator> animstart_ = new List<Animator>();

	[SerializeField]
	private GameObject start_ready_;

	/// <summary>
	/// レディーフラグ
	/// </summary>
	private bool start_ = false;
	public bool SetStart
	{
		set { start_ = value; }
	}

	/// <summary>
	/// ゴーフラグ
	/// </summary>
	private bool go_ = false;
	public bool GetGo
	{
		get { return go_; }
	}

	enum START_
	{
		Ready,
		Go
	}

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}

	void Start()
    {
		start_ = false;
		go_ = false;
	}

	public override void UpdateNormal()
	{
		if (start_)
		{
			animstart_[(int)START_.Ready].SetBool("Readyflag", true);
		}
	}

	/// <summary>
	/// レディーのアニメーションが終わったらゴーのアニメーションを実行
	/// </summary>
	public void GoAimStart()
	{
		animstart_[(int)START_.Go].SetBool("Goflag", true);
		go_ = true;
	}

	public void DestroyAnim()
	{
		Destroy(start_ready_);
	}
}
