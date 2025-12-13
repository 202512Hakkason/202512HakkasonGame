// BandCollider.cs
using UnityEngine;

public class BandCollider : MonoBehaviour
{
    [Header("このコライダーが表す層のインデックス")]
    // このコライダーが表す層のインデックス (0, 1, 2, ...)
    public int bandIndex = 0;

    [Header("Quadの制御スクリプトへの参照")]
    // BandFlashControllerを参照して、このコライダーが「正解」かどうかを判定する
    public BandFlashController bandController;

    void Start()
    {
        // 念のためコライダーがトリガー設定されていることを確認
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"BandCollider ({gameObject.name}) のコライダーがTriggerに設定されていなかったので設定しました。");
        }

        // Rendererを無効にして見えないようにする
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
}