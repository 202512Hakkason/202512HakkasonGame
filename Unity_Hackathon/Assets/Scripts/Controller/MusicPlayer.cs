using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("UDP受信")]
    public UDPReceiver udpReceiver;

    [Header("音声クリップ（下→上）")]
    public AudioClip do_mp3;
    public AudioClip re_mp3;
    public AudioClip mi_mp3;
    public AudioClip fa_mp3;
    public AudioClip so_mp3;
    public AudioClip la_mp3;

    private AudioSource[] audioSources;
    private int currentNoteIndex = -1; // 今鳴っている音（-1 = なし）

    void Start()
    {
        // AudioSourceを6個作る
        AudioClip[] clips = {
            do_mp3, re_mp3, mi_mp3, fa_mp3, so_mp3, la_mp3
        };

        audioSources = new AudioSource[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clips[i];
            source.loop = true;          // その場所にいる間は鳴り続ける
            source.playOnAwake = false;
            audioSources[i] = source;
        }
    }

    void Update()
    {
        if (udpReceiver == null) return;

        string yStr = udpReceiver.latestYString;
        if (string.IsNullOrEmpty(yStr)) return;

        if (!float.TryParse(yStr, out float yNorm)) return;

        // 0〜1 にクランプ
        float yNorm_reversed = Mathf.Clamp01(1-yNorm);

        // 6分割して音 index を決定
        int noteIndex = Mathf.FloorToInt(yNorm_reversed * 6);
        noteIndex = Mathf.Clamp(noteIndex, 0, 5);

        // 音が変わったときだけ処理
        if (noteIndex != currentNoteIndex)
        {
            // 前の音を止める
            if (currentNoteIndex >= 0)
            {
                audioSources[currentNoteIndex].Stop();
            }

            // 新しい音を鳴らす
            audioSources[noteIndex].Play();
            currentNoteIndex = noteIndex;
        }
    }
}
