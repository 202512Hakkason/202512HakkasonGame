using UnityEngine;

public class MusicPlayer_time : MonoBehaviour
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



    [Header("判定周期（秒）")]
    public float interval = 0.9f;   // 0.9秒ごとに音を鳴らす


    private AudioClip[] clips;
    private AudioSource audioSource;
    private float timer = 0f;

    void Start()
    {
        clips = new AudioClip[]
        {
            do_mp3, re_mp3, mi_mp3, fa_mp3, so_mp3, la_mp3
        };

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (udpReceiver == null) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer -= interval;
            PlayNoteByPosition();
        }
    }

    void PlayNoteByPosition()
    {
        string yStr = udpReceiver.latestYString;
        if (string.IsNullOrEmpty(yStr)) return;
        if (!float.TryParse(yStr, out float yNorm)) return;

        yNorm = Mathf.Clamp01(1-yNorm);

        int noteIndex = Mathf.FloorToInt(yNorm * 6);
        noteIndex = Mathf.Clamp(noteIndex, 0, 5);

        audioSource.PlayOneShot(clips[noteIndex]);
    }
}
