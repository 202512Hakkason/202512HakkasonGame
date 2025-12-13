Shader "Custom/HeightBandFlash"
{
    Properties
        {
            _Color ("Base Color", Color) = (0.1, 0.1, 0.1, 1) // 基本の色 (暗めにして光が目立つように)
            _BandColor ("Active Band Color", Color) = (1, 0.5, 0, 1) // 点灯時の色 (例: オレンジ)
            _BandCount ("Number of Bands", Float) = 8

            // C#から渡す制御用パラメータ
            _ActiveBandIndex ("Active Band Index", Float) = -1 // 現在点灯させる層のインデックス (0〜7)
            _ColorStartTime ("Color Start Time", Float) = -100.0 // 点灯開始したゲーム時間 (Time.time)
            _ColorDuration ("Color Duration (s)", Range(0.01, 5)) = 0.3 // 色が付く継続時間
        }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // 【重要：エラー解消のための修正済み構造体】
            // 頂点位置のフィールド名を 'pos' に変更
            struct v2f 
            { 
                float2 uv : TEXCOORD0; 
                float4 pos : SV_POSITION; // 頂点位置
            };

            fixed4 _Color;
            fixed4 _BandColor;
            float _BandCount;
            float _ActiveBandIndex;
            float _ColorStartTime;
            float _ColorDuration;

            v2f vert (appdata v)
            {
                v2f o;
                // 【重要：エラー解消のための修正済み代入】
                // 代入先を 'o.pos' に変更
                o.pos = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. 現在のピクセルの層インデックスを計算 [0, bandCount-1]
                // Quadに適用されている場合、i.uv.yは下端で0.0、上端で1.0になります
                float normalizedY = i.uv.y; 
                float bandFloat = normalizedY * _BandCount; 
                float currentBandIndex = floor(bandFloat);

                // 2. 現在時刻と開始時刻から経過時間を計算
                float timeElapsed = _Time.y - _ColorStartTime;

                // 3. レイヤーインデックスが一致するか AND 継続時間内かを判定
                // (currentBandIndex == _ActiveBandIndex) : 今見ているピクセルがC#で指定された層か
                bool isTargetBand = (currentBandIndex == _ActiveBandIndex);
                
                // (timeElapsed < _ColorDuration) : 点灯継続時間内か
                // (timeElapsed > 0.0) : _ColorStartTimeが初期値(-100.0)や過去の非アクティブ化値でないか
                bool isTimeActive = (timeElapsed > 0.0) && (timeElapsed < _ColorDuration);

                // 4. 条件が揃っていればアクティブな色を適用
                if (isTargetBand && isTimeActive)
                {
                    return _BandColor;
                }
                
                // それ以外はベースカラー
                return _Color;
            }
            ENDCG
        }
    }
}