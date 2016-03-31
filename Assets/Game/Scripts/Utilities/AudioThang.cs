using UnityEngine;
using System.Collections;

public class AudioThang : MonoBehaviour
{
    private static AudioThang _instance = null;
    public static AudioThang Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private AudioSource bgm_mi;
    [SerializeField] private AudioSource bgm_ffx;
    [SerializeField] private AudioSource bgm_mario;
    [SerializeField] private AudioSource correct;
    [SerializeField] private AudioSource incorrect;
    [SerializeField] private AudioSource angryCat;
    [SerializeField] private AudioSource angryDog;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void PlayMainBGM()
    {
        this.bgm_ffx.Stop();
        this.bgm_mario.Stop();
        this.bgm_mi.Play();
    }

    public void PlayVictory()
    {
        this.bgm_ffx.Play();
        this.bgm_mario.Stop();
        this.bgm_mi.Stop();
    }

    public void PlayGameOver()
    {
        this.bgm_ffx.Stop();
        this.bgm_mario.Play();
        this.bgm_mi.Stop();
    }

    public void PlayCorrect()
    {
        this.incorrect.Stop();
        this.correct.Play();
    }

    public void PlayIncorrect()
    {
        this.correct.Stop();
        this.incorrect.Play();
    }

    public void PlayAngryDog()
    {
        this.angryDog.Play();
    }

    public void PlayAngryCat()
    {
        this.angryCat.Play();
    }
}