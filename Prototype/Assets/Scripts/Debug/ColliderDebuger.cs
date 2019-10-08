using UnityEngine;
using HC.Debug;


/// <summary>
/// 使用例
/// </summary>
[DisallowMultipleComponent]
public class ColliderDebuger : SingletonMonoBehaviour<ColliderDebuger>
{
    #region フィールド / プロパティ
    [SerializeField, Tooltip("可視化出来る個数")]
    private int MaxDrawCollider;

    [SerializeField, Tooltip("可視コライダーの色")]
    private ColliderVisualizer.VisualizerColorType[] _visualizerColor;

    [SerializeField, Tooltip("メッセージ")]
    private string[] _message;

    [SerializeField, Tooltip("フォントサイズ")]
    private int[] _fontSize;

    [SerializeField, Tooltip("当たり判定を可視化させる対象")]
    private GameObject[] _TargetOjbect;
    [SerializeField]
    private bool[] drawflag;
    #endregion

    #region アニメーションイベントメソッド

    public void ColliderDrawStart(int num)
    {
        if (!drawflag[num])
        {
            _TargetOjbect[num].AddComponent<ColliderVisualizer>().Initialize(_visualizerColor[num], _message[num], _fontSize[num]);
            drawflag[num] = true;
        }
    }

    public void ColliderDrawEnd(int num)
    {
        if(!drawflag[num])
        {
            Destroy(_TargetOjbect[num].GetComponent<ColliderVisualizer>());
            drawflag[num] = false;
        }
    }

    public int GetMaxColliderDraw()
    {
        return MaxDrawCollider;
    }
    #endregion
}