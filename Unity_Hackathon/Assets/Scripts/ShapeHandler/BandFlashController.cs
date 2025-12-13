// BandFlashController.cs (CSV制御・ゼロ値非アクティブ化バージョン)
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq; // LINQを使うため

public class BandFlashController : MonoBehaviour
{
    // === 外部参照可能 ===
    [NonSerialized]
    public int ActiveBandIndex = -1; // 現在光っている層のインデックス (-1は非アクティブを示す)

    // === Inspectorで設定する項目 ===
    [Header("連携")]
    public Material targetMaterial; // シェーダーが適用されているマテリアル

    [Header("設定")]
    public TextAsset csvFile; // CSVファイルをInspectorにドラッグ&ドロップ
    public int bandCount = 6; // 分割数 (Shaderの_BandCountと合わせる。CSVの1~6に対応させるため6を推奨)
    public float flashInterval = 0.3f; // CSVの次の数字を読み込む間隔（秒）

    // === 内部変数 ===
    private List<int> bandData; // CSVから読み込んだ層のインデックスデータ (0〜6のまま保持)
    private int dataIndex = 0; // 現在読み込んでいるデータのインデックス
    private float nextFlashTime = 0f; // 次に層を切り替える時刻

    void Start()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("ターゲットマテリアルが設定されていません。");
            enabled = false;
            return;
        }

        // CSVデータの読み込みとパースを実行
        LoadCSVData();

        if (bandData == null || bandData.Count == 0)
        {
            Debug.LogError("CSVデータが見つからないか、読み込めませんでした。");
            enabled = false;
            return;
        }

        // Shaderの初期設定
        targetMaterial.SetFloat("_BandCount", (float)bandCount);
        // フラッシュ継続時間と切り替え間隔を合わせる
        targetMaterial.SetFloat("_ColorDuration", flashInterval);

        // 初回実行時刻を設定
        nextFlashTime = Time.time;
    }

    private void LoadCSVData()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSVファイル (TextAsset) が設定されていません。");
            bandData = new List<int>();
            return;
        }

        // TextAssetから文字列として内容を取得
        string text = csvFile.text;

        // カンマ、改行、スペースで分割
        string[] values = text.Split(new char[] { ',', '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // 読み込んだ文字列をintに変換
        bandData = values.Select(val =>
        {
            if (int.TryParse(val, out int index)) return index;
            return -1; // パース失敗時は無効な値として-1
        }).Where(index => index >= 0) // パース失敗(-1)を除外
          .ToList();

        Debug.Log($"CSVデータ {bandData.Count} 件を読み込みました。");
    }

    void Update()
    {
        // 1. 次のフラッシュ時刻が来ているか、かつ、データが残っているかをチェック
        if (Time.time >= nextFlashTime && bandData != null && bandData.Count > 0)
        {
            // データインデックスがリストの範囲外にならないように調整（ループ処理）
            if (dataIndex >= bandData.Count)
            {
                dataIndex = 0; // データが最後まで行ったら最初に戻る (ループ)
            }

            // CSVから読み込んだ値を取得 (例: 1, 5, 0, 6 など)
            int csvValue = bandData[dataIndex];

            // Shaderに送るためのfloat変数と開始時刻
            float activeBandIndexFloat;
            float colorStartTime;

            // 2. CSVの値に応じてシェーダーに送るインデックスを決定
            if (csvValue == 0)
            {
                // CSV値が 0 の場合: 色を消す
                ActiveBandIndex = -1; // 【CubeFeedback用】非アクティブを示す
                activeBandIndexFloat = -1;
                colorStartTime = -100f; // 遠い過去の時刻 (Shaderで非点灯判定させる)

            }
            else // CSV値が 1〜6 の場合
            {
                // CSVの値（1〜6）をシェーダーのインデックス（0〜5）に変換
                int targetIndex = csvValue - 1;

                // インデックスがbandCountの範囲内であることを保証
                if (targetIndex >= 0 && targetIndex < bandCount)
                {
                    ActiveBandIndex = targetIndex; // 【CubeFeedback用】アクティブなインデックス
                    activeBandIndexFloat = (float)targetIndex;
                    // 現在のゲーム時間 (ColorStartTime) - これがフラッシュ開始時間になる
                    colorStartTime = Time.time;
                }
                else
                {
                    // bandCountの範囲外の数値がCSVにあった場合
                    Debug.LogWarning($"CSV値 {csvValue} が設定された bandCount {bandCount} の範囲外です。スキップします。");

                    // 色を消す処理を適用
                    ActiveBandIndex = -1;
                    activeBandIndexFloat = -1;
                    colorStartTime = -100f;
                }
            }

            // 3. Shaderに新しい点灯情報を送る (★この行から下のロジックが欠落していました)
            targetMaterial.SetFloat("_ActiveBandIndex", activeBandIndexFloat);
            targetMaterial.SetFloat("_ColorStartTime", colorStartTime);

            // 4. 次の処理のためのインデックスと時刻を更新
            dataIndex++; // 次のデータへ
            nextFlashTime = Time.time + flashInterval; // 次のフラッシュ時刻を設定
        }
    }
}