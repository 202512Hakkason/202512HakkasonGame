// BodyData.cs
// UDPで受信した座標データ(JSON)を格納するクラス
using System;

[Serializable]
public class BodyData
{
	// x座標（0.0〜1.0）
	public float x;
	// y座標（0.0〜1.0）
	public float y;
}
