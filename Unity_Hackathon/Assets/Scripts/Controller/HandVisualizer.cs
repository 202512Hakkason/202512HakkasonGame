// HandVisualizer.cs
// UDPReceiverから受信したy座標データでオブジェクトを動かす
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
	// UDPReceiverの参照（Inspectorでセット）
	public UDPReceiver udpReceiver;

	// ワールドy座標の範囲（例: -3〜3）
	public Vector2 worldYRange = new Vector2(-3f, 3f);
	// x座標は固定値
	public float fixedX = 0f;
	// 補間速度
	public float lerpSpeed = 5f;

	// 目標位置
	private Vector3 targetPosition;

	void Start()
	{
		targetPosition = transform.position;
	}

	void Update()
	{
		if (udpReceiver == null) return;
		string yStr = udpReceiver.latestYString;
		if (string.IsNullOrEmpty(yStr)) return;

		if (float.TryParse(yStr, out float yNorm))
		{
			float mappedY = Mathf.Lerp(worldYRange.x, worldYRange.y, Mathf.Clamp01(1-yNorm));
			targetPosition = new Vector3(fixedX, mappedY, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
		}
	}
}
