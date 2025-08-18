using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Separate form the rest of the managers;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (!instance && SceneManager.GetActiveScene().buildIndex != 0)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (playOnAwake)
                InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
        }
        else
            Destroy(gameObject);

    }


    [Header("Audio Source")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [Space]
    [SerializeField] private bool playOnAwake = false;
    private int bgmIndex;

    public void PlaySFX(int sfxToPlay, bool randomPitch = true)
    {
        if (sfxToPlay >= sfx.Length) return;

        if (randomPitch)
            sfx[sfxToPlay].pitch = Random.Range(.9f, 1.1f);

        sfx[sfxToPlay].Play();
    }

    public void PlayMusicIfNeeded()
    {
        if (!bgm[bgmIndex].isPlaying)
            PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int bgmToPlay, bool loop = true)
    {
        if (bgm.Length <= 0 || bgmToPlay < 0 || bgmToPlay >= bgm.Length) return;

        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
            bgm[i].loop = false;
        }

        bgmIndex = bgmToPlay;
        bgm[bgmToPlay].loop = loop;
        bgm[bgmToPlay].Play();
    }


    public void StopSFX(int sfxToStop) => sfx[sfxToStop].Stop();

    public void StopBGM(bool cancelInvoke = false)
    {
        for (int i = 0; i < bgm.Length; i++)
            bgm[i].Stop();

        if (cancelInvoke)
            CancelInvoke(nameof(PlayMusicIfNeeded));
    }

    public bool isPlayingBGM(int index) => bgm[index].isPlaying;
    

}
