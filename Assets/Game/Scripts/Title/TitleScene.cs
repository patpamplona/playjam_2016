using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour 
{
    public void OnPlayTapped()
    {
        SceneManager.LoadScene("GameScene");
    }
}