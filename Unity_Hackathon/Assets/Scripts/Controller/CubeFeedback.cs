// CubeFeedback.cs (マテリアル置き換えバージョン)
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CubeFeedback : MonoBehaviour
{
    [Header("視覚フィードバック")]
    // ★ 正解時に切り替えるマテリアル (Inspectorで設定)
    public Material correctMaterial;

    [Header("マテリアル設定")]
    // ★ 不正解時に戻す元のマテリアル (Inspectorで設定)
    public Material defaultMaterial;

    private Renderer cubeRenderer;

    // 聴覚フィードバック
    private AudioSource audioSource;
    public AudioClip[] scaleClips;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();

        if (cubeRenderer == null || defaultMaterial == null)
        {
            Debug.LogError("Rendererコンポーネント、またはDefault Materialが設定されていません。");
            enabled = false;
            return;
        }

        audioSource = GetComponent<AudioSource>();

        // Cube自体にRigidbodyとColliderが必要なことを確認
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Cubeには衝突判定のためにRigidbodyが必要です。");
        }

        // 開始時、CubeのRendererにdefaultMaterialが設定されていることを確認/設定
        // (sharedMaterialの比較前に、materialを初期化)
        if (cubeRenderer.sharedMaterial != defaultMaterial)
        {
            cubeRenderer.material = defaultMaterial;
        }
    }

    // ----------------------------------------------------
    // Cubeがコライダーに入ったとき (音の制御のみ)
    // ----------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        BandCollider bandCol = other.GetComponent<BandCollider>();
        if (bandCol != null)
        {
            int enteredBandIndex = bandCol.bandIndex;

            // 音の再生（層に入ったら音を出す）
            if (audioSource != null && scaleClips != null && enteredBandIndex < scaleClips.Length)
            {
                // 既に別の音が再生中でない場合のみ再生
                if (!audioSource.isPlaying)
                {
                    AudioClip clip = scaleClips[enteredBandIndex];
                    if (clip != null)
                    {
                        audioSource.clip = clip;
                        audioSource.Play();
                    }
                }
            }
        }
    }

    // ----------------------------------------------------
    // Cubeがコライダーの中にいる間、毎フレーム実行 (色の継続判定用)
    // ----------------------------------------------------
    void OnTriggerStay(Collider other)
    {
        BandCollider bandCol = other.GetComponent<BandCollider>();
        if (bandCol != null)
        {
            int enteredBandIndex = bandCol.bandIndex;

            // 正解の層のインデックスをリアルタイムで取得
            int correctBandIndex = bandCol.bandController.ActiveBandIndex;

            // 1. 正解判定（マテリアルを切り替える）
            if (enteredBandIndex == correctBandIndex)
            {
                // 正解の場合: マテリアルを正解色に切り替え
                // sharedMaterialで比較し、materialで代入することで、インスペクター設定が上書きされる
                if (cubeRenderer.sharedMaterial != correctMaterial)
                {
                    cubeRenderer.material = correctMaterial;
                }
            }
            else
            {
                // 不正解の場合: マテリアルを元の色に戻す
                if (cubeRenderer.sharedMaterial != defaultMaterial)
                {
                    cubeRenderer.material = defaultMaterial;
                }
            }
        }
    }


    // ----------------------------------------------------
    // Cubeがコライダーから出たとき (音と色のリセット)
    // ----------------------------------------------------
    void OnTriggerExit(Collider other)
    {
        BandCollider bandCol = other.GetComponent<BandCollider>();
        if (bandCol != null)
        {
            // コライダーから出たら、マテリアルを元の色に戻す
            if (cubeRenderer.sharedMaterial != defaultMaterial)
            {
                cubeRenderer.material = defaultMaterial;
            }

            // コライダーから出たら音を停止する
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}