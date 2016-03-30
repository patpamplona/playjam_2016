using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string RUN_ANIM   = "_anim_run";
    private const string PANIC_ANIM = "_anim_panic";

    [SerializeField] private Animation animation;

    public void Run()
    {
        animation.Play(RUN_ANIM);
    }

    public void Panic()
    {
        
    }

    public void UsePaw(Vector3 target)
    {
        
    }
}
