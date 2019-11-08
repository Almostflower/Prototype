using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour
{
	////////////////// ここに出したい音を入力 ///////////////////////////
	/// <summary>
	/// フォルダ名と同じ名前のBGMを入力
	/// </summary>
	public enum BGMLabel
	{
		None,
		stage2,
		stage3
	}

	/// <summary>
	/// フォルダ名と同じ名前のSEを入力
	/// </summary>
	public enum SELabel
	{
		JumpVoice
	}
	////////////////// ここまでに出したい音を入力 ///////////////////////
	
	/// <summary>
	/// ファイルパス
	/// </summary>
	private const string BGM_PATH = "Audio/BGM";
	private const string SE_PATH = "Audio/SE";
	private const string SOUND_OBJECT_NAME = "SoundManager";
	/// <summary>
	/// 同時にならす数（BGM）
	/// </summary>
	private const int BGM_SOURCE_NUM = 1;
	/// <summary>
	/// 同時にならす数（SE）
	/// </summary>
	private const int SE_SOURCE_NUM = 5;
	/// <summary>
	/// フェードイン、フェードアウトにかかる時間です。
	/// </summary>
	private const float FADE_OUT_SECONDO = 0.5f;
	/// <summary>
	/// BGMの音量
	/// </summary>
	private const float BGM_VOLUME = 0.5f;
	/// <summary>
	/// SEの音量
	/// </summary>
	private const float SE_VOLUME = 0.3f;
	/// <summary>
	/// 次に鳴らす音の準備するところ
	/// </summary>
	private int nextSESourceNum = 0;
	/// <summary>
	/// BGMを鳴らす所
	/// </summary>
	private BGMLabel currentBGM = BGMLabel.None;
	/// <summary>
	/// デバッグモード
	/// </summary>
	public bool DebugMode = true;
	/// <summary>
	/// BGM再生音量
	/// 次回フェードインから適用されます。
	/// 再生中の音量を変更するには、CurrentAudioSource.Volumeを変更してください。
	/// </summary>
	[Range(0f, 1f)]
	public float TargetVolume = 1.0f;
	/// <summary>
	/// フェードイン、フェードアウトにかかる時間です。
	/// </summary>
	public float TimeToFade = 2.0f;
	/// <summary>
	/// フェードインとフェードアウトの実行を重ねる割合です。
	/// 0を指定すると、完全にフェードアウトしてからフェードインを開始します。
	/// 1を指定すると、フェードアウトとフェードインを同時に開始します。
	/// </summary>
	[Range(0f, 1f)]
	public float CrossFadeRatio = 1.0f;
	/// <summary>
	/// 現在再生中のAudioSource
	/// FadeOut中のものは除く
	/// </summary>
	[System.NonSerialized]
	public AudioSource CurrentAudioSource = null;
	/// <summary>
	/// FadeOut中、もしくは再生待機中のAudioSource
	/// </summary>
	public AudioSource SubAudioSource
	{
		get
		{
			//bgmSourcesのうち、CurrentAudioSourceでない方を返す
			if (this.bgmSource == null)
				return null;
			foreach (AudioSource s in this.bgmSource)
			{
				if (s != this.CurrentAudioSource)
				{
					return s;
				}
			}
			return null;
		}
	}

	/// <summary>
	/// コルーチン中断に使用
	/// </summary>
	private IEnumerator fadeOutCoroutine;
	/// <summary>
	/// コルーチン中断に使用
	/// </summary>
	private IEnumerator fadeInCoroutine;
	/// <summary>
	/// BGMは一つづつ鳴るが、SEは複数同時に鳴ることがある
	/// </summary>
	private List<AudioSource> seSourceList;
	/// <summary>
	/// BGMを再生するためのAudioSourceです。
	/// クロスフェードを実現するための２つの要素を持ちます。
	/// </summary>
	private List<AudioSource> bgmSource = null;
	/// <summary>
	/// 再生可能なBGM or SE(AudioClip)のリストです。
	/// 実行時に Resources/Audio/BGM or SEフォルダから自動読み込みされます。
	/// </summary>
	private Dictionary<string, AudioClip> seClipDic;
	private Dictionary<string, AudioClip> bgmClipDic;
	private static SoundManager singletonInstance = null;

	/// <summary>
	/// シングルトン
	/// </summary>
	public static SoundManager SingletonInstance
	{
		get
		{
			if (!singletonInstance)
			{
				GameObject obj = new GameObject(SOUND_OBJECT_NAME);
				singletonInstance = obj.AddComponent<SoundManager>();
				DontDestroyOnLoad(obj);
			}
			return singletonInstance;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < SE_SOURCE_NUM + BGM_SOURCE_NUM; i++)
		{
			gameObject.AddComponent<AudioSource>();
		}

		IEnumerable<AudioSource> audioSources = GetComponents<AudioSource>().Select(a => { a.playOnAwake = false; a.volume = BGM_VOLUME; a.loop = true; return a; });
		//AudioSourceを２つ用意。クロスフェード時に同時再生するために２つ用意する。
		this.bgmSource = new List<AudioSource>();
		this.bgmSource.Add(this.gameObject.AddComponent<AudioSource>());
		this.bgmSource.Add(this.gameObject.AddComponent<AudioSource>());
		foreach (AudioSource s in this.bgmSource)
		{
			s.playOnAwake = false;
			s.volume = 0f;
			s.loop = true;
		}
		seSourceList = audioSources.Skip(BGM_SOURCE_NUM).ToList();
		seSourceList.ForEach(a => { a.volume = SE_VOLUME; a.loop = false; });

		bgmClipDic = (Resources.LoadAll(BGM_PATH) as Object[]).ToDictionary(bgm => bgm.name, bgm => (AudioClip)bgm);
		seClipDic = (Resources.LoadAll(SE_PATH) as Object[]).ToDictionary(se => se.name, se => (AudioClip)se);

		//有効なAudioListenerが一つも無い場合は生成する。（大体はMainCameraについてる）
		if (FindObjectsOfType(typeof(AudioListener)).All(o => !((AudioListener)o).enabled))
		{
			this.gameObject.AddComponent<AudioListener>();
		}
	}

	/// <summary>
	/// デバッグ用操作パネルを表示
	/// </summary>
	public void OnGUI()
	{
		if (this.DebugMode)
		{
			//AudioClipが見つからなかった場合
			if (this.bgmClipDic.Count == 0)
			{
				GUI.Box(new Rect(10, 10, 200, 50), "BGM Manager(Debug Mode)");
				GUI.Label(new Rect(10, 35, 80, 20), "Audio clips not found.");
				return;
			}

			//枠
			GUI.Box(new Rect(10, 10, 200, 150 + this.bgmClipDic.Count * 25), "BGM Manager(Debug Mode)");
			int i = 0;
			GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Target Volume : " + this.TargetVolume.ToString("0.00"));
			GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Time to Fade : " + this.TimeToFade.ToString("0.00"));
			GUI.Label(new Rect(20, 30 + i++ * 20, 180, 20), "Crossfade Ratio : " + this.CrossFadeRatio.ToString("0.00"));

			i = 0;
			//再生ボタン
			foreach (AudioClip bgm in this.bgmClipDic.Values)
			{
				bool currentBgm = (this.CurrentAudioSource != null && this.CurrentAudioSource.clip == this.bgmClipDic[bgm.name]);

				if (GUI.Button(new Rect(20, 100 + i * 25, 40, 20), "Play"))
				{
					this.PlayBGM((BGMLabel)bgm.length);
				}
				string txt = string.Format("[{0}] {1}", currentBgm ? "X" : "_", bgm.name);
				GUI.Label(new Rect(70, 100 + i * 25, 1000, 20), txt);

				i++;
			}

			//停止ボタン
			if (GUI.Button(new Rect(20, 100 + i++ * 25, 180, 20), "Stop"))
			{
				this.Stop();
			}
			if (GUI.Button(new Rect(20, 100 + i++ * 25, 180, 20), "Stop Immediately"))
			{
				this.StopImmediately();
			}
		}
	}

	/// <summary>
	/// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
	/// </summary>
	/// /// <param name="seLabel"></param>
	/// /// <param name="delay"></param>
	public void PlaySE(SELabel seLabel, float delay = 0.0f) => StartCoroutine(DelayPlaySE(seLabel, delay));

	/// <summary>
	/// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
	/// </summary>
	/// /// <param name="seLabel"></param>
	/// /// <param name="delay"></param>
	private IEnumerator DelayPlaySE(SELabel seLabel, float delay)
	{
		yield return new WaitForSeconds(delay);
		AudioSource se = seSourceList[nextSESourceNum];
		se.PlayOneShot(seClipDic[seLabel.ToString()]);
		nextSESourceNum = (++nextSESourceNum < SE_SOURCE_NUM) ? nextSESourceNum : 0;
	}

	/// <summary>
	/// BGMを再生します。
	/// </summary>
	/// <param name="bgmName">BGM名</param>
	public void PlayBGM(BGMLabel bgmLabel)
	{
		if (!this.bgmClipDic.ContainsKey(bgmLabel.ToString()))
		{
			Debug.LogError($"bgmClipDicに{bgmLabel.ToString()}というKeyはありません");
			return;
		}

		if ((this.CurrentAudioSource != null)
			&& (this.CurrentAudioSource.clip == this.bgmClipDic[bgmLabel.ToString()]))
		{
			//すでに指定されたBGMを再生中
			return;
		}

		//クロスフェード中なら中止
		stopFadeOut();
		stopFadeIn();

		//再生中のBGMをフェードアウト開始
		this.Stop();

		float fadeInStartDelay = this.TimeToFade * (1.0f - this.CrossFadeRatio);

		//BGM再生開始
		this.CurrentAudioSource = this.SubAudioSource;
		this.CurrentAudioSource.clip = this.bgmClipDic[bgmLabel.ToString()];
		this.fadeInCoroutine = fadeIn(this.CurrentAudioSource, this.TimeToFade, this.CurrentAudioSource.volume, this.TargetVolume, fadeInStartDelay);
		StartCoroutine(this.fadeInCoroutine);
	}

	/// <summary>
	///	全部の音を止める
	/// </summary>
	public void StopSound()
	{
		this.fadeInCoroutine = null;
		this.fadeOutCoroutine = null;
		foreach (AudioSource s in this.bgmSource)
		{
			s.Stop();
		}
		this.CurrentAudioSource = null;
		seSourceList.ForEach(a => { a.Stop(); });
	}

	/// <summary>
	/// BGMを停止します。
	/// </summary>
	public void Stop()
	{
		if (this.CurrentAudioSource != null)
		{
			this.fadeOutCoroutine = fadeOut(this.CurrentAudioSource, this.TimeToFade, this.CurrentAudioSource.volume, 0f);
			StartCoroutine(this.fadeOutCoroutine);
		}
	}

	/// <summary>
	/// BGMをただちに停止します。
	/// </summary>
	public void StopImmediately()
	{
		this.fadeInCoroutine = null;
		this.fadeOutCoroutine = null;
		foreach (AudioSource s in this.bgmSource)
		{
			s.Stop();
		}
		this.CurrentAudioSource = null;
	}

	/// <summary>
	/// 全SEを止める
	/// </summary>
	public void StopSE()
	{
		seSourceList.ForEach(a => { a.Stop(); });
	}

	/// <summary>
	/// BGMをフェードインさせながら再生を開始します。
	/// </summary>
	/// <param name="bgm">AudioSource</param>
	/// <param name="timeToFade">フェードインにかかる時間</param>
	/// <param name="fromVolume">初期音量</param>
	/// <param name="toVolume">フェードイン完了時の音量</param>
	/// <param name="delay">フェードイン開始までの待ち時間</param>
	private IEnumerator fadeIn(AudioSource bgm, float timeToFade, float fromVolume, float toVolume, float delay)
	{
		if (delay > 0)
		{
			yield return new WaitForSeconds(delay);
		}


		float startTime = Time.time;
		bgm.Play();
		while (true)
		{
			float spentTime = Time.time - startTime;
			if (spentTime > timeToFade)
			{
				bgm.volume = toVolume;
				this.fadeInCoroutine = null;
				break;
			}

			float rate = spentTime / timeToFade;
			float vol = Mathf.Lerp(fromVolume, toVolume, rate);
			bgm.volume = vol;
			yield return null;
		}
	}

	/// <summary>
	/// BGMをフェードアウトし、その後停止します。
	/// </summary>
	/// <param name="bgm">フェードアウトさせるAudioSource</param>
	/// <param name="timeToFade">フェードアウトにかかる時間</param>
	/// <param name="fromVolume">フェードアウト開始前の音量</param>
	/// <param name="toVolume">フェードアウト完了時の音量</param>
	private IEnumerator fadeOut(AudioSource bgm, float timeToFade, float fromVolume, float toVolume)
	{
		float startTime = Time.time;
		while (true)
		{
			float spentTime = Time.time - startTime;
			if (spentTime > timeToFade)
			{
				bgm.volume = toVolume;
				bgm.Stop();
				this.fadeOutCoroutine = null;
				break;
			}

			float rate = spentTime / timeToFade;
			float vol = Mathf.Lerp(fromVolume, toVolume, rate);
			bgm.volume = vol;
			yield return null;
		}
	}

	/// <summary>
	/// フェードイン処理を中断します。
	/// </summary>
	private void stopFadeIn()
	{
		if (this.fadeInCoroutine != null)
			StopCoroutine(this.fadeInCoroutine);
		this.fadeInCoroutine = null;

	}

	/// <summary>
	/// フェードアウト処理を中断します。
	/// </summary>
	private void stopFadeOut()
	{
		if (this.fadeOutCoroutine != null)
			StopCoroutine(this.fadeOutCoroutine);
		this.fadeOutCoroutine = null;
	}
}