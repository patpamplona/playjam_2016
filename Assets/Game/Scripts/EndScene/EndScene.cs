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
	}

    public void OnReplay()
    {
        SceneManager.LoadScene("GameScene");
    }
}