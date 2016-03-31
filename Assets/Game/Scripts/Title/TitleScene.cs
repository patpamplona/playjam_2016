using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour 
{
    void Start()
    {
        if(AudioThang.Instance != null)
        {
            AudioThang.Instance.PlayMainBGM();
        }
    }

    public void OnPlayTapped()
    {
        SceneManager.LoadScene("GameScene");
    }
}