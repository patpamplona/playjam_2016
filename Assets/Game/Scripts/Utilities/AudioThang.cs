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

    [SerializeField] private AudioSource correct;
    [SerializeField] private AudioSource incorrect;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
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
}
