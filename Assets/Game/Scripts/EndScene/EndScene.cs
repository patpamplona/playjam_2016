using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

	void Start () 
    {
        winScreen.SetActive(InfoMessenger.DidPlayerWin);
        loseScreen.SetActive(!InfoMessenger.DidPlayerWin);

        if(AudioThang.Instance != null)
        {
            if(InfoMessenger.DidPlayerWin)
            {
                AudioThang.Instance.PlayVictory();
            }
            else
            {
                AudioThang.Instance.PlayGameOver();
            }
        }
	}

    public void OnReplay()
    {
        if(AudioThang.Instance != null)
        {
            AudioThang.Instance.PlayMainBGM();
        }
        SceneManager.LoadScene("GameScene");
    }
}