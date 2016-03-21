using UnityEngine;
using System.Collections;

public class PauseOverlay : MonoBehaviour
{
    public void OnGameStarted()
    {
        GameplayManager.Instance.StartGame();
    }
}
