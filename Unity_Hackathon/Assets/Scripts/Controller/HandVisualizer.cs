// HandVisualizer.cs
// UDPReceiverから受信した座標データでオブジェクトを動かす
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
	// UDPReceiverの参照（Inspectorでセット）
	public UDPReceiver udpReceiver;

	// ワールド座標の範囲（例: x: -5〜5, y: -3〜3）
	public Vector2 worldXRange = new Vector2(-5f, 5f);
	public Vector2 worldYRange = new Vector2(-3f, 3f);
	// 補間速度
	public float lerpSpeed = 5f;

	// 目標位置
	private Vector3 targetPosition;

	void Start()
	{
		// 初期位置を現在位置に
		targetPosition = transform.position;
	}

	void Update()
	{
		if (udpReceiver == null) return;
		string json = udpReceiver.latestJson;
		if (string.IsNullOrEmpty(json)) return;

		// JSONをBodyDataに変換
		BodyData data = null;
		try
		{
			data = JsonUtility.FromJson<BodyData>(json);
		}
		catch
		{
			// パース失敗時は無視
			return;
		}
		if (data == null) return;

		// 0.0〜1.0の値をワールド座標に変換
		float mappedX = Mathf.Lerp(worldXRange.x, worldXRange.y, Mathf.Clamp01(data.x));
		float mappedY = Mathf.Lerp(worldYRange.x, worldYRange.y, Mathf.Clamp01(data.y));
		targetPosition = new Vector3(mappedX, mappedY, transform.position.z);

		// 現在位置から目標位置へ滑らかに移動
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
	}
}